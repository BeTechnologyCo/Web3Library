using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Eth
{
    public interface IEthCreateAccessList
    {
        BlockParameter DefaultBlock { get; set; }

        RpcRequest BuildRequest(TransactionInput transactionInput, BlockParameter block, object id = null);
        UniTask<AccessListGasUsed> SendRequestAsync(TransactionInput transactionInput, object id = null);
        UniTask<AccessListGasUsed> SendRequestAsync(TransactionInput transactionInput, BlockParameter block, object id = null);
    }
}