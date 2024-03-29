﻿using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthCall
    {
        BlockParameter DefaultBlock { get; set; }

        RpcRequest BuildRequest(CallInput callInput, BlockParameter block, object id = null);
        UniTask<string> SendRequestAsync(CallInput callInput, object id = null);
        UniTask<string> SendRequestAsync(CallInput callInput, BlockParameter block, object id = null);
    }
}