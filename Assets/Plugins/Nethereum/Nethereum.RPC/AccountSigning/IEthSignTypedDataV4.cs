using Nethereum.JsonRpc.Client;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.AccountSigning
{
    public interface IEthSignTypedDataV4 : ISignTypedDataV4
    {
        RpcRequest BuildRequest(string message, object id = null);
    }

    public interface ISignTypedDataV4
    {
        UniTask<string> SendRequestAsync(string jsonMessage, object id = null);
    }
}