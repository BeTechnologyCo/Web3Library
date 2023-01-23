using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.ProgressRepositories
{
    public interface IBlockProgressRepository
    {
        UniTask UpsertProgressAsync(BigInteger blockNumber);
        UniTask<BigInteger?> GetLastBlockNumberProcessedAsync();
    }
}