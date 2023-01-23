using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public interface ITransactionLogRepository
    {
        UniTask UpsertAsync(FilterLogVO log);
        UniTask<ITransactionLogView> FindByTransactionHashAndLogIndexAsync(string hash, BigInteger logIndex);
    }
}