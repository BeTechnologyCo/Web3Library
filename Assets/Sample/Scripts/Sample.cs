using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using TokenContract;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;

public class Sample : MonoBehaviour
{
    protected Button btnMint;
    protected Button btnWallet;
    protected Button btnCall;
    protected Button btnSign;
    protected Button btnSwitch;
    protected Button btnDeposit;
    protected Button btnWithdraw;
    protected Label lblResult;
    protected Label lblAccount;
    protected Label lblChain;
    protected VisualElement root;
    protected VisualElement veActions;


    private TokenContractService tokenService;
    public const string tokenContractAddress = "0x685576c3a592088eA9CA528b342D05087a64b6E7";

    public GameObject cube;

    private bool connected = false;
    private bool getbalance = false;

    // Start is called before the first frame update
    async void Start()
    {
        //print("url " + Application.absoluteURL);
        root = GetComponent<UIDocument>().rootVisualElement;
        btnWallet = root.Q<Button>("btnWallet");
        btnMint = root.Q<Button>("btnMint");
        btnCall = root.Q<Button>("btnCall");
        btnSign = root.Q<Button>("btnSign");
        btnSwitch = root.Q<Button>("btnSwitch");
        btnDeposit = root.Q<Button>("btnDeposit");
        btnWithdraw = root.Q<Button>("btnWithdraw");
        lblResult = root.Q<Label>("lblResult");
        lblAccount = root.Q<Label>("lblAccount");
        lblChain = root.Q<Label>("lblChain");
        veActions = root.Q<VisualElement>("veActions");

        btnMint.clicked += BtnMint_clicked;
        btnCall.clicked += BtnCall_clicked;
        btnSign.clicked += BtnSign_clicked;
        btnWallet.clicked += BtnWallet_clicked;
        btnSwitch.clicked += BtnSwitch_clicked;
        btnDeposit.clicked += BtnDeposit_clicked;
        btnWithdraw.clicked += BtnWithdraw_clicked;

        veActions.style.display = DisplayStyle.None;

        Web3Connect.Instance.OnConnected += Instance_OnConnected;
        Web3Connect.Instance.OnDisconnected += Instance_OnDisconnected;
        Web3Connect.Instance.OnChainChanged += Instance_OnChainChanged;
        Web3Connect.Instance.OnAccountChanged += Instance_OnAccountChanged;
    }

    private void Instance_OnAccountChanged(object sender, string e)
    {
        GetBalance();
    }

    private void Instance_OnChainChanged(object sender, string e)
    {
        GetBalance();
    }

    private void Instance_OnDisconnected(object sender, EventArgs e)
    {
        Disconnect();
    }

    private void Instance_OnConnected(object sender, string e)
    {
        Initialize();
    }

    private async void BtnDeposit_clicked()
    {
        // exemple call payable function
        DepositFunction depositFunction = new DepositFunction()
        {
            AmountToSend = UnitConversion.Convert.ToWei(0.01m)
        };
        var deposit = await tokenService.DepositRequestAndWaitForReceiptAsync(depositFunction);
        if (deposit.Succeeded())
        {
            lblResult.text = $"Matic depose on contract";
        }
    }

    private async void BtnWithdraw_clicked()
    {
        WithdrawFunction function = new WithdrawFunction()
        {
            // bug resolved
            // Gas = 100000
        };
        // withdraw matic depose on contract
        var withdraw = await tokenService.WithdrawRequestAsync(function);
        lblResult.text = $"Withdraw tx hash {withdraw}";
    }

    private async void GetBalance()
    {
        if (Web3Connect.Instance.Web3 != null)
        {
            // request user balance, we can use classic nethereum function
            var balance = await Web3Connect.Instance.Web3.Eth.GetBalance.SendRequestAsync(Web3Connect.Instance.AccountAddress);
            var amount = UnitConversion.Convert.FromWei(balance.Value);
            lblAccount.text = $"{Web3Connect.Instance.AccountAddress} {amount.ToString("F3")} matic";
            lblChain.text = $"Chain id {Web3Connect.Instance.ChainId}";
        }
    }

    private async void BtnMint_clicked()
    {
        // exemple mint 10 000 token
        var decimals = await tokenService.DecimalsQueryAsync();
        var symbol = await tokenService.SymbolQueryAsync();
        var amount = UnitConversion.Convert.ToWei(10000, decimals);
        lblResult.text = $"Minting {amount} wei {symbol}";
        var mint = await tokenService.MintRequestAndWaitForReceiptAsync(Web3Connect.Instance.AccountAddress, amount);
        if (mint.Succeeded())
        {
            var transferEvent = mint.GetEvent<TransferEventDTO>();
            lblResult.text = $"{transferEvent.Value} wei Token minted to {transferEvent.To}";
        }
    }

    private async void BtnSwitch_clicked()
    {
        // exemple of call add and switch
        print("switch 80001 Polygon mumbai Testnet");
        var chainId = new BigInteger(80001);
        AddEthereumChainParameter data = new AddEthereumChainParameter()
        {
            ChainId = chainId.ToHexBigInteger(),
            BlockExplorerUrls = new List<string> { "https://mumbai.polygonscan.com/" },
            ChainName = "Polygon Mumbai Testnet",
            IconUrls = new List<string> { "https://polygon.technology/favicon.ico" },
            NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "Matic", Symbol = "MATIC" },
            RpcUrls = new List<string> { "https://rpc-mumbai.maticvigil.com/" }

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
            // sign text
            var sign = await Web3Connect.Instance.PersonalSign("Hello Unity Dev");
            lblResult.text = sign;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            lblResult.text = e.Message;
        }

    }

    private async void BtnApprove_clicked()
    {
        // old exemple with web3 contract use
        print("request approve");
        ApproveFunction func = new ApproveFunction()
        {
            Amount = 10,
            Spender = "0x0b33fA091642107E3a63446947828AdaA188E276"
        };
        print("approve");
        var smartcontract = new Web3Contract(tokenContractAddress);
        var result = await smartcontract.Send(func);
        lblResult.text = result;
        print("approve ended " + result);
    }


    private async void BtnCall_clicked()
    {
        try
        {
            var decimals = await tokenService.DecimalsQueryAsync();
            var symbol = await tokenService.SymbolQueryAsync();
            // emple of call balance
            BigInteger result = await tokenService.BalanceOfQueryAsync(Web3Connect.Instance.AccountAddress);
            var amount = UnitConversion.Convert.FromWei(result, decimals);
            lblResult.text = $"My balance {amount} {symbol}";
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
        cube.transform.Rotate(new UnityEngine.Vector3(0, 20f, 0) * Time.deltaTime);
        if (!connected)
        {
            if (Web3Connect.Instance.Connected)
            {
                Initialize();
            }
        }
    }

    private void Initialize()
    {
        connected = true;
        lblAccount.text = Web3Connect.Instance.AccountAddress;
        veActions.style.display = DisplayStyle.Flex;
        tokenService = new TokenContractService(tokenContractAddress);
        if (!getbalance)
        {
            // prevent from multiple invoke repeating
            getbalance = true;
            InvokeRepeating("GetBalance", 0, 5);
        }
    }

    private void Disconnect()
    {
        connected = false;
        lblAccount.text = "Not Connected";
        lblChain.text = "";
        veActions.style.display = DisplayStyle.None;
    }
}
