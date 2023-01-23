using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth.Mining
{
    public interface IEthSubmitHashrate
    {
        RpcRequest BuildRequest(string hashRate, string clientId, object id = null);
        UniTask<bool> SendRequestAsync(string hashRate, string clientId, object id = null);
    }
}