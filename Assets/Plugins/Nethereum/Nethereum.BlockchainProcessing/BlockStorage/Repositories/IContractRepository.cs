using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public interface IContractRepository
    {
        UniTask FillCacheAsync();
        UniTask UpsertAsync(ContractCreationVO contractCreation);
        UniTask<bool> ExistsAsync(string contractAddress);

        UniTask<IContractView> FindByAddressAsync(string contractAddress);
        bool IsCached(string contractAddress);
    }
}