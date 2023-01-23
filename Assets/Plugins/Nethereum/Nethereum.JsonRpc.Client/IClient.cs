using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Nethereum.JsonRpc.Client
{
    public interface IClient : IBaseClient
    {
        UniTask<RpcRequestResponseBatch> SendBatchRequestAsync(RpcRequestResponseBatch rpcRequestResponseBatch);
        UniTask<T> SendRequestAsync<T>(RpcRequest request, string route = null);
        UniTask<T> SendRequestAsync<T>(string method, string route = null, params object[] paramList);
    }
}