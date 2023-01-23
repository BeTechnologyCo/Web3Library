using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public interface ITransactionRepository
    {
        UniTask UpsertAsync(TransactionReceiptVO transactionReceiptVO, string code, bool failedCreatingContract);

        UniTask UpsertAsync(TransactionReceiptVO transactionReceiptVO);

        UniTask<Entities.ITransactionView> FindByBlockNumberAndHashAsync(HexBigInteger blockNumber, string hash);
    }
}