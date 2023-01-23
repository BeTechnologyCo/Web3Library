using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.CQS
{
    public interface IContractDeploymentTransactionHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        UniTask<TransactionInput> CreateTransactionInputEstimatingGasAsync(TContractDeploymentMessage deploymentMessage = null);
        UniTask<HexBigInteger> EstimateGasAsync(TContractDeploymentMessage contractDeploymentMessage);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(TContractDeploymentMessage contractDeploymentMessage = null, CancellationTokenSource tokenSource = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(TContractDeploymentMessage contractDeploymentMessage, CancellationToken cancellationToken);
        UniTask<string> SendRequestAsync(TContractDeploymentMessage contractDeploymentMessage = null);
        UniTask<string> SignTransactionAsync(TContractDeploymentMessage contractDeploymentMessage);
    }
}