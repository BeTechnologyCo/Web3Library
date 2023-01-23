using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthGetTransactionByHash
    {
        RpcRequest BuildRequest(string hashTransaction, object id = null);
        UniTask<Transaction> SendRequestAsync(string hashTransaction, object id = null);
    }
}