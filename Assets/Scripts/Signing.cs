using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Util;
using System.Collections.Generic;
using System.Text;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.Encoders;
using System.Threading.Tasks;

public class Signing
{
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
}
