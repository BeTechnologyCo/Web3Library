using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Web3Unity;
using ZXing;
using ZXing.QrCode;

public class Web3Modal : MonoBehaviour
{
    public RawImage image;

    // Texture for encoding 
    private Texture2D encoded;
    private string LastResult;
    private bool shouldEncodeNow;

    void Start()
    {
        encoded = new Texture2D(512, 512);

        //await Web3Connect.Instance.Web3WC.Connect("https://rpc.ankr.com/fantom_testnet");
        Web3Connect.Instance.Connected += Instance_Connected;

    }

    private void Instance_Connected(object sender, string e)
    {
        SceneManager.UnloadSceneAsync("Web3Modal");
    }

    public void Connect()
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

    public void Metamask()
    {
        Web3Connect.Instance.ConnectMetamask(true);
    }


    private void Web3WC_Connected(object sender, string e)
    {
        Debug.Log("connected " + e);
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
            image.texture = encoded;
            Debug.Log("image" + image.name);
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
