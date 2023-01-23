using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Hex.HexTypes;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public class InMemoryBlockRepository : IBlockRepository
    {
        public List<IBlockView> Records { get; set;}

        public InMemoryBlockRepository(List<IBlockView> records)
        {
            Records = records;
        }

        public UniTask<IBlockView> FindByBlockNumberAsync(HexBigInteger blockNumber)
        {
            var block = Records.FirstOrDefault(r => r.BlockNumber == blockNumber.Value.ToString());
            return UniTask.FromResult(block);
        }

        public async UniTask UpsertBlockAsync(RPC.Eth.DTOs.Block source)
        {
            var record = await FindByBlockNumberAsync(source.Number);
            if(record != null) Records.Remove(record);
            Records.Add(source.MapToStorageEntityForUpsert());
        }
    }
}
