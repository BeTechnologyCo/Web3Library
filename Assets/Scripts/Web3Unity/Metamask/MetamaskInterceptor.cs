using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC;
using Nethereum.RPC.Eth.DTOs;
using UnityEngine;

namespace Web3Unity
{
    public class MetamaskInterceptor : RequestInterceptor
    {
        private readonly MetamaskProvider _metamaskHostProvider;
        public MetamaskInterceptor(MetamaskProvider metamaskHostProvider)
        {
            _metamaskHostProvider = metamaskHostProvider;
        }

        public override  Task<object> InterceptSendRequestAsync<T>(
            Func<RpcRequest, string, Task<T>> interceptedSendRequestAsync, RpcRequest request,
            string route = null)
        {
            Debug.Log($"InterceptSendRequestAsync T {typeof(T)}");
            return await _metamaskHostProvider.SendRequestAsync<T>(request, route);

        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], Task<T>> interceptedSendRequestAsync, string method,
            string route = null, params object[] paramList)
        {
            Debug.Log($"InterceptSendRequestAsync T {typeof(T)}");
            return await _metamaskHostProvider.SendRequestAsync<T>(method, route, paramList);

        }

    }
}