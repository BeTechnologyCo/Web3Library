using Org.BouncyCastle.Math;
using TokenContract;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;
using TokenDefinition = InfernalTower.Contracts.Token.ContractDefinition;

public class Sample : MonoBehaviour
{
    protected Button btnConnect;
    protected Button btnCall;
    protected Button btnSign;
    protected Label lblResult;
    protected VisualElement root;

    public GameObject cube;

    protected string tokenContract = "0x61A154Ef11d64309348CAA98FB75Bd82e58c9F89";

    //private void Awake()
    //{
    //    print(Application.absoluteURL);
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        Application.deepLinkActivated += onDeepLinkActivated;
    //        if (!string.IsNullOrEmpty(Application.absoluteURL))
    //        {
    //            // Cold start and Application.absoluteURL not null so process Deep Link.
    //            onDeepLinkActivated(Application.absoluteURL);
    //        }
    //        // Initialize DeepLink Manager global variable.
    //        else deeplinkURL = "[none]";
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}


    // Start is called before the first frame update
    void Start()
    {
        //print("url " + Application.absoluteURL);
        root = GetComponent<UIDocument>().rootVisualElement;
        btnConnect = root.Q<Button>("btnConnect");
        btnCall = root.Q<Button>("btnCall");
        btnSign = root.Q<Button>("btnSign");
        lblResult = root.Q<Label>("lblResult");

        btnConnect.clicked += BtnConnect_clicked;
        btnCall.clicked += BtnCall_clicked;
        btnSign.clicked += BtnSign_clicked;

        // Web3GL.OnAccountConnected += Web3GL_OnAccountConnected;


        Web3Unity.Web3Connect.Instance.ConnectMetamask();
    }

    private async void BtnSign_clicked()
    {
        //print("request sign");
        //var result = Web3Utils.SignMessage("hello toto", MetamaskSignature.personal_sign);
        //lblResult.text = result.Result;
        //print("sign ended " + result);
        print("request approve");
        TokenDefinition.ApproveFunction func = new TokenDefinition.ApproveFunction()
        {
            Amount = 10,
            Spender = "0x0b33fA091642107E3a63446947828AdaA188E276"
        };
        print("approve");
        var smartcontract = new Web3Contract(tokenContract);
        var result = await smartcontract.Send(func);
        lblResult.text = result;
        print("approve ended " + result);

    }

    private async void BtnApprove_clicked()
    {
        print("request approve");
        TokenDefinition.ApproveFunction func = new TokenDefinition.ApproveFunction()
        {
            Amount = 10,
            Spender = "0x0b33fA091642107E3a63446947828AdaA188E276"
        };
        print("approve");
        var smartcontract = new Web3Contract(tokenContract);
        var result = await smartcontract.Send(func);
        lblResult.text = result;
        print("approve ended " + result);
    }

    private void Web3GL_OnAccountConnected(object sender, string e)
    {
        print("account connected " + e);
        lblResult.text = e;
    }

    private async void BtnConnect_clicked()
    {
        //print("request connect");
        //await Web3Connect.Instance.MetamaskProvider.ConnectAccount();
        //// await Web3GL.ConnectAccount();
        //print("request ended");
        SceneManager.LoadScene("Web3Modal", LoadSceneMode.Additive);
    }


    private async void BtnCall_clicked()
    {
        TokenDefinition.BalanceOfFunction func = new TokenDefinition.BalanceOfFunction()
        {
            Account = "0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f"
        };
        print("get balance");
        var smartcontract = new TokenContractService(tokenContract);
        System.Numerics.BigInteger result = await smartcontract.BalanceOfQueryAsync("0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f");
        print("balance 0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f " + result);
        lblResult.text = "balance 0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f " + result;
        //var smartContract = new Web3Contract(tokenContract);
        //var res = await smartContract.Call<TokenDefinition.BalanceOfFunction, TokenDefinition.BalanceOfOutputDTO>(func);
        //print("balance 0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f " + res.ReturnValue1);
    }


    // Update is called once per frame
    void Update()
    {
        cube.transform.Rotate(new Vector3(10f, 10f, 10f) * Time.deltaTime);
    }
}
