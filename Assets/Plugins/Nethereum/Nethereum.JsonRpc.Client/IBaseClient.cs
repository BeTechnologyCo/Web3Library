using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Nethereum.JsonRpc.Client
{
    public interface IBaseClient
    {
        RequestInterceptor OverridingRequestInterceptor { get; set; }

        UniTask SendRequestAsync(RpcRequest request, string route = null);
        UniTask SendRequestAsync(string method, string route = null, params object[] paramList);
    }
}