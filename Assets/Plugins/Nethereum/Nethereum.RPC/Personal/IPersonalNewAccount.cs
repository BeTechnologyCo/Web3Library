using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Personal
{
    public interface IPersonalNewAccount
    {
        RpcRequest BuildRequest(string passPhrase, object id = null);
        UniTask<string> SendRequestAsync(string passPhrase, object id = null);
    }
}