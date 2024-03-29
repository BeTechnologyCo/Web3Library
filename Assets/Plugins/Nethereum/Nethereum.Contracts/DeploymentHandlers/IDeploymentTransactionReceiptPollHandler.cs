﻿using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.DeploymentHandlers
{
    public interface IDeploymentTransactionReceiptPollHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        UniTask<TransactionReceipt> SendTransactionAsync(TContractDeploymentMessage deploymentMessage = null, CancellationTokenSource cancellationTokenSource = null);
        UniTask<TransactionReceipt> SendTransactionAsync(TContractDeploymentMessage deploymentMessage, CancellationToken cancellationToken);
    }
}