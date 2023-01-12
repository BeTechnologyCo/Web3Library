
using Nethereum.Contracts;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WalletConnectSharp.Core.Network;
using WalletConnectSharp.NEthereum;
using WalletConnectSharp.Unity;

namespace Web3Unity
{
    public class Web3Connect
    {
        public string RpcUrl { get; private set; }

        public string PrivateKey { get; private set; }

        public string ChainId { get; set; }

        public ConnectionType ConnectionType { get; private set; }

        public Web3WC Web3WC { get; private set; }

        public MetamaskProvider MetamaskProvider { get; private set; }

        private static readonly Lazy<Web3Connect> lazy =
        new Lazy<Web3Connect>(() => new Web3Connect());

        public Web3 Web3 { get; private set; }


        public static Web3Connect Instance { get { return lazy.Value; } }

        /// <summary>
        /// Fire when wallet connected, return current account (Metamask & WalletConnect)
        /// </summary>
        public event EventHandler<string> Connected;

        public string AccountAddress { get; private set; }

        private Web3Connect()
        {
            WalletConnect.Instance.Session.OnSessionConnect += Session_OnSessionConnect;
        }


        /// <summary>
        /// Etablish a connection with nethereum classic RPC
        /// </summary>
        /// <param name="rpcUrl">rpc url to connect</param>
        /// <param name="privateKey">private key to sign call</param>
        public void ConnectRPC(string rpcUrl = "https://rpc.builder0x69.io", string privateKey = "0x3141592653589793238462643383279502884197169399375105820974944592")
        {
            ConnectionType = ConnectionType.RPC;
            PrivateKey = privateKey;
            RpcUrl = rpcUrl;
            var account = new Account(PrivateKey);
            Web3 = new Web3(account, RpcUrl);
        }

        /// <summary>
        /// Etablish a connection with metasmaks browser plugin (only for webGL)
        /// </summary>
        /// <param name="autoConnect">Request connection to account at init</param>
        public void ConnectMetamask(bool autoConnect = false)
        {
            ConnectionType = ConnectionType.Metamask;
            MetamaskProvider = new MetamaskProvider(autoConnect);
            MetamaskProvider.OnAccountConnected += MetamaskProvider_OnAccountConnected;
            Web3 = new Web3(MetamaskProvider);
        }

        private void MetamaskProvider_OnAccountConnected(object sender, string e)
        {
            AccountAddress = e;
            if (Connected != null)
            {
                Connected(this, e);
            }
        }

        /// <summary>
        /// Etablish a connection with wallet connect
        /// </summary>
        /// <param name="rpcUrl">The rpc url to call contract</param>
        /// <param name="name">Name of the dapp who appears in the popin in the wallet</param>
        /// <param name="description">Description of the dapp</param>
        /// <param name="icon">Icon show on the popin</param>
        /// <param name="url">Url to the project</param>
        /// <returns>The uri to connect to wallet connect</returns>
        public string ConnectWalletConnect(string rpcUrl = "https://rpc.builder0x69.io", string name = "Test Unity", string description = "Test dapp", string icon = "https://unity.com/favicon.ico", string url = "https://unity.com/")
        {
            ConnectionType = ConnectionType.WalletConnect;
            RpcUrl = rpcUrl;
            Web3 = WalletConnect.Instance.Session.BuildWeb3(new Uri(rpcUrl)).AsWalletAccount(true);
            // Web3WC = new Web3WC(transport, rpcUrl, name, description, icon, url);
            // Web3WC.Connected += Web3WC_Connected;

            return ""; //Web3WC.Uri;
        }

        private void Session_OnSessionConnect(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {
            //Web3Connect.Instance.ConnectWalletConnect("https://rpc.ankr.com/fantom_testnet");
            AccountAddress = e.Accounts[0];
        }

        private void Web3WC_Connected(object sender, string e)
        {
            AccountAddress = e;
            Web3 = Web3WC.Web3Client;
            if (Connected != null)
            {
                Connected(this, e);
            }
        }

        public async Task<string> Sign(string message, MetamaskSignature sign = MetamaskSignature.personal_sign)
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskProvider.Sign(message, sign);
            }
            else
            {
                var signer1 = new EthereumMessageSigner();
                return signer1.EncodeUTF8AndSign(message, new EthECKey(PrivateKey));
            }
        }

        public void Disconnect()
        {
        }
    }
}
