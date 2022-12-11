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

#if UNITY_WEBGL
public class Web3GL
{
    [DllImport("__Internal")]
    private static extern void Connect();

    [DllImport("__Internal")]
    private static extern void CallContract(string id, string parametersJson);

    [DllImport("__Internal")]
    private static extern string GetResult(string id);


    public static U Call<T, U>(T _function, string _address) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
    {
        var test = _function.CreateCallInput(_address);
        var callValue = JsonConvert.SerializeObject(test);
        string id = Guid.NewGuid().ToString();
        CallContract(id, callValue);
        string result;
        do
        {
            Console.WriteLine("get result");
            Thread.Sleep(1000);
            new WaitForSeconds(1f);
            result = GetResult(id);
        } while (string.IsNullOrWhiteSpace(result));
        Console.WriteLine("result " + result);
        var decode = new FunctionCallDecoder().DecodeFunctionOutput<U>(result);
        return decode;
    }

    public static async Task ConnectAccount()
    {
        Connect();
    }

}
#endif