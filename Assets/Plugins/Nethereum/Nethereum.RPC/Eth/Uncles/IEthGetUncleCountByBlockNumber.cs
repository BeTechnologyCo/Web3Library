using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Uncles
{
    public interface IEthGetUncleCountByBlockNumber
    {
        RpcRequest BuildRequest(HexBigInteger blockNumber, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(HexBigInteger blockNumber, object id = null);
    }
}