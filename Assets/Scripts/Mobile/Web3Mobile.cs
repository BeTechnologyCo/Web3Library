using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Util;
using System.Collections.Generic;
using System.Text;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.Encoders;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using UnityEngine;
using System;
using System.Threading;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using AOT;
using System.Numerics;
using Nethereum.JsonRpc.Client.RpcMessages;


public class Web3Mobile
{
    private static int id = 0;
    private static string server = "https://192.168.1.154:44451";
    public static string deepLink = "unitydl://mylink";
    private static Dictionary<int, UniTaskCompletionSource<string>> utcs = new Dictionary<int, UniTaskCompletionSource<string>>();
    private static UniTaskCompletionSource<string> utcsConnected;

    [MonoPInvokeCallback(typeof(Action<int, string>))]
    private static void RequestCallResult(int key, string val)
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


    public static async UniTask<RpcResponseMessage> RequestCallAsync(int val, string jsonCall)
    {
        utcs[val] = new UniTaskCompletionSource<string>();
        Application.OpenURL($"{server}?sign={jsonCall}");
        string result = await utcs[val].Task;
        return JsonConvert.DeserializeObject<RpcResponseMessage>(result);
    }


    public Web3Mobile()
    {
    }

    public static async Task<string> Send<T>(T _function, string _address) where T : FunctionMessage, new()
    {
        var transactioninput = _function.CreateTransactionInput(_address);
        var parameters = new object[1] { transactioninput };
        int val = ++id;
        RpcRequestMessage rpcRequest = new RpcRequestMessage(val, "eth_sendTransaction", parameters);
        SignRequest sign = new SignRequest()
        {
            Id = val.ToString(),
            DeepLink = deepLink,
            Message = rpcRequest,
            Result = ""
        };
        var jsonCall = JsonConvert.SerializeObject(sign);
        RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
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
        SignRequest sign = new SignRequest()
        {
            Id = val.ToString(),
            DeepLink = deepLink,
            Message = rpcRequest,
            Result = ""
        };
        var jsonCall = JsonConvert.SerializeObject(sign);
        RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
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
        SignRequest sign = new SignRequest()
        {
            Id = val.ToString(),
            DeepLink = deepLink,
            Message = rpcRequest,
            Result = ""
        };
        var jsonCall = JsonConvert.SerializeObject(sign);
        RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
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
        SignRequest sign = new SignRequest()
        {
            Id = val.ToString(),
            DeepLink = deepLink,
            Message = rpcRequest,
            Result = ""
        };
        var jsonCall = JsonConvert.SerializeObject(sign);
        RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
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
        SignRequest sign = new SignRequest()
        {
            Id = val.ToString(),
            DeepLink = deepLink,
            Message = rpcRequest,
            Result = ""
        };
        var jsonCall = JsonConvert.SerializeObject(sign);
        RpcResponseMessage response = await RequestCallAsync(val, jsonCall);
        if (!string.IsNullOrEmpty(response.Error?.Message))
        {
            throw new Exception(response.Error?.Message);
        }
        Console.WriteLine("result " + response.GetResult<string>());
        return response.GetResult<string>();
    }

}