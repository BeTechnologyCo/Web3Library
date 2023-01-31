using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.NEthereum;

namespace Web3Unity
{
    /// <summary>
    /// Wallet connect provider
    /// </summary>
    public class WalletConnectProvider
    {
        private static int id = 0;

        public string Uri { get; private set; }

        public WalletConnect Client { get; private set; }

        public int ChainId { get; private set; }
        public string RpcUrl { get; private set; }

        public Web3 Web3Client { get; private set; }

        public event EventHandler<string> OnAccountConnected;

        private static Dictionary<int, UniTaskCompletionSource<string>> utcs = new Dictionary<int, UniTaskCompletionSource<string>>();
        private static UniTaskCompletionSource<string> utcsConnected;


        public event EventHandler<string> OnUriGenerated;

        public static void RequestCallResult(int key, string val)
        {
            if (utcs.ContainsKey(key))
            {
                utcs[key].TrySetResult(val);
                Debug.Log($"Key found Web3GL {key}");
            }
            else
            {
                Debug.LogWarning($"Key not found Web3GL {key}");
            }
        }


        public static async UniTask<RpcResponseMessage> RequestCallAsync(int val, RpcRequestMessage request, bool sign = false)
        {
            utcs[val] = new UniTaskCompletionSource<string>();
            var datas = request.RawParameters as object[];
            if (datas?.Length > 0)
            {
                if (!sign)
                {
                    var callParam = datas[0] as TransactionInput;
                    // Application.OpenURL($"{server}?id={request.Id}&method={request.Method}&data={callParam.Data}&deepLink={deepLink}&to={callParam.To}&value={callParam.Value}");
                }
                else
                {
                    var callParam = datas[0].ToString();
                    // Application.OpenURL($"{server}?id={request.Id}&method={request.Method}&data={callParam}&deepLink={deepLink}");
                }
            }

            //while (utcs[val].Task.Status == UniTaskStatus.Pending)
            //{
            //    await Task.Delay(100);
            //}

            string result = await utcs[val].Task;
            return JsonConvert.DeserializeObject<RpcResponseMessage>(result);
        }

        public ClientMeta Metadata { get; private set; }
        public WalletConnectProvider(string rpcUrl, int chainId, string name, string description, string icon, string url)
        {
            ChainId = chainId;
            RpcUrl = rpcUrl;
            Metadata = new ClientMeta()
            {
                Description = description,
                Icons = new[] { icon },
                Name = name,
                URL = url
            };


        }

        private void Client_OnSessionConnect(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {

            Debug.Log($"session connect");
        }

        private void Client_OnSend(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {
            Debug.Log($"Client_OnSend " + e.Accounts?.Length);
        }

        private void Client_OnTransportConnect(object sender, WalletConnectSharp.Core.WalletConnectProtocol e)
        {
            Debug.Log($"Transport");
        }

        private void Client_OnSessionCreated(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {
            Debug.Log($"session");
        }

        public async UniTask Connect()
        {
            Client = new WalletConnect(clientMeta: Metadata);
            //var nethereum = new Web3(walletConnect.CreateProvider(new Uri("https//rpc.testnet.fantom.network/")));
            Client.OnSessionCreated += Client_OnSessionCreated;
            Client.OnTransportConnect += Client_OnTransportConnect;
            Client.OnSend += Client_OnSend;
            Client.OnSessionConnect += Client_OnSessionConnect;
            Client.SessionUpdate += Client_SessionUpdate;
            Client.OnReadyToConnect += Client_OnReadyToConnect;
            Uri = Client.URI;

            if (OnUriGenerated != null)
            {
                OnUriGenerated(this, Uri);
            }
            Debug.Log("connect");
            await Client.Connect();
            if (Client.Accounts?.Length > 0)
            {
                Debug.Log($"Address: {Client.Accounts[0]}");
                Debug.Log($"Chain ID: {Client.ChainId}");
            }
            else
            {
                Debug.LogWarning("No account detected");
            }

            Web3Client = Client.BuildWeb3(new Uri(RpcUrl)).AsWalletAccount(true);
            if (OnAccountConnected != null)
            {
                OnAccountConnected(this, Client.Accounts[0]);
            }

        }

        private void Client_OnReadyToConnect(object sender, WalletConnectSharp.Core.WalletConnectSession e)
        {
#if UNITY_EDITOR
            // prevent from open url on unity editor
            Debug.Log("[WalletConnectProvider] Ready to connect");
#elif UNITY_ANDROID || UNITY_IOS
           Client.OpenDeepLink();
#endif
        }

        private void Client_SessionUpdate(object sender, WCSessionData e)
        {
            Debug.Log($"sessionupdate");
        }

        public async UniTask<string> SwitchChain()
        {
            if (ChainId != Client.ChainId)
            {
                EthChain data = new EthChain()
                {
                    chainId = "0x" + ChainId.ToString("X"),
                    //blockExplorerUrls = new[] { "https://testnet.ftmscan.com/" },
                    //chainName = "Fantom testnet",
                    //iconUrls = new[] { "https://fantom.foundation/favicon.ico" },
                    //nativeCurrency = new NativeCurrency() { decimals = 18, name = "Fantom", symbol = "FTM" },
                    //rpcUrls = new[] { "https://rpc.testnet.fantom.network/" }

                };
                return await Client.WalletSwitchEthChain(data).AsUniTask();
            }
            return string.Empty;
        }

    }

}