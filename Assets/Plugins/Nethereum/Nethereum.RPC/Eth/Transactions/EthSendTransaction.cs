using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
 
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public class EthSendTransaction : RpcRequestResponseHandler<string>, IEthSendTransaction
    {
        public EthSendTransaction(IClient client) : base(client, ApiMethods.eth_sendTransaction.ToString())
        {
        }

        public UniTask<string> SendRequestAsync(TransactionInput input, object id = null)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return base.SendRequestAsync(id, input);
        }

        public RpcRequest BuildRequest(TransactionInput input, object id = null)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return base.BuildRequest(id, input);
        }
    }
}