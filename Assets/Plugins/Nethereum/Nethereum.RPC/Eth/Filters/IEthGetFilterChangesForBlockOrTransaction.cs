using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Filters
{
    public interface IEthGetFilterChangesForBlockOrTransaction
    {
        RpcRequest BuildRequest(HexBigInteger filterId, object id = null);
        UniTask<string[]> SendRequestAsync(HexBigInteger filterId, object id = null);
    }
}