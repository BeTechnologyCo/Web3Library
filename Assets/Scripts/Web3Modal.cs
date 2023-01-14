using QRCoder;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using WalletConnectSharp.Unity;
using WalletConnectSharp.Unity.Network;
using Web3Unity;
using ZXing;
using ZXing.QrCode;

public class Web3Modal : MonoBehaviour
{
    //public RawImage image;

    // Texture for encoding 
    private Texture2D encoded;
    private string LastResult;
    private bool shouldEncodeNow;

    protected VisualElement root;
    protected VisualElement imgQrCode;
    protected VisualElement veMetamask;
    protected Button btnWC;
    protected Button btnMetamask;
    protected Button btnClose;

    void Start()
    {
        encoded = new Texture2D(512, 512);

        root = GetComponent<UIDocument>().rootVisualElement;
        btnWC = root.Q<Button>("btnWeb3ModalWC");
        btnMetamask = root.Q<Button>("btnWeb3ModalMetamask");
        btnClose = root.Q<Button>("btnWeb3ModalClose");
        imgQrCode = root.Q<VisualElement>("imgQrCode");
        veMetamask = root.Q<VisualElement>("veMetamask");

        btnMetamask.clicked += BtnMetamask_clicked;
        btnWC.clicked += BtnWC_clicked;
        btnClose.clicked += BtnClose_clicked;

        btnWC.text = "Regenerate QR code";
        veMetamask.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        imgQrCode.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);

#if UNITY_EDITOR
        btnWC.text = "Regenerate QR code";
#elif UNITY_IOS || UNITY_ANDROID
        btnWC.text = "Open wallet";
        imgQrCode.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
#elif UNITY_WEBGL
        veMetamask.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
#endif

        //await Web3Connect.Instance.Web3WC.Connect("https://rpc.ankr.com/fantom_testnet");
        Web3Connect.Instance.Connected += Instance_Connected;

        GetUri();

    }

    private void Session_OnSessionConnect(object sender, WalletConnectSharp.Core.WalletConnectSession e)
    {
        //  Web3Connect.Instance.ConnectWalletConnect("https://rpc.ankr.com/fantom_testnet");
        Debug.Log("connected " + e.Accounts[0]);
    }

    private void BtnClose_clicked()
    {
        SceneManager.UnloadSceneAsync("Web3Modal");
    }
    private async Task GetUri()
    {
        var uri = Web3Connect.Instance.ConnectWalletConnect("https://rpc.ankr.com/fantom_testnet");
        Debug.Log("uri " + uri);
#if UNITY_EDITOR
        LastResult = uri;
        shouldEncodeNow = true;
#elif UNITY_IOS || UNITY_ANDROID
        Application.OpenURL(uri);
#else
        LastResult = uri;
        shouldEncodeNow = true;
#endif
    }

    private async void BtnWC_clicked()
    {
        await Web3Connect.Instance.Web3WC.Client.Disconnect();
        await GetUri();
    }

    private void BtnMetamask_clicked()
    {
        Web3Connect.Instance.ConnectMetamask(true);
    }

    private void Instance_Connected(object sender, string e)
    {

        Debug.Log("connected account " + e);
        SceneManager.UnloadSceneAsync("Web3Modal");
    }

    void Update()
    {

        // encode the last found
        var textForEncoding = LastResult;
        if (shouldEncodeNow &&
            textForEncoding != null)
        {
            Debug.Log("set image");
            var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            shouldEncodeNow = false;
            imgQrCode.style.backgroundImage = new StyleBackground(encoded);

        }
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
