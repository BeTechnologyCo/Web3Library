using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.DebugNode
{
    public interface IDebugGetRawTransaction
    {
        RpcRequest BuildRequest(string transactionHash, object id = null);
        UniTask<string> SendRequestAsync(string transactionHash, object id = null);
    }
}