using Nethereum.Contracts.Standards.ERC20.TokenList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using TokenDefinition = InfernalTower.Contracts.Token.ContractDefinition;

public class Web3Utils : MonoBehaviour
{

    public event EventHandler<string> OnAccountConnected;
    public event EventHandler<BigInteger> OnChainChanged;
    public event EventHandler OnAccountDisconnected;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnEthereurmAccountConnected(string account)
    {
        if (OnAccountConnected != null)
        {
            OnAccountConnected(this, account);
        }
    }

    public void OnEthereurmAccountDisconnected()
    {
        if (OnAccountDisconnected != null)
        {
            OnAccountDisconnected(this, new EventArgs());
        }
    }

    public void OnEthereurmChainChanged(BigInteger chainId)
    {
        if (OnChainChanged != null)
        {
            OnChainChanged(this, chainId);
        }
    }
}
