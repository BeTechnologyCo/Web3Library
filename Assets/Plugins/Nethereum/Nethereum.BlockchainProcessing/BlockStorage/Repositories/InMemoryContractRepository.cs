using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public class InMemoryContractRepository : IContractRepository
    {
        public List<IContractView> Records { get; set; }

        public InMemoryContractRepository(List<IContractView> records)
        {
            Records = records;
        }

        public async UniTask<bool> ExistsAsync(string contractAddress)
        {
            var existing = await FindByAddressAsync(contractAddress);
            return existing != null;
        }

        public UniTask FillCacheAsync() => UniTask.FromResult(0);

        public UniTask<IContractView> FindByAddressAsync(string contractAddress)
        {
            IContractView contract = Find(contractAddress);
            return UniTask.FromResult(contract);
        }

        private IContractView Find(string contractAddress)
        {
            return Records.FirstOrDefault(r => AddressUtil.Current.AreAddressesTheSame(r.Address, contractAddress));
        }

        public bool IsCached(string contractAddress)
        {
            return Find(contractAddress) != null;
        }

        public async UniTask UpsertAsync(ContractCreationVO contractCreation)
        {
            var record = await FindByAddressAsync(contractCreation.ContractAddress);
            if(record != null) Records.Remove(record);
            Records.Add(contractCreation.MapToStorageEntityForUpsert());
        }
    }
}
