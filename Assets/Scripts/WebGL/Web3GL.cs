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

#if UNITY_WEBGL
public class Web3GL
{
    [DllImport("__Internal")]
    private static extern void Connect();

    [DllImport("__Internal")]
    private static extern void CallContract(int index, string parametersJson);

    [DllImport("__Internal")]
    private static extern void SendContract(int index, string parametersJson);

    [DllImport("__Internal")]
    private static extern string GetResult(int index);

    private static int id = 0;

    public event EventHandler<string> AccountConnected;
    public Web3GL()
    {
    }

    public async Task<U> Call<T, U>(T _function, string _address) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
    {
        var test = _function.CreateCallInput(_address);
        var callValue = JsonConvert.SerializeObject(test);
        int val = ++id;
        CallContract(val, callValue);
        string result;
        do
        {
            Console.WriteLine("get result");
            await new WaitForSeconds(1f);
            result = GetResult(val);
        } while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        var decode = new FunctionCallDecoder().DecodeFunctionOutput<U>(result);
        return decode;
    }

    public async Task<string> Send<T>(T _function, string _address) where T : FunctionMessage, new()
    {
        var test = _function.CreateTransactionInput(_address);
        var callValue = JsonConvert.SerializeObject(test);
        int val = ++id;
        CallContract(val, callValue);
        string result;
        do
        {
            Console.WriteLine("get result");
            await new WaitForSeconds(1f);
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
        CallContract(val, callValue);
        string result;
        do
        {
            Console.WriteLine("get result");
            await new WaitForSeconds(1f);
            result = GetResult(val);
        } while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        return result;
    }

    public static async Task<string> ConnectAccount()
    {
        string result;
        Connect();
        do
        {
            Console.WriteLine("get result");
            await new WaitForSeconds(1f);
            result = GetResult(-1);
        } while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        return result;
    }

}
#endif