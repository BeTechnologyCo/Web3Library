using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Mining
{
    public interface IEthSubmitWork
    {
        RpcRequest BuildRequest(string nonce, string header, string mix, object id = null);
        UniTask<bool> SendRequestAsync(string nonce, string header, string mix, object id = null);
    }
}