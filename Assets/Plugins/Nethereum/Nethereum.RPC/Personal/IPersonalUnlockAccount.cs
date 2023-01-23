using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth;

namespace Nethereum.RPC.Personal
{
    public interface IPersonalUnlockAccount
    {
        RpcRequest BuildRequest(string address, string passPhrase, int? durationInSeconds, object id = null);
#if !DOTNET35
        UniTask<bool> SendRequestAsync(EthCoinBase coinbaseRequest, string passPhrase, object id = null);
#endif
        UniTask<bool> SendRequestAsync(string address, string passPhrase, HexBigInteger durationInSeconds, object id = null);
        UniTask<bool> SendRequestAsync(string address, string passPhrase, ulong? durationInSeconds, object id = null);
    }
}