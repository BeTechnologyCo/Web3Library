using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace Nethereum.RPC.Eth.Compilation
{
    public interface IEthCompileSerpent
    {
        RpcRequest BuildRequest(string serpentCode, object id = null);
        UniTask<JObject> SendRequestAsync(string serpentCode, object id = null);
    }
}