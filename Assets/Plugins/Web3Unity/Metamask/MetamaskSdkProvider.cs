using Cysharp.Threading.Tasks;
using MetaMask.Models;
using MetaMask.Unity;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RpcError = Nethereum.JsonRpc.Client.RpcError;

namespace Web3Unity
{
    public class MetamaskSdkProvider : IClient
    {
        public RequestInterceptor OverridingRequestInterceptor { get; set; }

        private static UniTaskCompletionSource<string> utcsConnected;
        private static Dictionary<int, UniTaskCompletionSource<string>> utcs = new Dictionary<int, UniTaskCompletionSource<string>>();
        private static int id = 0;

        public static event EventHandler<string> OnAccountConnected;
        public static event EventHandler<string> OnAccountChanged;
        public static event EventHandler<string> OnChainChanged;
        public static event EventHandler OnAccountDisconnected;

        public MetamaskSdkProvider(bool autoConnect)
        {
            MetaMaskUnity.Instance.Initialize();
            if (autoConnect)
            {
                ConnectAccount();
            }
        }

        public async UniTask<string> ConnectAccount()
        {
            utcsConnected = new UniTaskCompletionSource<string>();
            var wallet = MetaMaskUnity.Instance.Wallet;
            wallet.WalletConnectedHandler += OnWalletConnected;
            wallet.WalletDisconnectedHandler += OnWalletDisconnected;
            wallet.AccountChangedHandler += OnWalletAccountChanged;
            wallet.ChainIdChangedHandler += OnWalletChainIdChanged;
            wallet.Connect();
            string result = await utcsConnected.Task;
            return result;
        }

        void OnWalletConnected(object sender, EventArgs e)
        {
            var wallet = MetaMaskUnity.Instance.Wallet;
            utcsConnected?.TrySetResult(wallet.SelectedAddress);

            if (OnAccountConnected != null)
            {
                OnAccountConnected(Web3Connect.Instance.MetamaskInstance, wallet.SelectedAddress);
            }
        }

        void OnWalletDisconnected(object sender, EventArgs e)
        {
            if (OnAccountDisconnected != null)
            {
                OnAccountDisconnected(Web3Connect.Instance.MetamaskInstance, new EventArgs());
            }
        }

        void OnWalletAccountChanged(object sender, EventArgs e)
        {
            var wallet = MetaMaskUnity.Instance.Wallet;
            if (OnAccountChanged != null)
            {
                OnAccountChanged(Web3Connect.Instance.MetamaskInstance, wallet.SelectedAddress);
            }
        }

        void OnWalletChainIdChanged(object sender, EventArgs e)
        {
            var wallet = MetaMaskUnity.Instance.Wallet;
            if (OnChainChanged != null)
            {
                OnChainChanged(Web3Connect.Instance.MetamaskInstance, wallet.SelectedChainId);
            }
        }

        public async UniTask<RpcRequestResponseBatch> SendBatchRequestAsync(RpcRequestResponseBatch rpcRequestResponseBatch)
        {
            foreach (var i in rpcRequestResponseBatch.BatchItems)
            {
                var request = i.RpcRequestMessage;
                RpcResponseMessage response = await SendAsync(request.Method, request.RawParameters);
                var resp = new RpcResponseMessage(request.Id, response.Result);
                rpcRequestResponseBatch.UpdateBatchItemResponses(new List<RpcResponseMessage>() { resp });
            }
            return rpcRequestResponseBatch;
        }

        public async UniTask<T> SendRequestAsync<T>(RpcRequest request, string route = null)
        {
            Debug.Log($"SendRequestAsync T {typeof(T)}");
            RpcResponseMessage response = await SendAsync(request.Method, request.RawParameters);
            Debug.Log($"Response  {response.Result}");
            try
            {
                var result = response.GetResult<T>();
                Debug.Log($"Result  {result}");
                return result;
            }
            catch (FormatException formatException)
            {
                throw new RpcResponseFormatException("Invalid format found in RPC response", formatException);
            };
        }

        public async UniTask<T> SendRequestAsync<T>(string method, string route = null, params object[] paramList)
        {
            RpcResponseMessage response = await SendAsync(method, paramList);
            Debug.Log($"SendRequestAsync Method T {typeof(T)}");
            try
            {
                return response.GetResult<T>();
            }
            catch (FormatException formatException)
            {
                throw new RpcResponseFormatException("Invalid format found in RPC response", formatException);
            }
        }

        public async UniTask SendRequestAsync(RpcRequest request, string route = null)
        {
            await SendAsync(request.Method, request.RawParameters);
        }

        public async UniTask SendRequestAsync(string method, string route = null, params object[] paramList)
        {
            await SendAsync(method, paramList);
        }

        private async UniTask<RpcResponseMessage> SendAsync(string method, params object[] paramList)
        {
            int val = ++id;
            var wallet = MetaMaskUnity.Instance.Wallet;
            var account = wallet.SelectedAddress;
            var transactionParams = new MetaMaskTransaction() { From = account };

            if (paramList != null && paramList.Length > 0)
            {
                var callInput = paramList[0] as CallInput;
                if (callInput != null)
                {
                    transactionParams.To = callInput.To;
                    transactionParams.Data = callInput.Data;
                }
                else
                {
                    var transactionInput = paramList[0] as TransactionInput;
                    if (transactionInput != null)
                    {
                        transactionParams.To = transactionInput.To;
                        transactionParams.Data = transactionInput.Data;
                        transactionParams.Value = transactionInput.Value.ToString();
                    }
                }
            }

            var request = new MetaMaskEthereumRequest
            {
                Method = method,
                Parameters = new MetaMaskTransaction[] { transactionParams }
            };
            RpcResponseMessage response = await RequestCallAsync(val, request);
            HandleRpcError(response, method);
            return response;
        }

        public async UniTask<RpcResponseMessage> RequestCallAsync(int val, MetaMaskEthereumRequest request)
        {
            var wallet = MetaMaskUnity.Instance.Wallet;
            utcs[val] = new UniTaskCompletionSource<string>();
            var requestResult = await wallet.Request(request);
            string result = await utcs[val].Task;
            utcs[val].TrySetResult(requestResult.GetRawText());
            return JsonConvert.DeserializeObject<RpcResponseMessage>(result);
        }


        public async UniTask<string> Sign(string message, MetamaskSignature sign)
        {
            var wallet = MetaMaskUnity.Instance.Wallet;
            var account = wallet.SelectedAddress;
            int val = ++id;

            var transactionParams = new MetaMaskTransaction() { From = account, Data = message };
            var request = new MetaMaskEthereumRequest
            {
                Method = Enum.GetName(typeof(MetamaskSignature), sign),
                Parameters = new MetaMaskTransaction[] { transactionParams }
            };
            RpcResponseMessage response = await RequestCallAsync(val, request);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }

        protected void HandleRpcError(RpcResponseMessage response, string reqMsg)
        {
            if (response.HasError)
                throw new RpcResponseException(new RpcError(response.Error.Code, response.Error.Message + ": " + reqMsg,
                    response.Error.Data));
        }

    }
}
