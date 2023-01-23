using Nethereum.JsonRpc.Client;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.HostWallet
{
    public interface IWalletWatchAsset
    {
        RpcRequest BuildRequest(WatchAssetParameter watchAssetParameter, object id = null);
        UniTask<bool> SendRequestAsync(WatchAssetParameter watchAssetParameter, object id = null);
    }
}