using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.Contracts.DeploymentHandlers
{
    public interface IDeploymentSigner<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        UniTask<string> SignTransactionAsync(TContractDeploymentMessage deploymentMessage);
    }
}