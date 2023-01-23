using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.Personal
{
    public interface IPersonalLockAccount
    {
        RpcRequest BuildRequest(string account, object id = null);
        UniTask<bool> SendRequestAsync(string account, object id = null);
    }
}