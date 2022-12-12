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

#if UNITY_WEBGL
public class Web3GL
{
    [DllImport("__Internal")]
    private static extern void Connect(Action<string> callback);

    [DllImport("__Internal")]
    private static extern void CallContract(int index, string parametersJson, Action<int, string> callback);

    [DllImport("__Internal")]
    private static extern void SendContract(int index, string parametersJson);

    [DllImport("__Internal")]
    private static extern string GetResult(int index);

    private static int id = 0;

    public static event EventHandler<string> OnAccountConnected;

    private static Dictionary<int, UniTaskCompletionSource<string>> utcs = new Dictionary<int, UniTaskCompletionSource<string>>();

    [MonoPInvokeCallback(typeof(Action<int, string>))]
    private static void testFuncCB(int key, string val)
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

    [MonoPInvokeCallback(typeof(Action<string>))]
    private static void Connected(string account)
    {
        if (OnAccountConnected != null)
        {
            OnAccountConnected(new Web3GL(), account);
        }
    }

    public static UniTask<string> TestFuncCallAsync(int val, string parametersJson)
    {
        utcs[val] = new UniTaskCompletionSource<string>();
        CallContract(val, parametersJson, testFuncCB);
        return utcs[val].Task;
    }


    public event EventHandler<string> AccountConnected;
    public Web3GL()
    {
    }

    public static async Task<U> Call<T, U>(T _function, string _address) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
    {
        var test = _function.CreateCallInput(_address);
        var callValue = JsonConvert.SerializeObject(test);
        int val = ++id;
        string result = await TestFuncCallAsync(val, callValue);

        //do
        //{
        //    Console.WriteLine("get result");
        //    await UniTask.Delay(1000);
        //    result = GetResult(val);
        //} while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        var decode = new FunctionCallDecoder().DecodeFunctionOutput<U>(result);
        return decode;
    }

    public async Task<string> Send<T>(T _function, string _address) where T : FunctionMessage, new()
    {
        var test = _function.CreateTransactionInput(_address);
        var callValue = JsonConvert.SerializeObject(test);
        int val = ++id;
        SendContract(val, callValue);
        string result;
        do
        {
            Console.WriteLine("get result");
            //   await new WaitForSeconds(1f);
            result = GetResult(val);
        } while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        return result;
    }

    public async Task<string> SendAndGetResult<T>(T _function, string _address) where T : FunctionMessage, new()
    {
        var test = _function.CreateTransactionInput(_address);
        var callValue = JsonConvert.SerializeObject(test);
        int val = ++id;
        SendContract(val, callValue);
        string result;
        do
        {
            Console.WriteLine("get result");
            //  await new WaitForSeconds(1f);
            result = GetResult(val);
        } while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        return result;
    }

    public static async Task<string> ConnectAccount()
    {
        string result;
        Connect(Connected);
        //do
        //{
        //    Console.WriteLine("get result");
        //    // await new WaitForSeconds(1f);
        //    result = GetResult(-1);
        //} while (string.IsNullOrWhiteSpace(result));
        //Console.WriteLine("result " + result);
        return string.Empty;
    }

}
#endif