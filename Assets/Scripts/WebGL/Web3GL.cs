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

public class Web3GL
{
    [DllImport("__Internal")]
    private static extern void Connect();

    [DllImport("__Internal")]
    private static extern string CallContract(string parametersJson);


    public static U Call<T, U>(T _function, string _address) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
    {
        var test = _function.CreateCallInput(_address);
        //var transaction = _function.CreateTransactionInput(_address);
        //var data = _function.GetCallData();
        //var value = _function.GetHexValue();
        //TransactionInput transaction = new TransactionInput();
        //transaction.From = _function.FromAddress;
        //transaction.Data = data.ToHex();
        //transaction.Value = value;
        //transaction.To = _address;
        var callValue = JsonConvert.SerializeObject(test);
        var result =  CallContract(callValue);
        Console.WriteLine("result " + result);
        var decode = new FunctionCallDecoder().DecodeFunctionOutput<U>(result);
        return decode;
    }

    public static async Task ConnectAccount()
    {
        Connect();
    }

}