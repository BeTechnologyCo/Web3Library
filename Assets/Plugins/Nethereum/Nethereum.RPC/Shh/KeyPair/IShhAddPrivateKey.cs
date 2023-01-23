using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Shh.KeyPair
{
    public interface IShhAddPrivateKey
    {
        UniTask<string> SendRequestAsync(string privateKey, object id = null);
        RpcRequest BuildRequest(string privateKey, object id = null);
    }
}