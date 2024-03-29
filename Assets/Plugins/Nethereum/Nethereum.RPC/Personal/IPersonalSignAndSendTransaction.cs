﻿using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Personal
{
    public interface IPersonalSignAndSendTransaction
    {
        RpcRequest BuildRequest(TransactionInput txn, string password, object id = null);
        UniTask<string> SendRequestAsync(TransactionInput txn, string password, object id = null);
    }
}