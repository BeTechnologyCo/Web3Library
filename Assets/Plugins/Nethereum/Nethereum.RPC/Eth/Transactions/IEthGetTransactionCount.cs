﻿using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthGetTransactionCount
    {
        BlockParameter DefaultBlock { get; set; }

        RpcRequest BuildRequest(string address, BlockParameter block, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(string address, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(string address, BlockParameter block, object id = null);
    }
}