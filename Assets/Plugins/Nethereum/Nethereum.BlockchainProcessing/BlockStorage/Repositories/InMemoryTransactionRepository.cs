using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        public List<ITransactionView> Records { get; set;}

        public InMemoryTransactionRepository(List<ITransactionView> records)
        {
            Records = records;
        }

        public UniTask<ITransactionView> FindByBlockNumberAndHashAsync(HexBigInteger blockNumber, string hash)
        {
            return UniTask.FromResult(Records.FirstOrDefault(r => r.BlockNumber == blockNumber.Value.ToString() && r.Hash == hash));
        }

        public async UniTask UpsertAsync(TransactionReceiptVO transactionReceiptVO, string code, bool failedCreatingContract)
        {
            var record = await FindByBlockNumberAndHashAsync(transactionReceiptVO.BlockNumber, transactionReceiptVO.TransactionHash);
            if(record != null ) Records.Remove(record);
            Records.Add(transactionReceiptVO.MapToStorageEntityForUpsert(code, failedCreatingContract));
        }

        public async UniTask UpsertAsync(TransactionReceiptVO transactionReceiptVO)
        {
            var record = await FindByBlockNumberAndHashAsync(transactionReceiptVO.BlockNumber, transactionReceiptVO.TransactionHash);
            if (record != null) Records.Remove(record);
            Records.Add(transactionReceiptVO.MapToStorageEntityForUpsert());
        }
    }
}
