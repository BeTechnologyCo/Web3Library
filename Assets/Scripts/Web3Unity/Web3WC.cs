using Cysharp.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Network;
using WalletConnectSharp.NEthereum;

namespace Web3Unity
{
    /// <summary>
    /// Wallet connect provider
    /// </summary>
    public class Web3WC
    {
        private static int id = 0;

        public string Uri { get; private set; }

        public WalletConnect Client { get; private set; }

        public Web3 Web3Client { get; private set; }

        public event EventHandler<string> Connected;

        private static Dictionary<int, UniTaskCompletionSource<string>> utcs = new Dictionary<int, UniTaskCompletionSource<string>>();
        private static UniTaskCompletionSource<string> utcsConnected;


        public event EventHandler<string> UriGenerated;

        public static void RequestCallResult(int key, string val)
        {
            if (utcs.ContainsKey(key))
            {
                utcs[key].TrySetResult(val);
                Debug.Log($"Key found Web3GL {key}");
            }
            else
            {
                Debug.LogWarning($"Key not found Web3GL {key}");
            }
        }


        public static async UniTask<RpcResponseMessage> RequestCallAsync(int val, RpcRequestMessage request, bool sign = false)
        {
            utcs[val] = new UniTaskCompletionSource<string>();
            var datas = request.RawParameters as object[];
            if (datas?.Length > 0)
            {
                if (!sign)
                {
                    var callParam = datas[0] as TransactionInput;
                    // Application.OpenURL($"{server}?id={request.Id}&method={request.Method}&data={callParam.Data}&deepLink={deepLink}&to={callParam.To}&value={callParam.Value}");
                }
                else
                {
                    var callParam = datas[0].ToString();
                    // Application.OpenURL($"{server}?id={request.Id}&method={request.Method}&data={callParam}&deepLink={deepLink}");
                }
            }

            //while (utcs[val].Task.Status == UniTaskStatus.Pending)
            //{
            //    await Task.Delay(100);
            //}

            string result = await utcs[val].Task;
            return JsonConvert.DeserializeObject<RpcResponseMessage>(result);
        }

        public ClientMeta Metadata { get; private set; }
        public Web3WC(string rpcUrl, string name, string description, string icon, string url)
        {
            Metadata = new ClientMeta()
            {
                Description = description,
                Icons = new[] { icon },
                Name = name,
                URL = url
            };

        }

        private void Client_OnSessionConnect(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {

            Debug.Log($"session connect");
        }

        private void Client_OnSend(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {
            Debug.Log($"Client_OnSend " + e.Accounts?.Length);
        }

        private void Client_OnTransportConnect(object sender, WalletConnectSharp.Core.WalletConnectProtocol e)
        {
            Debug.Log($"Transport");
        }

        private void Client_OnSessionCreated(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {
            Debug.Log($"session");
        }

        public async Task Connect(string rpcUrl)
        {
            Client = new WalletConnect(clientMeta: Metadata);
            //var nethereum = new Web3(walletConnect.CreateProvider(new Uri("https//rpc.testnet.fantom.network/")));
            Client.OnSessionCreated += Client_OnSessionCreated;
            Client.OnTransportConnect += Client_OnTransportConnect;
            Client.OnSend += Client_OnSend;
            Client.OnSessionConnect += Client_OnSessionConnect;
            Uri = Client.URI;

            if (UriGenerated != null)
            {
                UriGenerated(this, Uri);
            }
            Debug.Log("connect");
            await Client.Connect();
            //await Client.Connect();
            Debug.Log($"Address: {Client.Accounts[0]}");
            Debug.Log($"Chain ID: {Client.ChainId}");

            Web3Client = Client.BuildWeb3(new Uri(rpcUrl)).AsWalletAccount(true);
            if (Connected != null)
            {
                Connected(this, Client.Accounts[0]);
            }
        }

        public static async Task<string> Send<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var transactioninput = _function.CreateTransactionInput(_address);
            var parameters = new object[1] { transactioninput };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_sendTransaction", parameters);
            RpcResponseMessage response = await RequestCallAsync(val, rpcRequest);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }

        public static async Task<TransactionReceipt> SendAndWaitForReceipt<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var getReceipt = await Send(_function, _address);
            var parameters = new object[1] { getReceipt };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_getTransactionReceipt", parameters);
            RpcResponseMessage response = await RequestCallAsync(val, rpcRequest);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            TransactionReceipt transaction = response.GetResult<TransactionReceipt>();
            Console.WriteLine("result " + transaction.TransactionHash);
            return transaction;
        }

        public static async Task<HexBigInteger> EstimateGas<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var transactioninput = _function.CreateTransactionInput(_address);
            var parameters = new object[1] { transactioninput };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_estimateGas", parameters);
            RpcResponseMessage response = await RequestCallAsync(val, rpcRequest);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<HexBigInteger>());
            return response.GetResult<HexBigInteger>();
        }

        public static async Task<string> SignFunction<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var transactioninput = _function.CreateTransactionInput(_address);
            var parameters = new object[1] { transactioninput };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_sign", parameters);
            RpcResponseMessage response = await RequestCallAsync(val, rpcRequest);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }

        public static async Task<string> Sign(string message, MetamaskSignature metamaskSign)
        {
            var parameters = new object[1] { message };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, Enum.GetName(typeof(MetamaskSignature), metamaskSign), parameters);
            RpcResponseMessage response = await RequestCallAsync(val, rpcRequest, true);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }

    }

}