using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Blocks
{
    public interface IEthGetBlockTransactionCountByNumber
    {
        RpcRequest BuildRequest(BlockParameter block, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(object id = null);
        UniTask<HexBigInteger> SendRequestAsync(BlockParameter block, object id = null);
    }
}