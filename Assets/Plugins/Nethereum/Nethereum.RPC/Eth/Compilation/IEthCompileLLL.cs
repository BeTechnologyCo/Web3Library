using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace Nethereum.RPC.Eth.Compilation
{
    public interface IEthCompileLLL
    {
        RpcRequest BuildRequest(string lllcode, object id = null);
        UniTask<JObject> SendRequestAsync(string lllcode, object id = null);
    }
}