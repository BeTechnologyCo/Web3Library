using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.ProgressRepositories
{
    public class InMemoryBlockchainProgressRepository : IBlockProgressRepository
    {
        public InMemoryBlockchainProgressRepository()
        {

        }

        public InMemoryBlockchainProgressRepository(BigInteger lastBlockProcessed)
        {
            LastBlockProcessed = lastBlockProcessed;
        }

        public BigInteger? LastBlockProcessed { get; private set;}

        public UniTask<BigInteger?> GetLastBlockNumberProcessedAsync() => UniTask.FromResult(LastBlockProcessed);

        public virtual UniTask UpsertProgressAsync(BigInteger blockNumber)
        {
            LastBlockProcessed = blockNumber;
            //Debug.WriteLine(blockNumber.ToString());
            return UniTask.FromResult(0);
        }
    }
}
