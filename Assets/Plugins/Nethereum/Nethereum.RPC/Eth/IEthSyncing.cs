using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Infrastructure;

namespace Nethereum.RPC.Eth
{
    public interface IEthSyncing: IGenericRpcRequestResponseHandlerNoParam<object>
    {
#if !DOTNET35
        UniTask<SyncingOutput> SendRequestAsync(object id = null);
#endif
    }
}