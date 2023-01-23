using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Filters
{
    public interface IEthUninstallFilter
    {
        RpcRequest BuildRequest(HexBigInteger filterId, object id = null);
        UniTask<bool> SendRequestAsync(HexBigInteger filterId, object id = null);
    }
}