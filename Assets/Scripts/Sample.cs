using Nethereum.Contracts.Standards.ERC20.TokenList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TokenDefinition = InfernalTower.Contracts.Token.ContractDefinition;

public class Sample : MonoBehaviour
{
    protected Button btnConnect;
    protected Button btnCall;
    protected VisualElement root;

    protected string tokenContract = "0x957fa05F4AeCf1e235a101a22Db16b66928b8bf2";

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        btnConnect = root.Q<Button>("btnConnect");
        btnCall = root.Q<Button>("btnCall");

        btnConnect.clicked += BtnConnect_clicked;
        btnCall.clicked += BtnCall_clicked;
    }

    private async void BtnConnect_clicked()
    {
        print("request connect");
        await Web3GL.ConnectAccount();
        print("request ended");
    }


    private async void BtnCall_clicked()
    {
        TokenDefinition.BalanceOfFunction func = new TokenDefinition.BalanceOfFunction()
        {
            Account = "0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f"
        };
        var result =  Web3GL.Call<TokenDefinition.BalanceOfFunction, TokenDefinition.BalanceOfOutputDTO>(func, tokenContract);
        print("balance 0xDBf0DC3b7921E9Ef897031db1DAe239B4E45Af5f " + result.ReturnValue1);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
