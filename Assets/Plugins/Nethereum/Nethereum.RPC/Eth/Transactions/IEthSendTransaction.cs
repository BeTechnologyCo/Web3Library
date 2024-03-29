﻿using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthSendTransaction
    {
        RpcRequest BuildRequest(TransactionInput input, object id = null);
        UniTask<string> SendRequestAsync(TransactionInput input, object id = null);
    }
}