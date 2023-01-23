using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace Nethereum.RPC.Eth.Compilation
{
    public interface IEthCompileSolidity
    {
        RpcRequest BuildRequest(string contractCode, object id = null);
        UniTask<JToken> SendRequestAsync(string contractCode, object id = null);
    }
}