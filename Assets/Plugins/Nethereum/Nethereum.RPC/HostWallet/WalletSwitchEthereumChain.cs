using Nethereum.JsonRpc.Client;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Nethereum.RPC.HostWallet
{
    /// <summary>
    /// The wallet_addEthereumChain RPC method allows Ethereum applications (“dapps”) to suggest chains to be added to the user’s wallet application. The caller must specify a chain ID and some chain metadata. The wallet application may arbitrarily refuse or accept the request. null is returned if the chain was added, and an error otherwise.
    /// https://eips.ethereum.org/EIPS/eip-3085
    /// </summary>
    public interface IWalletSwitchEthereumChain
    {
        RpcRequest BuildRequest(SwitchEthereumChainParameter switchEthereumChainParameter, object id = null);

#if !DOTNET35
        UniTask<string> SendRequestAsync(SwitchEthereumChainParameter switchEthereumChainParameter, object id = null);
#endif
    }


    /// <summary>
    /// The wallet_addEthereumChain RPC method allows Ethereum applications (“dapps”) to suggest chains to be added to the user’s wallet application. The caller must specify a chain ID and some chain metadata. The wallet application may arbitrarily refuse or accept the request. null is returned if the chain was added, and an error otherwise.
    /// https://eips.ethereum.org/EIPS/eip-3085
    /// </summary>
    public class WalletSwitchEthereumChain : RpcRequestResponseHandler<string>, IWalletSwitchEthereumChain
    {
        public WalletSwitchEthereumChain() : this(null)
        {
        }

        public WalletSwitchEthereumChain(IClient client) : base(client, ApiMethods.wallet_switchEthereumChain.ToString())
        {

        }

#if !DOTNET35
        public async UniTask<string> SendRequestAsync(SwitchEthereumChainParameter switchEthereumChainParameter, object id = null)
        {
            await base.SendRequestAsync(id, switchEthereumChainParameter);

            return null;
        }
#endif
        public RpcRequest BuildRequest(SwitchEthereumChainParameter switchEthereumChainParameter, object id = null)
        {
            return base.BuildRequest(id, switchEthereumChainParameter);
        }
    }
}
