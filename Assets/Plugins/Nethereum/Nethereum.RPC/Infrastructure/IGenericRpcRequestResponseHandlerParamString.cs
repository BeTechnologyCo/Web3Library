using Nethereum.JsonRpc.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Infrastructure
{
    public interface IGenericRpcRequestResponseHandlerParamString<T>
    {
        UniTask<T> SendRequestAsync(string str, object id = null);
        RpcRequest BuildRequest(string str, object id = null);
    }
}
