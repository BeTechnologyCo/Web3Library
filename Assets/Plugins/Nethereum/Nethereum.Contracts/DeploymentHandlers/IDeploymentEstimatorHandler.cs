using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Nethereum.Contracts.DeploymentHandlers
{
    public interface IDeploymentEstimatorHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
        UniTask<HexBigInteger> EstimateGasAsync(TContractDeploymentMessage deploymentMessage);
    }
}