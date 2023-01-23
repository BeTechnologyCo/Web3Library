using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Shh.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Shh
{
    public interface IShhPost
    {
        RpcRequest BuildRequest(MessageInput input, object id = null);
        UniTask<string> SendRequestAsync(MessageInput input, object id = null);
    }
}
