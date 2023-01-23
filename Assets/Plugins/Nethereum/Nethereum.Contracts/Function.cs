using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts
{
    public class Function : FunctionBase
    {
        public Function(Contract contract, FunctionBuilder functionBuilder) : base(contract, functionBuilder)
        {
        }

        protected FunctionBuilder FunctionBuilder => (FunctionBuilder) FunctionBuilderBase;

        public CallInput CreateCallInput(params object[] functionInput)
        {
            return FunctionBuilder.CreateCallInput(functionInput);
        }

        public CallInput CreateCallInput(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return FunctionBuilder.CreateCallInput(from, gas, value, functionInput);
        }

        public string GetData(params object[] functionInput)
        {
            return FunctionBuilder.GetData(functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, gas, value, functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, gas, gasPrice, value, functionInput);
        }

        public TransactionInput CreateTransactionInput(TransactionInput input, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(input, functionInput);
        }

        public TransactionInput CreateTransactionInput(HexBigInteger type, string from, HexBigInteger gas,
            HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(type, from, gas, value, maxFeePerGas, maxPriorityFeePerGas,
                functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas,
            HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, gas, value, maxFeePerGas, maxPriorityFeePerGas,
                functionInput);
        }

#if !DOTNET35

        public UniTask<List<ParameterOutput>> CallDecodingToDefaultAsync(params object[] functionInput)
        {
            return base.CallDecodingToDefaultAsync(CreateCallInput(functionInput));
        }

        public UniTask<List<ParameterOutput>> CallDecodingToDefaultAsync(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return base.CallDecodingToDefaultAsync(CreateCallInput(from, gas, value, functionInput));
        }

        public UniTask<List<ParameterOutput>> CallDecodingToDefaultAsync(string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block, params object[] functionInput)
        {
            return base.CallDecodingToDefaultAsync(CreateCallInput(from, gas, value, functionInput), block);
        }

        public UniTask<List<ParameterOutput>> CallDecodingToDefaultAsync(BlockParameter block, params object[] functionInput)
        {
            return base.CallDecodingToDefaultAsync(CreateCallInput(functionInput), block);
        }

        public UniTask<TReturn> CallAsync<TReturn>(params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput));
        }

        public UniTask<TReturn> CallAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(from, gas, value, functionInput));
        }

        public UniTask<TReturn> CallAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block, params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(from, gas, value, functionInput), block);
        }

        public UniTask<TReturn> CallAsync<TReturn>(BlockParameter block, params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput), block);
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(params object[] functionInput)
            where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput));
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(from, gas, value, functionInput));
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block, params object[] functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(from, gas, value, functionInput), block);
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(
            BlockParameter blockParameter, params object[] functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput), blockParameter);
        }

        public UniTask<HexBigInteger> EstimateGasAsync(params object[] functionInput)
        {
            return EstimateGasFromEncAsync(CreateCallInput(functionInput));
        }

        public UniTask<HexBigInteger> EstimateGasAsync(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return EstimateGasFromEncAsync(CreateCallInput(from, gas, value, functionInput));
        }

        public UniTask<string> SendTransactionAsync(string from, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, functionInput));
        }

        public UniTask<string> SendTransactionAsync(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, gas, value, functionInput));
        }

        public UniTask<string> SendTransactionAsync(HexBigInteger type, string from, HexBigInteger gas,
            HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(type, from, gas, value, maxFeePerGas, maxPriorityFeePerGas,
                functionInput));
        }

        public UniTask<string> SendTransactionAsync(string from, HexBigInteger gas,
            HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, gas, value, maxFeePerGas, maxPriorityFeePerGas,
                functionInput));
        }


        public UniTask<string> SendTransactionAsync(string from, HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, gas, gasPrice, value, functionInput));
        }

        public UniTask<string> SendTransactionAsync(TransactionInput input, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(input, functionInput));
        }


        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from,
            CancellationToken receiptRequestCancellationToken, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, functionInput),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger value, CancellationToken receiptRequestCancellationToken,
            params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, gas, value, functionInput),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value, CancellationToken receiptRequestCancellationToken,
            params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(
                CreateTransactionInput(from, gas, gasPrice, value, functionInput), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TransactionInput input,
            CancellationToken receiptRequestCancellationToken, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(input, functionInput),
                receiptRequestCancellationToken);
        }


        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from,
           CancellationTokenSource receiptRequestCancellationToken = null, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, functionInput),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, gas, value, functionInput),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(
                CreateTransactionInput(from, gas, gasPrice, value, functionInput), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TransactionInput input,
            CancellationTokenSource receiptRequestCancellationToken = null, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(input, functionInput),
                receiptRequestCancellationToken);
        }


        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(HexBigInteger type, string from, HexBigInteger gas,
            HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(type, from, gas, value, maxFeePerGas, maxPriorityFeePerGas,
                functionInput));
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger value, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, gas, value, maxFeePerGas, maxPriorityFeePerGas,
                functionInput));
        }
#endif
    }
}