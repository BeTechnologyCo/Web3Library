using Nethereum.BlockchainProcessing.ProgressRepositories;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Orchestrator
{
    public interface IBlockchainProcessingOrchestrator
    {
        UniTask<OrchestrationProgress> ProcessAsync(BigInteger fromNumber, BigInteger toNumber, CancellationToken cancellationToken = default(CancellationToken), IBlockProgressRepository blockProgressRepository = null);  
    }
}