using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Filters
{
    public interface IEthGetLogs
    {
        RpcRequest BuildRequest(NewFilterInput newFilter, object id = null);
        UniTask<FilterLog[]> SendRequestAsync(NewFilterInput newFilter, object id = null);
    }
}