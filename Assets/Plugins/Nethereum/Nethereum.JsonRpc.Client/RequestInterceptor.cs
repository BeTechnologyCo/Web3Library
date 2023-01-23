using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;

namespace Nethereum.JsonRpc.Client
{
    public abstract class RequestInterceptor
    {
        public virtual async UniTask<object> InterceptSendRequestAsync<T>(
            Func<RpcRequest, string, UniTask<T>> interceptedSendRequestAsync, RpcRequest request,
            string route = null)
        {
            return await interceptedSendRequestAsync(request, route);
        }

        public virtual async UniTask InterceptSendRequestAsync(
            Func<RpcRequest, string, UniTask> interceptedSendRequestAsync, RpcRequest request,
            string route = null)
        {
            await interceptedSendRequestAsync(request, route);
        }

        public virtual async UniTask<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], UniTask<T>> interceptedSendRequestAsync, string method,
            string route = null, params object[] paramList)
        {
            return await interceptedSendRequestAsync(method, route, paramList);
        }

        public virtual UniTask InterceptSendRequestAsync(
            Func<string, string, object[], UniTask> interceptedSendRequestAsync, string method,
            string route = null, params object[] paramList)
        {
             return interceptedSendRequestAsync(method, route, paramList);
        }
    }
}