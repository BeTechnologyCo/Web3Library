using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthSendRawTransaction
    {
        RpcRequest BuildRequest(string signedTransactionData, object id = null);
        UniTask<string> SendRequestAsync(string signedTransactionData, object id = null);
    }
}