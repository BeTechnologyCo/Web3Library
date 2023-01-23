using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.Contracts.DeploymentHandlers
{
    public interface IDeploymentTransactionSenderHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        UniTask<string> SendTransactionAsync(TContractDeploymentMessage deploymentMessage = null);
    }
}