using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Uncles
{
    public interface IEthGetUncleCountByBlockHash
    {
        RpcRequest BuildRequest(string hash, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(string hash, object id = null);
    }
}