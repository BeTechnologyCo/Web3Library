﻿using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using UnityEngine;

namespace Web3Unity
{
    public class Web3Connect
    {
        public string RpcUrl { get; private set; }

        public string PrivateKey { get; private set; }

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
        public event EventHandler<string> Connected;

        public string AccountAddress { get; private set; }

        public event EventHandler<string> UriGenerated;

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
            MetamaskInstance = new MetamaskProvider(autoConnect);
            MetamaskProvider.OnAccountConnected += MetamaskProvider_OnAccountConnected;
            Web3 = new Web3(MetamaskInstance);
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
        /// <param name="chainId">Chain id desired default 1 "ethereum"</param>
        /// <param name="name">Name of the dapp who appears in the popin in the wallet</param>
        /// <param name="description">Description of the dapp</param>
        /// <param name="icon">Icon show on the popin</param>
        /// <param name="url">Url to the project</param>
        /// <returns>The uri to connect to wallet connect</returns>
        public async UniTask<string> ConnectWalletConnect(string rpcUrl = "https://rpc.builder0x69.io", int chainId = 1, string name = "Test Unity", string description = "Test dapp", string icon = "https://unity.com/favicon.ico", string url = "https://unity.com/")
        {
            ConnectionType = ConnectionType.WalletConnect;
            RpcUrl = rpcUrl;
            WalletConnectInstance = new WalletConnectProvider(rpcUrl, chainId, name, description, icon, url);
            WalletConnectInstance.UriGenerated += Web3WC_UriGenerated;
            WalletConnectInstance.Connected += Web3WC_Connected;
            await WalletConnectInstance.Connect();

            Web3 = WalletConnectInstance.Web3Client;
            return WalletConnectInstance.Uri;
        }

        public async UniTask<string> SwitchChain(int chainId)
        {
            //if (ConnectionType == ConnectionType.WalletConnect)
            //{

            //    return await WalletConnectInstance.SwitchChain();
            //}
            //else if(ConnectionType == ConnectionType.Metamask)
            //{


            //}
            var paramChain = new SwitchEthereumChainParameter()
            {
                ChainId = new HexBigInteger(chainId.ToString("X"))
            };
            return await Web3.Eth.HostWallet.SwitchEthereumChain.SendRequestAsync(paramChain);
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
            if (Connected != null)
            {
                Connected(this, e);
            }
        }

        public async UniTask<string> Sign(string message, MetamaskSignature sign = MetamaskSignature.personal_sign)
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskInstance.Sign(message, sign);
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
