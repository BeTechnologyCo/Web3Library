using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Blocks
{
    public interface IEthGetBlockWithTransactionsByHash
    {
        RpcRequest BuildRequest(string blockHash, object id = null);
        UniTask<BlockWithTransactions> SendRequestAsync(string blockHash, object id = null);
    }
}