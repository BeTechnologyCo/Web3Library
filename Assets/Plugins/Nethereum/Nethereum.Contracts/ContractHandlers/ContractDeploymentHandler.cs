using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.DeploymentHandlers;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts.ContractHandlers
{
#if !DOTNET35
    public class ContractDeploymentTransactionHandler<TContractDeploymentMessage> : ContractTransactionHandlerBase, IContractDeploymentTransactionHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        private readonly IDeploymentEstimatorHandler<TContractDeploymentMessage> _estimatorHandler;
        private readonly IDeploymentTransactionReceiptPollHandler<TContractDeploymentMessage> _receiptPollHandler;
        private readonly IDeploymentTransactionSenderHandler<TContractDeploymentMessage> _transactionSenderHandler;
        private readonly IDeploymentSigner<TContractDeploymentMessage> _transactionSigner;
  
        public ContractDeploymentTransactionHandler(ITransactionManager transactionManager) : base(transactionManager)
        {
            _estimatorHandler = new DeploymentEstimatorHandler<TContractDeploymentMessage>(transactionManager);
            _receiptPollHandler = new DeploymentTransactionReceiptPollHandler<TContractDeploymentMessage>(transactionManager);
            _transactionSenderHandler = new DeploymentTransactionSenderHandler<TContractDeploymentMessage>(transactionManager);
            _transactionSigner = new DeploymentSigner<TContractDeploymentMessage>(transactionManager);
        }

        public UniTask<string> SignTransactionAsync(TContractDeploymentMessage contractDeploymentMessage)
        {
            return _transactionSigner.SignTransactionAsync(contractDeploymentMessage);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(
            TContractDeploymentMessage contractDeploymentMessage = null, CancellationTokenSource tokenSource = null)
        {
            return _receiptPollHandler.SendTransactionAsync(contractDeploymentMessage, tokenSource);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(
            TContractDeploymentMessage contractDeploymentMessage, CancellationToken cancellationToken)
        {
            return _receiptPollHandler.SendTransactionAsync(contractDeploymentMessage, cancellationToken);
        }

        public UniTask<string> SendRequestAsync(TContractDeploymentMessage contractDeploymentMessage = null)
        {
            return _transactionSenderHandler.SendTransactionAsync(contractDeploymentMessage);
        }

        public UniTask<HexBigInteger> EstimateGasAsync(TContractDeploymentMessage contractDeploymentMessage)
        {
            return _estimatorHandler.EstimateGasAsync(contractDeploymentMessage);
        }

        public async UniTask<TransactionInput> CreateTransactionInputEstimatingGasAsync(TContractDeploymentMessage deploymentMessage = null)
        {
            if (deploymentMessage == null) deploymentMessage = new TContractDeploymentMessage();
            var gasEstimate = await EstimateGasAsync(deploymentMessage);
            deploymentMessage.Gas = gasEstimate;
            return deploymentMessage.CreateTransactionInput();
        }
    }
#endif

}