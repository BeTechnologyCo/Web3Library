using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.Signer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using TokenDefinition = InfernalTower.Contracts.Token.ContractDefinition;

public class Web3Utils : IDisposable
{

    public event EventHandler<string> OnAccountConnected;
    public event EventHandler<BigInteger> OnChainChanged;
    public event EventHandler OnAccountDisconnected;

    // Start is called before the first frame update
    public Web3Utils()
    {
        if (IsWebGL())
        {

            Web3GL.OnAccountConnected += OnAccountConnected;
            Web3GL.OnAccountDisconnected += OnAccountDisconnected;
            Web3GL.OnChainChanged += OnChainChanged;
        }
    }

    public void Dispose()
    {
        if (IsWebGL())
        {
            Web3GL.OnAccountConnected -= OnAccountConnected;
            Web3GL.OnAccountDisconnected -= OnAccountDisconnected;
            Web3GL.OnChainChanged -= OnChainChanged;
        }
    }

    public static string SignMessage(string message, string privateKey)
    {
        var signer1 = new EthereumMessageSigner();
        return signer1.EncodeUTF8AndSign(message, new EthECKey(privateKey));
    }

    public static string GetAddress(string message, string signature)
    {
        var signer1 = new EthereumMessageSigner();
        return signer1.HashAndEcRecover(message, signature);
    }

    public bool IsWebGL()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_WEBGL
        return true;
#else
        return false;
#endif
    }
}
