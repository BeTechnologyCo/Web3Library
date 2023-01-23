using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Blocks
{
    public interface IEthGetBlockWithTransactionsByNumber
    {
        RpcRequest BuildRequest(BlockParameter blockParameter, object id = null);
        RpcRequest BuildRequest(HexBigInteger number, object id = null);
        UniTask<BlockWithTransactions> SendRequestAsync(BlockParameter blockParameter, object id = null);
        UniTask<BlockWithTransactions> SendRequestAsync(HexBigInteger number, object id = null);
    }
}