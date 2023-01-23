using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Blocks
{
    public interface IEthGetBlockWithTransactionsHashesByHash
    {
        RpcRequest BuildRequest(string blockHash, object id = null);
        UniTask<BlockWithTransactionHashes> SendRequestAsync(string blockHash, object id = null);
    }
}