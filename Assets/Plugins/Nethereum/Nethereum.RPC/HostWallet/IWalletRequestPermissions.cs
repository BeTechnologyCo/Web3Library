using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.HostWallet
{
    public interface IWalletRequestPermissions
    {
        RpcRequest BuildRequest(string[] methods, object id = null);
        UniTask<JObject> SendRequestAsync(string[] methods, object id = null);

    }
}