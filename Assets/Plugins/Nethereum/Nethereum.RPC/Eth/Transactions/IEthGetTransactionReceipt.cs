using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthGetTransactionReceipt
    {
        RpcRequest BuildRequest(string transactionHash, object id = null);
        UniTask<TransactionReceipt> SendRequestAsync(string transactionHash, object id = null);
    }
}