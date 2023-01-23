using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public interface IAddressTransactionRepository
    {
        UniTask UpsertAsync(
            TransactionReceiptVO transactionReceiptVO, string address, string error = null, 
            string newContractAddress = null);

        UniTask<IAddressTransactionView> FindAsync(
            string address, HexBigInteger blockNumber, string transactionHash);
    }
}