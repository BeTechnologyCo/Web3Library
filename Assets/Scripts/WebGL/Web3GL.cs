using AOT;
using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;


public class Web3GL
{
    [DllImport("__Internal")]
    private static extern void Connect(Action<int, string> callback);

    [DllImport("__Internal")]
    private static extern void Request(string jsonCall, Action<int, string> callback);

    [DllImport("__Internal")]
    private static extern bool IsMetamaskAvailable();

    [DllImport("__Internal")]
    private static extern string GetSelectedAddress();

    [DllImport("__Internal")]
    private static extern bool IsConnected();

    private static int id = 0;

    public static event EventHandler<string> OnAccountConnected;
    public static event EventHandler<string> OnAccountChanged;
    public static event EventHandler<BigInteger> OnChainChanged;
    public static event EventHandler OnAccountDisconnected;

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

    [MonoPInvokeCallback(typeof(Action<int, string>))]
    private static void Connected(int changeType, string result)
    {
        switch (changeType)
        {
            case 1:
                utcsConnected?.TrySetResult(result);
                if (OnAccountConnected != null)
                {
                    OnAccountConnected(new Web3GL(), result);
                }

                break;
            case 2:
                if (OnChainChanged != null)
                {
                    OnChainChanged(new Web3GL(), BigInteger.Parse(result));
                }
                break;
            case 3:
                if (OnAccountChanged != null)
                {
                    OnAccountChanged(new Web3GL(), result);
                }
                break;
            case 4:
                if (OnAccountDisconnected != null)
                {
                    OnAccountDisconnected(new Web3GL(), new EventArgs());
                }
                break;
        }

    }

    public static async UniTask<RpcResponseMessage> RequestCallAsync(int val, string jsonCall)
    {
        utcs[val] = new UniTaskCompletionSource<string>();
        Request(jsonCall, RequestCallResult);
        string result = await utcs[val].Task;
        return JsonConvert.DeserializeObject<RpcResponseMessage>(result);
    }


    public Web3GL()
    {
    }

    public static async Task<U> Call<T, U>(T _function, string _address) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
    {

        var callInput = _function.CreateCallInput(_address);
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


    public static async Task<string> Send<T>(T _function, string _address) where T : FunctionMessage, new()
    {
        var transactioninput = _function.CreateTransactionInput(_address);
        var parameters = new object[1] { transactioninput };
        int val = ++id;
        MetamaskRequest rpcRequest = new MetamaskRequest(val, "eth_sendTransaction", GetSelectedAddress(), parameters);
        var jsonCall = JsonConvert.SerializeObject(rpcRequest);
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

    public static async Task<HexBigInteger> EstimateGas<T>(T _function, string _address) where T : FunctionMessage, new()
    {
        var transactioninput = _function.CreateTransactionInput(_address);
        var parameters = new object[1] { transactioninput };
        int val = ++id;
        MetamaskRequest rpcRequest = new MetamaskRequest(val, "eth_estimateGas", GetSelectedAddress(), parameters);
        var jsonCall = JsonConvert.SerializeObject(rpcRequest);
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
        string address = GetSelectedAddress();
        var transactioninput = _function.CreateTransactionInput(_address);
        var parameters = new object[2] { address, transactioninput.Value };
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

    public static async Task<string> Sign(string message, MetamaskSignature sign)
    {
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


    public static async Task<string> ConnectAccount()
    {
        utcsConnected = new UniTaskCompletionSource<string>();
        Connect(Connected);
        string result = await utcsConnected.Task;
        return result;
    }

}