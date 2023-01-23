using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.TransactionManagers;
using Newtonsoft.Json.Linq;

namespace Nethereum.Contracts
{

    public abstract class FunctionBase
    {
        private readonly Contract _contract;
        protected FunctionBuilderBase FunctionBuilderBase { get; set; }
        private ContractCall _contractCall;
        protected ITransactionManager TransactionManager => _contract.Eth.TransactionManager;

        public string ContractAddress => _contract.Address;

        protected FunctionBase(Contract contract, FunctionBuilderBase functionBuilder)
        {
            FunctionBuilderBase = functionBuilder;
            _contract = contract;
            if (contract.Eth != null) 
            {
                _contractCall = new ContractCall(_contract.Eth?.Transactions.Call, _contract.Eth?.DefaultBlock);
            }

        }
#if !DOTNET35

        public UniTask<string> CallAsync(CallInput callInput, BlockParameter block = null)
        {
            return _contractCall.CallAsync(callInput, block);
        }

        public UniTask<string> SendTransactionAsync(string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return SendTransactionAsync(FunctionBuilderBase.CreateTransactionInput(from, gas, value));
        }

        protected UniTask<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            return TransactionManager.SendTransactionAsync(transactionInput);
        }
   

        protected UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TransactionInput transactionInput,
            CancellationTokenSource cancellationTokenSource = null)
        {
            return cancellationTokenSource == null
                ? SendTransactionAndWaitForReceiptAsync(transactionInput, CancellationToken.None)
                : SendTransactionAndWaitForReceiptAsync(transactionInput, cancellationTokenSource.Token);
        }

        protected UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TransactionInput transactionInput,
            CancellationToken receiptRequestCancellationToken)
        {
            return TransactionManager.TransactionReceiptService.SendRequestAndWaitForReceiptAsync(transactionInput,
                receiptRequestCancellationToken);
        }

        public async UniTask<byte[]> CallRawAsync(CallInput callInput)
        {
            var result = await CallAsync(callInput);
            return result.HexToByteArray();
        }

        public async UniTask<byte[]> CallRawAsync(CallInput callInput, BlockParameter block)
        {
            var result = await CallAsync(callInput, block);
            return result.HexToByteArray();
        }

        public async UniTask<List<ParameterOutput>> CallDecodingToDefaultAsync(CallInput callInput, BlockParameter block)
        {
            var result = await CallAsync(callInput, block);
            return FunctionBuilderBase.DecodeOutput(result);
        }

        public async UniTask<List<ParameterOutput>> CallDecodingToDefaultAsync(CallInput callInput)
        {
            var result = await CallAsync(callInput);
            return FunctionBuilderBase.DecodeOutput(result);
        }

        protected async UniTask<TReturn> CallAsync<TReturn>(CallInput callInput)
        {
            var result = await CallAsync(callInput);
            return FunctionBuilderBase.DecodeTypeOutput<TReturn>(result);
        }

        protected async UniTask<TReturn> CallAsync<TReturn>(CallInput callInput, BlockParameter block)
        {
            var result = await CallAsync(callInput, block);
            return FunctionBuilderBase.DecodeTypeOutput<TReturn>(result);
        }

        protected async UniTask<TReturn> CallAsync<TReturn>(TReturn functionOuput, CallInput callInput)
        {
            var result = await CallAsync(callInput);
            return FunctionBuilderBase.DecodeDTOTypeOutput(functionOuput, result);
        }

        protected async UniTask<TReturn> CallAsync<TReturn>(TReturn functionOuput, CallInput callInput,
            BlockParameter block)
        {
            var result = await CallAsync(callInput, block);
            return FunctionBuilderBase.DecodeDTOTypeOutput(functionOuput, result);
        }

        protected async UniTask<HexBigInteger> EstimateGasFromEncAsync(CallInput callInput)
        {
            try
            {
                return
                    await
                        TransactionManager.EstimateGasAsync(callInput)
                            ;
            }
            catch (RpcResponseException rpcException)
            {
                ContractRevertExceptionHandler.HandleContractRevertException(rpcException);
                throw;
            }
        }
#endif
        public List<ParameterOutput> DecodeInput(string data)
        {
            return FunctionBuilderBase.DecodeInput(data);
        }

        public TReturn DecodeTypeOutput<TReturn>(string output)
        {
            return FunctionBuilderBase.DecodeTypeOutput<TReturn>(output);
        }

        public TReturn DecodeDTOTypeOutput<TReturn>(TReturn functionOuput, string output)
        {
            return FunctionBuilderBase.DecodeDTOTypeOutput(functionOuput, output);
        }

        public TReturn DecodeDTOTypeOutput<TReturn>(string output) where TReturn : new()
        {
            return FunctionBuilderBase.DecodeDTOTypeOutput<TReturn>(output);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return FunctionBuilderBase.CreateTransactionInput(from, gas, value);
        }

        public object[] ConvertJsonToObjectInputParameters(string json)
        {
            return FunctionBuilderBase.ConvertJsonToObjectInputParameters(json);
        }

        public object[] ConvertJsonToObjectInputParameters(JObject jObject)
        {
            return FunctionBuilderBase.ConvertJsonToObjectInputParameters(jObject);
        }
    }
}