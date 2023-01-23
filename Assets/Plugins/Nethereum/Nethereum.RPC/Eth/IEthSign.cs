using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Eth
{
    public interface IEthSign
    {
        RpcRequest BuildRequest(string address, string data, object id = null);
        UniTask<string> SendRequestAsync(string address, string data, object id = null);
    }
}