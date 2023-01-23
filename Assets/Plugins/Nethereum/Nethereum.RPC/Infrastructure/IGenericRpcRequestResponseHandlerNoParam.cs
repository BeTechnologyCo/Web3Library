using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Infrastructure
{
    public interface IGenericRpcRequestResponseHandlerNoParam<TResponse>
    {
        UniTask<TResponse> SendRequestAsync(object id = null);
    }
}