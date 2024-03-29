﻿using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.HostWallet;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System;
using UnityEngine;
using WalletConnectSharp.Core.Models.Ethereum.Types;

namespace Web3Unity
{
    public class Web3Connect
    {
        public string RpcUrl { get; private set; }

        public string PrivateKey { get; private set; }

        /// <summary>
        /// Current Chain Id connected (integer format)
        /// </summary>
        public string ChainId { get; set; }

        public ConnectionType ConnectionType { get; private set; }

        public WalletConnectProvider WalletConnectInstance { get; private set; }

        public MetamaskProvider MetamaskInstance { get; private set; }

        private static readonly Lazy<Web3Connect> lazy =
        new Lazy<Web3Connect>(() => new Web3Connect());

        public Web3 Web3 { get; private set; }


        public static Web3Connect Instance { get { return lazy.Value; } }

        /// <summary>
        /// Fire when wallet connected, return current account (Metamask & WalletConnect)
        /// </summary>
        public event EventHandler<string> OnConnected;

        /// <summary>
        /// Fire when wallet disconnected (Metamask & Walletconnect)
        /// </summary>
        public event EventHandler OnDisconnected;

        /// <summary>
        /// Fire when wallet account changed, return current account (Metamask & WalletConnect)
        /// </summary>
        public event EventHandler<string> OnAccountChanged;

        /// <summary>
        /// Fire when wallet chain changed, return current chain (Metamask & WalletConnect)
        /// </summary>
        public event EventHandler<string> OnChainChanged;

        /// <summary>
        /// Current public address of the account connected
        /// </summary>
        public string AccountAddress { get; private set; }

        public event EventHandler<string> UriGenerated;

        public bool Connected
        {
            get
            {
                switch (ConnectionType)
                {
                    case ConnectionType.None:
                        return false;
                    case ConnectionType.RPC:
                        return true;
                    case ConnectionType.WalletConnect:
                        return WalletConnectInstance?.Client?.Connected == true;
                    case ConnectionType.Metamask:
                        return MetamaskProvider.IsConnected();
                    default:
                        return false;
                }
                return false;
            }
        }

        private Web3Connect()
        {

        }


        /// <summary>
        /// Etablish a connection with nethereum classic RPC
        /// </summary>
        /// <param name="rpcUrl">rpc url to connect</param>
        /// <param name="privateKey">private key to sign call</param>
        public void ConnectRPC(string rpcUrl = "https://rpc.builder0x69.io", string privateKey = "0x3141592653589793238462643383279502884197169399375105820974944592")
        {
            RemoveSubscription();
            ConnectionType = ConnectionType.RPC;
            PrivateKey = privateKey;
            RpcUrl = rpcUrl;
            var account = new Account(PrivateKey);
            Web3 = new Web3(account, RpcUrl);

            AccountAddress = account.Address;
            if (OnConnected != null)
            {
                OnConnected(this, AccountAddress);
            }
            GetChainId();
        }

        /// <summary>
        /// Etablish a connection with metasmaks browser plugin (only for webGL)
        /// </summary>
        /// <param name="autoConnect">Request connection to account at init</param>
        public void ConnectMetamask(bool autoConnect = false)
        {
            RemoveSubscription();
            ConnectionType = ConnectionType.Metamask;
            MetamaskInstance = new MetamaskProvider(autoConnect);
            MetamaskProvider.OnAccountConnected += MetamaskProvider_OnAccountConnected;
            MetamaskProvider.OnChainChanged += MetamaskProvider_OnChainChanged;
            MetamaskProvider.OnAccountChanged += MetamaskProvider_OnAccountChanged;
            MetamaskProvider.OnAccountDisconnected += MetamaskProvider_OnAccountDisconnected;
            Web3 = new Web3(MetamaskInstance);
        }

        private void MetamaskProvider_OnAccountDisconnected(object sender, EventArgs e)
        {
            RemoveSubscription();
            ConnectionType = ConnectionType.None;
            Web3 = null;
            AccountAddress = string.Empty;
            ChainId =string.Empty;

            if (OnDisconnected != null)
            {
                OnDisconnected(this, new EventArgs());
            }
        }

        private void MetamaskProvider_OnAccountChanged(object sender, string e)
        {
            AccountAddress = e;
            if (OnAccountChanged != null)
            {
                OnAccountChanged(this, e);
            }
        }

        private async void GetChainId()
        {
            var getChainId = await Web3.Eth.ChainId.SendRequestAsync();
            ChainId = getChainId.ToString();
        }

        private void MetamaskProvider_OnChainChanged(object sender, string e)
        {
            ChainId = e;
        }

        private void MetamaskProvider_OnAccountConnected(object sender, string e)
        {
            AccountAddress = e;
            if (OnConnected != null)
            {
                OnConnected(this, e);
            }

            GetChainId();
        }

        private void RemoveSubscription()
        {
            if (MetamaskInstance != null)
            {
                // remove old subscription
                MetamaskProvider.OnAccountConnected -= MetamaskProvider_OnAccountConnected;
                MetamaskProvider.OnChainChanged -= MetamaskProvider_OnChainChanged;
                MetamaskProvider.OnAccountChanged -= MetamaskProvider_OnAccountChanged;
            }

            if (WalletConnectInstance != null)
            {
                // remove old subscription
                WalletConnectInstance.OnUriGenerated -= Web3WC_UriGenerated;
                WalletConnectInstance.OnAccountConnected -= Web3WC_Connected;
                if (WalletConnectInstance.Client != null)
                {
                    WalletConnectInstance.Client.SessionUpdate -= Client_SessionUpdate;
                }
            }
        }

        /// <summary>
        /// Etablish a connection with wallet connect
        /// </summary>
        /// <param name="rpcUrl">The rpc url to call contract</param>
        /// <param name="chainId">Chain id desired default 0 "not defined", use it if you want to prevent from send tx in the wrong chain</param>
        /// <param name="name">Name of the dapp who appears in the popin in the wallet</param>
        /// <param name="description">Description of the dapp</param>
        /// <param name="icon">Icon show on the popin</param>
        /// <param name="url">Url to the project</param>
        public async UniTask ConnectWalletConnect(string rpcUrl = "https://rpc.builder0x69.io", int chainId = 0, string name = "Test Unity", string description = "Test dapp", string icon = "https://unity.com/favicon.ico", string url = "https://unity.com/")
        {
            RemoveSubscription();
            ConnectionType = ConnectionType.WalletConnect;
            RpcUrl = rpcUrl;
           
            WalletConnectInstance = new WalletConnectProvider(rpcUrl, chainId, name, description, icon, url);
            WalletConnectInstance.OnUriGenerated += Web3WC_UriGenerated;
            WalletConnectInstance.OnAccountConnected += Web3WC_Connected;
            await WalletConnectInstance.Connect();

            Web3 = WalletConnectInstance.Web3Client;
        }

        /// <summary>
        /// Switch to chain
        /// </summary>
        /// <param name="chainId">Chain Id in decimal format</param>
        public async UniTask SwitchChain(int chainId)
        {
            var paramChain = new SwitchEthereumChainParameter()
            {
                ChainId = new HexBigInteger(chainId.ToString("X"))
            };
            await Web3.Eth.HostWallet.SwitchEthereumChain.SendRequestAsync(paramChain);
        }

        /// <summary>
        /// Switch to chain
        /// </summary>
        /// <param name="chainId">Chain Id in hexadecimal format</param>
        public async UniTask SwitchChain(string chainId)
        {
            var paramChain = new SwitchEthereumChainParameter()
            {
                ChainId = new HexBigInteger(chainId)
            };
            await Web3.Eth.HostWallet.SwitchEthereumChain.SendRequestAsync(paramChain);
        }

        /// <summary>
        /// Switch to chain
        /// </summary>
        /// <param name="chainId">Chain Id in HexBigInteger format</param>
        public async UniTask SwitchChain(HexBigInteger chainId)
        {
            var paramChain = new SwitchEthereumChainParameter()
            {
                ChainId = chainId
            };
            await Web3.Eth.HostWallet.SwitchEthereumChain.SendRequestAsync(paramChain);
        }

        /// <summary>
        /// Add chain to the wallet
        /// </summary>
        /// <param name="chainParameter">Chain paramater to add</param>
        public async UniTask AddChain(AddEthereumChainParameter chainParameter)
        {
            await Web3.Eth.HostWallet.AddEthereumChain.SendRequestAsync(chainParameter);
        }

        /// <summary>
        /// Add chain to the wallet
        /// </summary>
        /// <param name="chainParameter">Chain paramater to add</param>
        public async UniTask AddAndSwitchChain(AddEthereumChainParameter chainParameter)
        {
            try
            {
                await AddChain(chainParameter);
            }
            catch (Exception)
            {
                Debug.Log("Chain probably already Added");
            }
            finally
            {
                await SwitchChain(chainParameter.ChainId);
            }
        }


        private void Web3WC_UriGenerated(object sender, string e)
        {
            Debug.Log("uri received " + e);
            if (UriGenerated != null)
            {
                UriGenerated(this, e);
            }
        }

        private void Web3WC_Connected(object sender, string e)
        {
            Debug.Log("Web3Connect connected " + e);
            AccountAddress = e;
            Web3 = WalletConnectInstance.Web3Client;
            if (OnConnected != null)
            {
                OnConnected(this, e);
            }
            ChainId = WalletConnectInstance.Client.ChainId.ToHexBigInteger().ToString();

            WalletConnectInstance.Client.SessionUpdate += Client_SessionUpdate;
        }

        private void Client_SessionUpdate(object sender, WalletConnectSharp.Core.Models.WCSessionData e)
        {
            // handle chain or account update
            if (e?.chainId != null)
            {
                var newChain = e?.chainId.Value.ToHexBigInteger().ToString();

                if (newChain != ChainId)
                {
                    ChainId = newChain;
                    if (OnChainChanged != null)
                    {
                        OnChainChanged(this, newChain);
                    }
                }
            }
            if (e?.accounts?.Length > 0)
            {
                var newAccount = e.accounts[0];

                if (newAccount != AccountAddress)
                {
                    AccountAddress = newAccount;
                    if (OnAccountChanged != null)
                    {
                        OnAccountChanged(this, newAccount);
                    }
                }
            }

            if (e == null || e.accounts == null || e.accounts.Length == 0)
            {
                RemoveSubscription();
                ConnectionType = ConnectionType.None;
                Web3 = null;
                AccountAddress = string.Empty;
                ChainId = string.Empty;
                if (OnDisconnected != null)
                {
                    OnDisconnected(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Sign a text message
        /// </summary>
        /// <param name="message">The text message</param>
        /// <returns>The message signed</returns>
        public async UniTask<string> PersonalSign(string message)
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskInstance.Sign(message, MetamaskSignature.personal_sign);
            }
            else if (ConnectionType == ConnectionType.WalletConnect)
            {
                return await WalletConnectInstance.Client.EthPersonalSign(AccountAddress, message);
            }
            else
            {
                var signer1 = new EthereumMessageSigner();
                return signer1.EncodeUTF8AndSign(message, new EthECKey(PrivateKey));
            }
        }

        /// <summary>
        /// Sign datas with EIP712 implementation
        /// </summary>
        /// <param name="data">The datas to sign</param>
        /// <param name="domain">Eip712 domain infos</param>
        /// <returns>The message signed</returns>
        public async UniTask<string> PersonalSign<T>(T data, EIP712Domain domain)
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                var msg = JsonConvert.SerializeObject(new EvmTypedData<T>(data, domain));
                return await MetamaskInstance.Sign(msg, MetamaskSignature.signTypedData_v4);
            }
            else if (ConnectionType == ConnectionType.WalletConnect)
            {
                return await WalletConnectInstance.Client.EthSignTypedData<T>(AccountAddress, data, domain);
            }
            else
            {
                var msg = JsonConvert.SerializeObject(new EvmTypedData<T>(data, domain));
                Eip712TypedDataSigner signer = new Eip712TypedDataSigner();
                return signer.SignTypedDataV4(msg, new EthECKey(PrivateKey));
            }
        }

        /// <summary>
        /// Remove connections infos, Metamask not supported yet
        /// </summary>
        public async void Disconnect()
        {
            RemoveSubscription();
            switch (ConnectionType)
            {
                case ConnectionType.WalletConnect:
                    await WalletConnectInstance?.Client?.Disconnect();
                    break;
                case ConnectionType.Metamask:
                    // no method for the moment
                    break;
            }
            ConnectionType = ConnectionType.None;
            Web3 = null;
            AccountAddress = string.Empty;
            ChainId = string.Empty;
            if (OnDisconnected != null)
            {
                OnDisconnected(this, new EventArgs());
            }
        }


    }
}
