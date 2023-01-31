using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;
using Nethereum.Util;
using System.Collections.Generic;
using System.Numerics;
using TokenContract;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;

public class Sample : MonoBehaviour
{
    protected Button btnConnect;
    protected Button btnWallet;
    protected Button btnCall;
    protected Button btnSign;
    protected Button btnSwitch;
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
    async void Start()
    {
        //print("url " + Application.absoluteURL);
        root = GetComponent<UIDocument>().rootVisualElement;
        btnWallet = root.Q<Button>("btnWallet");
        btnConnect = root.Q<Button>("btnConnect");
        btnCall = root.Q<Button>("btnCall");
        btnSign = root.Q<Button>("btnSign");
        btnSwitch = root.Q<Button>("btnSwitch");
        lblResult = root.Q<Label>("lblResult");

        btnConnect.clicked += BtnConnect_clicked;
        btnCall.clicked += BtnCall_clicked;
        btnSign.clicked += BtnSign_clicked;
        btnWallet.clicked += BtnWallet_clicked;
        btnSwitch.clicked += BtnSwitch_clicked;

        // Web3GL.OnAccountConnected += Web3GL_OnAccountConnected;

        //var transferEventHandler = Web3Connect.Instance.Web3.Eth.GetEvent<TransferEventDTO>(tokenContract);
        //var filter = transferEventHandler.CreateFilterInput<string,string>("toto","tata");
        //var filterId = await transferEventHandler.CreateFilterAsync(filter);
        //var result = await transferEventHandler.GetFilterChangesAsync(filterId);

        //Web3Connect.Instance.ConnectRPC();
        var eventSub = new EventSubscription<TransferEventDTO, string, string>("0xEf1c6E67703c7BD7107eed8303Fbe6EC2554BF6B", "0xEf1c6E67703c7BD7107eed8303Fbe6EC2554BF6B", "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2");
        eventSub.OnEventsReceived += EventSub_EventsReceived;
        Debug.Log("finish start");
    }

    private void EventSub_EventsReceived(object sender, List<TransferEventDTO> messages)
    {
        messages.ForEach(e =>
      {
          Debug.Log($"Transfer Weth From {e.From} To {e.To} Amount {UnitConversion.Convert.FromWei(e.Value)}");
      });
    }

    private void EventSub_EventReceived(object sender, TransferEventDTO e)
    {

    }

    private async void BtnSwitch_clicked()
    {
        print("switch 4002");
        var chainId = new BigInteger(4002);
        AddEthereumChainParameter data = new AddEthereumChainParameter()
        {
            ChainId = chainId.ToHexBigInteger(),
            BlockExplorerUrls = new List<string> { "https://testnet.ftmscan.com/" },
            ChainName = "Fantom testnet",
            IconUrls = new List<string> { "https://fantom.foundation/favicon.ico" },
            NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "Fantom", Symbol = "FTM" },
            RpcUrls = new List<string> { "https://rpc.testnet.fantom.network/" }

        };
        await Web3Connect.Instance.AddAndSwitchChain(data);
        print("switch end");
    }

    private void BtnWallet_clicked()
    {
        SceneManager.LoadScene("Web3Modal", LoadSceneMode.Additive);
    }

    private async void BtnSign_clicked()
    {
        try
        {
            //print("request sign");
            //var result = Web3Utils.SignMessage("hello toto", MetamaskSignature.personal_sign);
            //lblResult.text = result.Result;
            //print("sign ended " + result);


            await Web3Connect.Instance.PersonalSign("Hello Unity Dev");

            print("request approve");
            ApproveFunction func = new ApproveFunction()
            {
                Amount = 10,
                Spender = "0x0b33fA091642107E3a63446947828AdaA188E276",
                FromAddress = Web3Connect.Instance.AccountAddress
            };
            //print("approve");
            //var smartcontract = new Web3Contract(tokenContract);
            //var result = await smartcontract.Send(func);
            var smartcontract = new TokenContractService(tokenContract);
            //var result = await smartcontract.ApproveRequestAsync(func);
            //lblResult.text = result;
            //print("approve ended " + result);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            lblResult.text = e.Message;
        }

    }

    private async void BtnApprove_clicked()
    {
        print("request approve");
        ApproveFunction func = new ApproveFunction()
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

    private async void BtnTransfer_clicked()
    {
        var smartcontract = new TokenContractService("0x61A154Ef11d64309348CAA98FB75Bd82e58c9F89");

        string transactionHash = await smartcontract.TransferRequestAsync("0x0b33fA091642107E3a63446947828AdaA188E276", 1000);

        TransactionReceipt result = await smartcontract.TransferRequestAndWaitForReceiptAsync("0x0b33fA091642107E3a63446947828AdaA188E276", 1000);
        if (result.Succeeded())
        {
            TransferEventDTO transferEvent = result.GetEvent<TransferEventDTO>();
            List<TransferEventDTO> transferEventList = result.GetEventList<TransferEventDTO>();
        }


    }

    private void Web3GL_OnAccountConnected(object sender, string e)
    {
        print("account connected " + e);
        lblResult.text = e;
    }

    private async void BtnConnect_clicked()
    {
        print("request connect");
        if (Web3Connect.Instance.ConnectionType == ConnectionType.Metamask)
        {
            await Web3Connect.Instance.MetamaskInstance.ConnectAccount();
        }
        else
        {
            Web3Connect.Instance.ConnectWalletConnect("https://rpc.ankr.com/fantom_testnet");
        }

        // await Web3GL.ConnectAccount();
        print("request ended");
    }


    private async void BtnCall_clicked()
    {
        try
        {
            BalanceOfFunction func = new BalanceOfFunction()
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
            //lblResult.text = "balance 0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f " + res.ReturnValue1;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            lblResult.text = e.Message;
        }

    }


    // Update is called once per frame
    void Update()
    {
        cube.transform.Rotate(new UnityEngine.Vector3(10f, 10f, 10f) * Time.deltaTime);
    }
}
