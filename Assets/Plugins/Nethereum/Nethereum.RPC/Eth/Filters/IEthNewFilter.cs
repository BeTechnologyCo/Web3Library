using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Filters
{
    public interface IEthNewFilter
    {
        RpcRequest BuildRequest(NewFilterInput newFilterInput, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(NewFilterInput newFilterInput, object id = null);
    }
}