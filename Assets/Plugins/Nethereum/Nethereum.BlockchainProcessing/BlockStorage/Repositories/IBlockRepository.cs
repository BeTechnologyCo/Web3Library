using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Hex.HexTypes;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public interface IBlockRepository
    {
        UniTask UpsertBlockAsync(Nethereum.RPC.Eth.DTOs.Block source);
        UniTask<IBlockView> FindByBlockNumberAsync(HexBigInteger blockNumber);
    }
}