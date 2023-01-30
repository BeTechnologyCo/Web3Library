using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Events;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Network;
using WalletConnectSharp.Unity.Models;
using WalletConnectSharp.Unity.Network;

namespace Web3Unity
{
    public class WalletConnect : WalletConnectSession
    {

        public Dictionary<string, AppEntry> SupportedWallets
        {
            get;
            private set;
        }

        public AppEntry SelectedWallet { get; set; }

        private Wallets defaultWallet;

        public Wallets DefaultWallet
        {
            get
            {
                if (defaultWallet == null || defaultWallet == Wallets.None)
                {
                    defaultWallet = Wallets.MetaMask;
                }
                return defaultWallet;
            }
            set { defaultWallet = value; }
        }


        static WalletConnect()
        {
            TransportFactory.Instance.RegisterDefaultTransport((eventDelegator) => new NativeWebSocketTransport(eventDelegator));
        }

        public WalletConnect(ClientMeta clientMeta, string bridgeUrl = null, ITransport transport = null, ICipher cipher = null, int chainId = 1, EventDelegator eventDelegator = null) : base(clientMeta, bridgeUrl, transport, cipher, chainId, eventDelegator)
        {
            SetupDefaultWallet();
            SetupEvents();
        }

        public WalletConnect(SavedSession savedSession, ITransport transport = null, ICipher cipher = null,
            EventDelegator eventDelegator = null) : base(savedSession, transport, cipher, eventDelegator)
        {
            SetupDefaultWallet();
            SetupEvents();
        }

        private void SetupEvents()
        {
#if UNITY_EDITOR || DEBUG
            //Useful for debug logging
            OnSessionConnect += (sender, session) =>
            {
                Debug.Log("[WalletConnect] Session Connected");
            };
#endif

#if UNITY_EDITOR
            // prevent from open url on unity editor
#elif UNITY_ANDROID || UNITY_IOS
            //Whenever we send a request to the Wallet, we want to open the Wallet app
            OnSend += (sender, session) => OpenMobileWallet();
#endif
        }


        public void OpenMobileWallet(AppEntry selectedWallet)
        {
            SelectedWallet = selectedWallet;

            OpenMobileWallet();
        }

        public void OpenDeepLink(AppEntry selectedWallet)
        {
            SelectedWallet = selectedWallet;

            OpenDeepLink();
        }

        public void OpenMobileWallet()
        {
#if UNITY_ANDROID
            var signingURL = URI.Split('@')[0];

            Application.OpenURL(signingURL);
#elif UNITY_IOS
            if (SelectedWallet == null)
            {
                throw new NotImplementedException(
                    "You must use OpenMobileWallet(AppEntry) or set SelectedWallet on iOS!");
            }
            else
            {
                string url;
                string encodedConnect = WebUtility.UrlEncode(URI);
                if (!string.IsNullOrWhiteSpace(SelectedWallet.mobile.universal))
                {
                    url = SelectedWallet.mobile.universal + "/wc?uri=" + encodedConnect;
                }
                else
                {
                    url = SelectedWallet.mobile.native + (SelectedWallet.mobile.native.EndsWith(":") ? "//" : "/") +
                          "wc?uri=" + encodedConnect;
                }

                var signingUrl = url.Split('?')[0];

                Debug.Log("Opening: " + signingUrl);
                Application.OpenURL(signingUrl);
            }
#else
            Debug.Log("Platform does not support deep linking");
            return;
#endif
        }

        public void OpenDeepLink()
        {
            if (!ReadyForUserPrompt)
            {
                Debug.LogError("WalletConnectUnity.ActiveSession not ready for a user prompt" +
                               "\nWait for ActiveSession.ReadyForUserPrompt to be true");

                return;
            }

#if UNITY_ANDROID
            Debug.Log("[WalletConnect] Opening URL: " + URI);
            Application.OpenURL(URI);
#elif UNITY_IOS
            if (SelectedWallet == null)
            {
                throw new NotImplementedException(
                    "You must use OpenDeepLink(AppEntry) or set SelectedWallet on iOS!");
            }
            else
            {
                string url;
                string encodedConnect = WebUtility.UrlEncode(URI);
                if (!string.IsNullOrWhiteSpace(SelectedWallet.mobile.universal))
                {
                    url = SelectedWallet.mobile.universal + "/wc?uri=" + encodedConnect;
                }
                else
                {
                    url = SelectedWallet.mobile.native + (SelectedWallet.mobile.native.EndsWith(":") ? "//" : "/") +
                          "wc?uri=" + encodedConnect;
                }

                Debug.Log("Opening: " + url);
                Application.OpenURL(url);
            }
#else
            Debug.Log("Platform does not support deep linking");
            return;
#endif
        }

        private async void SetupDefaultWallet()
        {
            await FetchWalletList(false);

            var wallet = SupportedWallets.Values.FirstOrDefault(a => FormatWalletName(a.name) == DefaultWallet.ToString().ToLower());

            if (wallet != null)
            {
                SelectedWallet = wallet;
                await DownloadImagesFor(wallet.id);
                Debug.Log("Setup default wallet " + wallet.name);
            }
            else
            {
                SelectedWallet = SupportedWallets.Values.First();
                Debug.Log("Setup default wallet " + SelectedWallet.name);
            }
        }

        private string FormatWalletName(string name)
        {
            return name.Replace('.', ' ').Replace('|', ' ').Replace(")", "").Replace("(", "").Replace("'", "")
                .Replace(" ", "").Replace("1", "One").ToLower();
        }

        private async UniTask DownloadImagesFor(string id, string[] sizes = null)
        {
            if (sizes == null)
            {
                sizes = new string[] { "sm", "md", "lg" };
            }

            var data = SupportedWallets[id];

            foreach (var size in sizes)
            {
                var url = "https://registry.walletconnect.org/logo/" + size + "/" + id + ".jpeg";

                using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url))
                {
                    await imageRequest.SendWebRequest();

                    if (imageRequest.isNetworkError)
                    {
                        Debug.Log("Error Getting Wallet Icon: " + imageRequest.error);
                    }
                    else
                    {
                        var texture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                        var sprite = Sprite.Create(texture,
                            new Rect(0.0f, 0.0f, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f), 100.0f);

                        if (size == "sm")
                        {
                            data.smallIcon = sprite;
                        }
                        else if (size == "md")
                        {
                            data.medimumIcon = sprite;
                        }
                        else if (size == "lg")
                        {
                            data.largeIcon = sprite;
                        }
                    }
                }
            }
        }

        public async UniTask FetchWalletList(bool downloadImages = true)
        {
            Debug.Log("FetchWalletList");
            using (UnityWebRequest webRequest = UnityWebRequest.Get("https://registry.walletconnect.org/data/wallets.json"))
            {
                // Request and wait for the desired page.
                await webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log("Error Getting Wallet Info: " + webRequest.error);
                }
                else
                {
                    var json = webRequest.downloadHandler.text;

                    SupportedWallets = JsonConvert.DeserializeObject<Dictionary<string, AppEntry>>(json);

                    Debug.Log($"Wallets {SupportedWallets.Keys.Count}");
                    if (downloadImages)
                    {
                        foreach (var id in SupportedWallets.Keys)
                        {
                            await DownloadImagesFor(id);
                        }
                    }
                }
            }
        }

    }
}