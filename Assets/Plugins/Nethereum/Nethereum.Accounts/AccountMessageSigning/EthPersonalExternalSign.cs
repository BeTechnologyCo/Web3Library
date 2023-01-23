﻿using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.AccountSigning;
using Nethereum.Signer;
using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.Accounts.AccountMessageSigning
{
    public class EthPersonalExternalSign : IEthPersonalSign
    {
        private readonly IEthExternalSigner _ethExternalSigner;

        public EthPersonalExternalSign(IEthExternalSigner ethExternalSigner)
        {
            _ethExternalSigner = ethExternalSigner;
        }

        public async UniTask<string> SendRequestAsync(byte[] value, object id = null)
        {
            var result = await _ethExternalSigner.SignEthereumMessageAsync(value);
            return EthECDSASignature.CreateStringSignature(result);
        }

        public UniTask<string> SendRequestAsync(HexUTF8String utf8Hex, object id = null)
        {
            return SendRequestAsync(utf8Hex.HexValue.HexToByteArray());
        }

        public RpcRequest BuildRequest(HexUTF8String utf8Hex, object id = null)
        {
            throw new NotImplementedException();
        }

        public RpcRequest BuildRequest(byte[] value, object id = null)
        {
            throw new NotImplementedException();
        }
    }
}
