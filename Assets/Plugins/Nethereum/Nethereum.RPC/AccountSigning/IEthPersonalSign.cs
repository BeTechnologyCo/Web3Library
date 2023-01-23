using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.AccountSigning
{
    public interface IEthPersonalSign : IEthereumMessageSign
    {
        RpcRequest BuildRequest(byte[] value, object id = null);
        RpcRequest BuildRequest(HexUTF8String utf8Hex, object id = null);
    }

    public interface IEthereumMessageSign
    {
        UniTask<string> SendRequestAsync(byte[] value, object id = null);
        UniTask<string> SendRequestAsync(HexUTF8String utf8Hex, object id = null);
    }
}