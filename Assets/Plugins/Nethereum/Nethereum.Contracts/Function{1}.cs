using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts
{
    public class Function<TFunctionInput> : FunctionBase
    {
        public Function(Contract contract, FunctionBuilder<TFunctionInput> functionBuilder)
            : base(contract, functionBuilder)
        {
        }

        protected FunctionBuilder<TFunctionInput> FunctionBuilder =>
            (FunctionBuilder<TFunctionInput>)FunctionBuilderBase;


        public CallInput CreateCallInputParameterless()
        {
            return FunctionBuilder.CreateCallInputParameterless();
        }

        public CallInput CreateCallInput(TFunctionInput functionInput)
        {
            return FunctionBuilder.CreateCallInput(functionInput);
        }

        public CallInput CreateCallInput(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return FunctionBuilder.CreateCallInput(functionInput, from, gas, value);
        }

        public string GetData(TFunctionInput functionInput)
        {
            return FunctionBuilder.GetData(functionInput);
        }

        public TFunctionInput DecodeFunctionInput(TFunctionInput functionInput, TransactionInput transactionInput)
        {
            return FunctionBuilder.DecodeFunctionInput(functionInput, transactionInput);
        }

        public TFunctionInput DecodeFunctionInput(TFunctionInput functionInput, string data)
        {
            return FunctionBuilder.DecodeFunctionInput(functionInput, data);
        }

        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from)
        {
            return FunctionBuilder.CreateTransactionInput(functionInput, from);
        }

        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return FunctionBuilder.CreateTransactionInput(functionInput, from, gas, value);
        }

        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger gasPrice, HexBigInteger value)
        {
            return FunctionBuilder.CreateTransactionInput(functionInput, from, gas, gasPrice, value);
        }

#if !DOTNET35
        public UniTask<TReturn> CallAsync<TReturn>()
        {
            return base.CallAsync<TReturn>(CreateCallInputParameterless());
        }

        public UniTask<TReturn> CallAsync<TReturn>(TFunctionInput functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput));
        }


        public UniTask<TReturn> CallAsync<TReturn>(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput, from, gas, value));
        }

        public UniTask<TReturn> CallAsync<TReturn>(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput, from, gas, value), block);
        }

        public UniTask<TReturn> CallAsync<TReturn>(TFunctionInput functionInput,
            BlockParameter blockParameter)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput), blockParameter);
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>() where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInputParameterless());
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(BlockParameter block) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInputParameterless(), block);
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput));
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, BlockParameter block)
            where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput), block);
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput, from, gas, value));
        }

        public UniTask<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value, BlockParameter block) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput, from, gas, value), block);
        }

        public UniTask<HexBigInteger> EstimateGasAsync()
        {
            return EstimateGasFromEncAsync(CreateCallInputParameterless());
        }

        public UniTask<HexBigInteger> EstimateGasAsync(TFunctionInput functionInput)
        {
            return EstimateGasFromEncAsync(CreateCallInput(functionInput));
        }

        public UniTask<HexBigInteger> EstimateGasAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return EstimateGasFromEncAsync(CreateCallInput(functionInput, from, gas, value));
        }

        public UniTask<HexBigInteger> EstimateGasAsync(TFunctionInput functionInput,
            CallInput callInput)
        {
            var encodedInput = GetData(functionInput);
            callInput.Data = encodedInput;
            return EstimateGasFromEncAsync(callInput);
        }


        public UniTask<string> SendTransactionAsync(TFunctionInput functionInput, string from)
        {
            return base.SendTransactionAsync(CreateTransactionInput(functionInput, from));
        }

        public UniTask<string> SendTransactionAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return base.SendTransactionAsync(CreateTransactionInput(functionInput, from, gas, value));
        }

        public UniTask<string> SendTransactionAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value)
        {
            return base.SendTransactionAsync(CreateTransactionInput(functionInput, from, gas, gasPrice, value));
        }

        public UniTask<string> SendTransactionAsync(TFunctionInput functionInput,
            TransactionInput input)
        {
            var encodedInput = GetData(functionInput);
            input.Data = encodedInput;
            return base.SendTransactionAsync(input);
        }


        public UniTask<byte[]> CallRawAsync<TReturn>()
        {
            return base.CallRawAsync(CreateCallInputParameterless());
        }

        public UniTask<byte[]> CallRawAsync<TReturn>(TFunctionInput functionInput)
        {
            return base.CallRawAsync(CreateCallInput(functionInput));
        }

        public UniTask<byte[]> CallRawAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return base.CallRawAsync(CreateCallInput(functionInput, from, gas, value));
        }

        public UniTask<byte[]> CallRawAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block)
        {
            return base.CallRawAsync(CreateCallInput(functionInput, from, gas, value), block);
        }

        public UniTask<byte[]> CallRawAsync(TFunctionInput functionInput,
            BlockParameter blockParameter)
        {
            return base.CallRawAsync(CreateCallInput(functionInput), blockParameter);
        }


        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            CancellationToken receiptRequestCancellationToken)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(functionInput, from),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value, CancellationToken receiptRequestCancellationToken)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(functionInput, from, gas, value),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, CancellationToken receiptRequestCancellationToken)
        {
            return base.SendTransactionAndWaitForReceiptAsync(
                CreateTransactionInput(functionInput, from, gas, gasPrice, value), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput,
            TransactionInput input, CancellationToken receiptRequestCancellationToken)
        {
            var encodedInput = GetData(functionInput);
            input.Data = encodedInput;
            return base.SendTransactionAndWaitForReceiptAsync(input, receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
           CancellationTokenSource receiptRequestCancellationTokenSource = null)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(functionInput, from),
                receiptRequestCancellationTokenSource);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationTokenSource = null)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(functionInput, from, gas, value),
                receiptRequestCancellationTokenSource);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationTokenSource = null)
        {
            return base.SendTransactionAndWaitForReceiptAsync(
                CreateTransactionInput(functionInput, from, gas, gasPrice, value), receiptRequestCancellationTokenSource);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput,
            TransactionInput input, CancellationTokenSource receiptRequestCancellationTokenSource = null)
        {
            var encodedInput = GetData(functionInput);
            input.Data = encodedInput;
            return base.SendTransactionAndWaitForReceiptAsync(input, receiptRequestCancellationTokenSource);
        }
#endif
    }
}
