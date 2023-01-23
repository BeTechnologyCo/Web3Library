using System;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Extensions;
using Nethereum.Contracts.TransactionHandlers;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts.ContractHandlers
{
#if !DOTNET35
    public class ContractTransactionHandler<TContractMessage> : ContractTransactionHandlerBase, IContractTransactionHandler<TContractMessage> where TContractMessage : FunctionMessage, new()
    {
        private readonly ITransactionEstimatorHandler<TContractMessage> _estimatorHandler;
        private readonly ITransactionReceiptPollHandler<TContractMessage> _receiptPollHandler;
        private readonly ITransactionSenderHandler<TContractMessage> _transactionSenderHandler;
        private readonly ITransactionSigner<TContractMessage> _transactionSigner;


        public ContractTransactionHandler(ITransactionManager transactionManager) : base(transactionManager)
        {
            _estimatorHandler = new TransactionEstimatorHandler<TContractMessage>(transactionManager);
            _receiptPollHandler = new TransactionReceiptPollHandler<TContractMessage>(transactionManager);
            _transactionSenderHandler = new TransactionSenderHandler<TContractMessage>(transactionManager);
            _transactionSigner = new TransactionSignerHandler<TContractMessage>(transactionManager);
        }

        public UniTask<string> SignTransactionAsync(
            string contractAddress, TContractMessage functionMessage = null)
        {
            return _transactionSigner.SignTransactionAsync(contractAddress, functionMessage);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(
            string contractAddress, TContractMessage functionMessage = null, CancellationTokenSource tokenSource = null)
        {
            return _receiptPollHandler.SendTransactionAsync(contractAddress, functionMessage, tokenSource);
        }

        public UniTask<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(
            string contractAddress, TContractMessage functionMessage, CancellationToken cancellationToken)
        {
            return _receiptPollHandler.SendTransactionAsync(contractAddress, functionMessage, cancellationToken);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractAddress, TContractMessage functionMessage, CancellationToken cancellationToken)
        {
            return SendTransactionAndWaitForReceiptAsync(contractAddress, functionMessage, cancellationToken);
        }

        [Obsolete("Use " + nameof(SendTransactionAndWaitForReceiptAsync) + " instead")]
        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(
            string contractAddress, TContractMessage functionMessage = null, CancellationTokenSource tokenSource = null)
        {
            return SendTransactionAndWaitForReceiptAsync(contractAddress, functionMessage, tokenSource);
        }

        public UniTask<string> SendTransactionAsync(string contractAddress, TContractMessage functionMessage = null)
        {
            return _transactionSenderHandler.SendTransactionAsync(contractAddress, functionMessage);
        }

        [Obsolete("Use " + nameof(SendTransactionAsync) + " instead")]
        public UniTask<string> SendRequestAsync(string contractAddress, TContractMessage functionMessage = null)
        {
            return SendTransactionAsync(contractAddress, functionMessage);
        }

        public async UniTask<TransactionInput> CreateTransactionInputEstimatingGasAsync(
            string contractAddress, TContractMessage functionMessage = null)
        {
            var gasEstimate = await EstimateGasAsync(contractAddress, functionMessage);
            functionMessage.Gas = gasEstimate;
            return functionMessage.CreateTransactionInput(contractAddress);
        }

        public UniTask<HexBigInteger> EstimateGasAsync(string contractAddress, TContractMessage functionMessage = null)
        {
            return _estimatorHandler.EstimateGasAsync(contractAddress, functionMessage);
        }

       
    }
#endif

}