using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Unity.Contracts;
using Nethereum.Unity.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Web3Unity
{
    public static class TransactionUnityRequest
    {
        public static string Call<T, U>(this IUnityRpcRequestClientFactory rpcUnityRequest, T function, string address, string account) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
        {
            var queryRequest = new QueryUnityRequest<T, U>(rpcUnityRequest, account);
            var call = queryRequest.Query(function, address);
            U result = default(U);
            do
            {
                result = call.Current as U;
            } while (call.MoveNext());

            return result;
        }

        public static string SendTransaction<T>(this IContractTransactionUnityRequest transactionUnityRequest, T function, string address) where T : FunctionMessage, new()
        {
            var call = transactionUnityRequest.SignAndSendTransaction(function, address);
            var result = string.Empty;
            do
            {
                result = call.Current.ToString();
            } while (call.MoveNext());

            return result;
        }

        public static TransactionReceipt SendWaitForReceipt<T>(this IUnityRpcRequestClientFactory rpcUnityRequest, IContractTransactionUnityRequest transactionUnityRequest, T function, string address) where T : FunctionMessage, new()
        {
            var transactionHash = SendTransaction(transactionUnityRequest, function, address);
            var transactionReceiptPolling = new TransactionReceiptPollingRequest(rpcUnityRequest);
            //checking every 2 seconds for the receipt
            var call = transactionReceiptPolling.PollForReceipt(transactionHash, 2);
            TransactionReceipt result = null;
            do
            {
                result = call.Current as TransactionReceipt;
            } while (call.MoveNext());
            return result;
        }
    }
}
