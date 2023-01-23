using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts
{
    public interface IDeployContract
    {
        ITransactionManager TransactionManager { get; set; }
        string GetData(string contractByteCode, string abi, params object[] values);
        string GetData<TConstructorParams>(string contractByteCode, TConstructorParams inputParams);

#if !DOTNET35
        UniTask<HexBigInteger> EstimateGasAsync(string abi, string contractByteCode, string from, params object[] values);
        UniTask<HexBigInteger> EstimateGasAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger value, TConstructorParams inputParams);
        UniTask<HexBigInteger> EstimateGasAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, TConstructorParams inputParams);
        UniTask<HexBigInteger> EstimateGasAsync<TConstructorParams>(string contractByteCode, string from, TConstructorParams inputParams);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, HexBigInteger gas,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, HexBigInteger gas, HexBigInteger value,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from,CancellationToken receiptRequestCancellationToken, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, HexBigInteger gas,CancellationToken receiptRequestCancellationToken, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger value,CancellationToken receiptRequestCancellationToken, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value,CancellationToken receiptRequestCancellationToken, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken);

        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, HexBigInteger gas, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, HexBigInteger gas, HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, CancellationTokenSource receiptRequestCancellationToken = null, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, HexBigInteger gas, CancellationTokenSource receiptRequestCancellationToken = null, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null, params object[] values);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from, TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null);


        UniTask<string> SendRequestAsync(string contractByteCode, string from);
        UniTask<string> SendRequestAsync(string contractByteCode, string from, HexBigInteger gas);
        UniTask<string> SendRequestAsync(string contractByteCode, string from, HexBigInteger gas, HexBigInteger value);
        UniTask<string> SendRequestAsync(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value);
        UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, params object[] values);
        UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, params object[] values);
        UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas, HexBigInteger value, params object[] values);
        UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas, params object[] values);
        UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, params object[] values);
        UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams);
        UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, TConstructorParams inputParams);
        UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from, HexBigInteger gas, TConstructorParams inputParams);
        UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from, TConstructorParams inputParams);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken = default);

        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken = default);

        UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams);

        UniTask<string> SendRequestAsync<TConstructorParams>(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams);

        UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, params object[] values);

        UniTask<string> SendRequestAsync(HexBigInteger type, string abi, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, params object[] values);

        UniTask<string> SendRequestAsync(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce);

        UniTask<string> SendRequestAsync(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce);
#endif


    }
}