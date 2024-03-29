﻿using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts.DeploymentHandlers
{
#if !DOTNET35
    public class DeploymentTransactionSenderHandler<TContractDeploymentMessage> : DeploymentHandlerBase<TContractDeploymentMessage>, 
        IDeploymentTransactionSenderHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        private readonly IDeploymentEstimatorHandler<TContractDeploymentMessage> _deploymentEstimatorHandler;

        public DeploymentTransactionSenderHandler(ITransactionManager transactionManager):base(transactionManager)
        {
            _deploymentEstimatorHandler = new DeploymentEstimatorHandler<TContractDeploymentMessage>(transactionManager);
        }

        public async UniTask<string> SendTransactionAsync(TContractDeploymentMessage deploymentMessage = null)
        {
            if(deploymentMessage == null) deploymentMessage = new TContractDeploymentMessage();
            deploymentMessage.Gas = await GetOrEstimateMaximumGasAsync(deploymentMessage);
            var transactionInput = DeploymentMessageEncodingService.CreateTransactionInput(deploymentMessage);
            return await TransactionManager.SendTransactionAsync(transactionInput);
        }

        protected virtual async UniTask<HexBigInteger> GetOrEstimateMaximumGasAsync(
            TContractDeploymentMessage deploymentMessage)
        {
            return deploymentMessage.GetHexMaximumGas()
                   ?? await _deploymentEstimatorHandler.EstimateGasAsync(deploymentMessage);
        }
    }
#endif
}