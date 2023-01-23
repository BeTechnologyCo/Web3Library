using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;
using Nethereum.RPC.TransactionTypes;

namespace Nethereum.Contracts
{
    public class DeployContract : IDeployContract
    {
        private readonly DeployContractTransactionBuilder _deployContractTransactionBuilder;

        public DeployContract(ITransactionManager transactionManager)
        {
            TransactionManager = transactionManager;
            _deployContractTransactionBuilder = new DeployContractTransactionBuilder();
        }

        public ITransactionManager TransactionManager { get; set; }

        public string GetData(string contractByteCode, string abi, params object[] values)
        {
            return _deployContractTransactionBuilder.GetData(contractByteCode, abi, values);
        }

        public string GetData<TConstructorParams>(string contractByteCode, TConstructorParams inputParams)
        {
            return _deployContractTransactionBuilder.GetData(contractByteCode, inputParams);
        }

#if !DOTNET35
        public UniTask<HexBigInteger> EstimateGasAsync(string abi, string contractByteCode, string from,
            params object[] values)
        {
            var callInput = _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, values);
            return TransactionManager.EstimateGasAsync(callInput);
        }

        public UniTask<HexBigInteger> EstimateGasAsync<TConstructorParams>(string contractByteCode, string from,
            TConstructorParams inputParams)
        {
            var callInput = _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, inputParams);
            return TransactionManager.EstimateGasAsync(callInput);
        }

        public UniTask<HexBigInteger> EstimateGasAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas,
            TConstructorParams inputParams)
        {
            var callInput =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, inputParams);
            return TransactionManager.EstimateGasAsync(callInput);
        }

        public UniTask<HexBigInteger> EstimateGasAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger value,
            TConstructorParams inputParams)
        {
            var callInput =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, null, value,
                    inputParams);
            return TransactionManager.EstimateGasAsync(callInput);
        }

        public UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas,
            HexBigInteger value,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, value, values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, gasPrice, value,
                    values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value,
            HexBigInteger nonce,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, gasPrice, value, nonce,
                    values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from,
            params object[] values)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(string abi, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, params object[] values)
        {
        
            var transaction = _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce, values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(HexBigInteger type, string abi, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, params object[] values)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(type, abi, contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce, values);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync(string contractByteCode, string from, HexBigInteger gas)
        {
            return TransactionManager.SendTransactionAsync(new TransactionInput(contractByteCode, gas, from));
        }

        public UniTask<string> SendRequestAsync(string contractByteCode, string from, HexBigInteger gas,
            HexBigInteger gasPrice, HexBigInteger value)
        {
            return TransactionManager.SendTransactionAsync(new TransactionInput(contractByteCode, null, from, gas,
                gasPrice, value));
        }

        public UniTask<string> SendRequestAsync(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce)
        {
            return TransactionManager.SendTransactionAsync(new TransactionInput(TransactionType.EIP1559.AsHexBigInteger(), contractByteCode, null, from, gas,
                value, maxFeePerGas, maxPriorityFeePerGas));
        }

        public UniTask<string> SendRequestAsync(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce)
        {
            return TransactionManager.SendTransactionAsync(new TransactionInput(type, contractByteCode, null, from, gas,
                value, maxFeePerGas, maxPriorityFeePerGas));
        }

        public UniTask<string> SendRequestAsync(string contractByteCode, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return TransactionManager.SendTransactionAsync(new TransactionInput(contractByteCode, null, from, gas,
                value));
        }

        public UniTask<string> SendRequestAsync(string contractByteCode, string from)
        {
            return TransactionManager.SendTransactionAsync(new TransactionInput(contractByteCode, null, from));
        }

        public UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from,
            TConstructorParams inputParams)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, inputParams);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, TConstructorParams inputParams)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, inputParams);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, TConstructorParams inputParams)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, gasPrice, value,
                    inputParams);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, gasPrice, value, nonce,
                    inputParams);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce,
                    inputParams);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<string> SendRequestAsync<TConstructorParams>(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(type, contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce,
                    inputParams);
            return TransactionManager.SendTransactionAsync(transaction);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from, HexBigInteger gas,CancellationToken receiptRequestCancellationToken,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from, HexBigInteger gas,
            HexBigInteger value,CancellationToken receiptRequestCancellationToken,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, value, values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from, HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value,CancellationToken receiptRequestCancellationToken,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, gasPrice, value,
                    values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from,CancellationToken receiptRequestCancellationToken,
            params object[] values)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            HexBigInteger gas,CancellationToken receiptRequestCancellationToken)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, gas, from), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value,
           CancellationToken receiptRequestCancellationToken)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, null, from, gas, gasPrice, value),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger value, CancellationToken receiptRequestCancellationToken)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, null, from, gas, value), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
           CancellationToken receiptRequestCancellationToken)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, null, from), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            HexBigInteger gas, TConstructorParams inputParams,
           CancellationToken receiptRequestCancellationToken)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, TConstructorParams inputParams,
           CancellationToken receiptRequestCancellationToken)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, gasPrice, value,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,
           CancellationToken receiptRequestCancellationToken)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, gasPrice, value, nonce,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,CancellationToken receiptRequestCancellationToken)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(type, contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }


        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
        string from, HexBigInteger gas, CancellationTokenSource receiptRequestCancellationToken = null,
        params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from, HexBigInteger gas,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, value, values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from, HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] values)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, gas, gasPrice, value,
                    values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string abi, string contractByteCode,
            string from, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] values)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(abi, contractByteCode, from, values);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            HexBigInteger gas, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, gas, from), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value,
            CancellationTokenSource receiptRequestCancellationToken = null)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, null, from, gas, gasPrice, value),
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, null, from, gas, value), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractByteCode, string from,
            CancellationTokenSource receiptRequestCancellationToken = null)
        {
            _deployContractTransactionBuilder.EnsureByteCodeDoesNotContainPlaceholders(contractByteCode);
            return DeployContractAndWaitForReceiptAsync(
                new TransactionInput(contractByteCode, null, from), receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var transaction = _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            HexBigInteger gas, TConstructorParams inputParams,
            CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, TConstructorParams inputParams,
           CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, gasPrice, value,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode,
            string from,
            HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams,
           CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, gasPrice, value, nonce,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TConstructorParams>(HexBigInteger type, string contractByteCode, string from,
            HexBigInteger gas, HexBigInteger maxFeePerGas, HexBigInteger maxPriorityFeePerGas, HexBigInteger value, HexBigInteger nonce, TConstructorParams inputParams, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var transaction =
                _deployContractTransactionBuilder.BuildTransaction(type, contractByteCode, from, gas, maxFeePerGas, maxPriorityFeePerGas, value, nonce,
                    inputParams);
            return DeployContractAndWaitForReceiptAsync(transaction,
                receiptRequestCancellationToken);
        }


        protected UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(TransactionInput transactionInput,
           CancellationTokenSource cancellationTokenSource = null)
        {
            return cancellationTokenSource == null
                ? DeployContractAndWaitForReceiptAsync(transactionInput, CancellationToken.None)
                : DeployContractAndWaitForReceiptAsync(transactionInput, cancellationTokenSource.Token);
        }

        protected UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(TransactionInput transactionInput,
            CancellationToken receiptRequestCancellationToken)
        {
            return TransactionManager.TransactionReceiptService.DeployContractAndWaitForReceiptAsync(transactionInput,
                receiptRequestCancellationToken);
        }
#endif
    }
}