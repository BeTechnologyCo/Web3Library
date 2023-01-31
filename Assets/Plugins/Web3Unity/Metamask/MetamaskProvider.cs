using AOT;
using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;
using RpcError = Nethereum.JsonRpc.Client.RpcError;

namespace Web3Unity
{
    public class MetamaskProvider : IClient
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern void Connect(Action<int, string> callback);

        [DllImport("__Internal")]
        public static extern void Request(string jsonCall, Action<int, string> callback);

        [DllImport("__Internal")]
        public static extern bool IsMetamaskAvailable();

        [DllImport("__Internal")]
        public static extern string GetSelectedAddress();

        [DllImport("__Internal")]
        public static extern bool IsConnected();

        [DllImport("__Internal")]
        public static extern string RequestRpcClientCallback(Action<string> rpcResponse, string rpcRequest);
#else
        // handle special platform like ios who throw an error on DllImport
        public static void Connect(Action<int, string> callback)
        {
        }
        public static void Request(string jsonCall, Action<int, string> callback)
        {
        }
        public static bool IsMetamaskAvailable()
        {
            return false;
        }
        public static string GetSelectedAddress()
        {
            return string.Empty;
        }

        public static bool IsConnected()
        {
            return false;
        }
        public static string RequestRpcClientCallback(Action<string> rpcResponse, string rpcRequest)
        {
            return string.Empty;
        }
#endif

        private static int id = 0;

        public static event EventHandler<string> OnAccountConnected;
        public static event EventHandler<string> OnAccountChanged;
        public static event EventHandler<BigInteger> OnChainChanged;
        public static event EventHandler OnAccountDisconnected;

        private static Dictionary<int, UniTaskCompletionSource<string>> utcs = new Dictionary<int, UniTaskCompletionSource<string>>();
        private static UniTaskCompletionSource<string> utcsConnected;

        public RequestInterceptor OverridingRequestInterceptor { get; set; }

        [MonoPInvokeCallback(typeof(Action<int, string>))]
        private static void RequestCallResult(int key, string val)
        {
            if (utcs.ContainsKey(key))
            {
                utcs[key].TrySetResult(val);
                Debug.Log($"Key found Web3GL {key}");
                Debug.Log($"val Web3GL {val}");
            }
            else
            {
                Debug.LogWarning($"Key not found Web3GL {key}");
            }
        }

        [MonoPInvokeCallback(typeof(Action<int, string>))]
        private static void Connected(int changeType, string result)
        {
            switch (changeType)
            {
                case 1:
                    utcsConnected?.TrySetResult(result);
                    if (OnAccountConnected != null)
                    {
                        OnAccountConnected(Web3Connect.Instance.MetamaskInstance, result);
                    }

                    break;
                case 2:
                    if (OnChainChanged != null)
                    {
                        OnChainChanged(Web3Connect.Instance.MetamaskInstance, BigInteger.Parse(result));
                    }
                    break;
                case 3:
                    if (OnAccountChanged != null)
                    {
                        OnAccountChanged(Web3Connect.Instance.MetamaskInstance, result);
                    }
                    break;
                case 4:
                    if (OnAccountDisconnected != null)
                    {
                        OnAccountDisconnected(Web3Connect.Instance.MetamaskInstance, new EventArgs());
                    }
                    break;
            }

        }

        public async UniTask<RpcResponseMessage> RequestCallAsync(int val, string jsonCall)
        {
            utcs[val] = new UniTaskCompletionSource<string>();
            Request(jsonCall, RequestCallResult);
            string result = await utcs[val].Task;
            return JsonConvert.DeserializeObject<RpcResponseMessage>(result);
        }


        public MetamaskProvider(bool autoConnect)
        {
            if (autoConnect)
            {
                ConnectAccount();
            }
        }


        public async UniTask<string> ConnectAccount()
        {
            utcsConnected = new UniTaskCompletionSource<string>();
            Connect(Connected);
            string result = await utcsConnected.Task;
            return result;
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
            }
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
            var account = GetSelectedAddress();
            if (paramList != null && paramList.Length > 0)
            {
                var callInput = paramList[0] as CallInput;
                if (callInput != null)
                {
                    callInput.From = account;
                }
                else
                {
                    var transactionInput = paramList[0] as TransactionInput;
                    if (transactionInput != null)
                    {
                        transactionInput.From = account;
                    }
                }
            }
            MetamaskRequest rpcRequest = new MetamaskRequest(val, method, account, paramList);

            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            HandleRpcError(response, method);
            return response;
        }

        protected void HandleRpcError(RpcResponseMessage response, string reqMsg)
        {
            if (response.HasError)
                throw new RpcResponseException(new RpcError(response.Error.Code, response.Error.Message + ": " + reqMsg,
                    response.Error.Data));
        }

        public async UniTask<U> Call<T, U>(T _function, string _address) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
        {

            var callInput = _function.CreateCallInput(_address);
            var account = GetSelectedAddress();
            callInput.From = account;
            var parameters = new object[1] { callInput };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_call", parameters);
            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            Console.WriteLine("jsoncall " + jsonCall);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            var decode = new FunctionCallDecoder().DecodeFunctionOutput<U>(response.GetResult<string>());
            return decode;
        }


        public async UniTask<string> Send<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var transactioninput = _function.CreateTransactionInput(_address);
            var account = GetSelectedAddress();
            transactioninput.From = account;
            var parameters = new object[1] { transactioninput };
            int val = ++id;
            MetamaskRequest rpcRequest = new MetamaskRequest(val, "eth_sendTransaction", account, parameters);
            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }

        public async UniTask<TransactionReceipt> SendAndWaitForReceipt<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var getReceipt = await Send(_function, _address);
            var parameters = new object[1] { getReceipt };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_getTransactionReceipt", parameters);
            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            TransactionReceipt transaction = response.GetResult<TransactionReceipt>();
            Console.WriteLine("result " + transaction.TransactionHash);
            return transaction;
        }

        public async UniTask<HexBigInteger> EstimateGas<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            var transactioninput = _function.CreateTransactionInput(_address);
            var account = GetSelectedAddress();
            transactioninput.From = account;
            var parameters = new object[1] { transactioninput };
            int val = ++id;
            MetamaskRequest rpcRequest = new MetamaskRequest(val, "eth_estimateGas", account, parameters);
            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<HexBigInteger>());
            return response.GetResult<HexBigInteger>();
        }

        public async UniTask<string> SignFunction<T>(T _function, string _address) where T : FunctionMessage, new()
        {
            string account = GetSelectedAddress();
            var transactioninput = _function.CreateTransactionInput(_address);
            transactioninput.From = account;
            var parameters = new object[2] { account, transactioninput.Value };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_sign", parameters);
            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            Console.WriteLine("jsoncall " + jsonCall);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }

        public async UniTask<string> Sign(string message, MetamaskSignature sign)
        {
            var account = GetSelectedAddress();
            var parameters = new object[2] { GetSelectedAddress(), message };
            int val = ++id;
            RpcRequestMessage rpcRequest = new RpcRequestMessage(val, Enum.GetName(typeof(MetamaskSignature), sign), parameters);
            var jsonCall = JsonConvert.SerializeObject(rpcRequest);
            RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
            if (!string.IsNullOrEmpty(response.Error?.Message))
            {
                throw new Exception(response.Error?.Message);
            }
            Console.WriteLine("result " + response.GetResult<string>());
            return response.GetResult<string>();
        }
    }
}