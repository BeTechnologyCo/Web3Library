using System;
using System.Numerics;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Web3.Accounts
{
    public class AccountTransactionSigningInterceptor : RequestInterceptor
    {
        private readonly AccountSignerTransactionManager _signer;

        public AccountTransactionSigningInterceptor(string privateKey, BigInteger chainId, IClient client)
        {
            _signer = new AccountSignerTransactionManager(client, privateKey, chainId);
        }

        public override async UniTask<object> InterceptSendRequestAsync<TResponse>(
            Func<RpcRequest, string, UniTask<TResponse>> interceptedSendRequestAsync, RpcRequest request,
            string route = null)
        {
            if (request.Method == "eth_sendTransaction")
            {
                var transaction = (TransactionInput)request.RawParameters[0];
                return await SignAndSendTransactionAsync(transaction);
            }

            return await base.InterceptSendRequestAsync(interceptedSendRequestAsync, request, route);
        }

        public override async UniTask<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], UniTask<T>> interceptedSendRequestAsync, string method,
            string route = null, params object[] paramList)
        {
            if (method == "eth_sendTransaction")
            {
                var transaction = (TransactionInput)paramList[0];
                return await SignAndSendTransactionAsync(transaction);
            }

            return await base.InterceptSendRequestAsync(interceptedSendRequestAsync, method, route, paramList);
        }

        private UniTask<string> SignAndSendTransactionAsync(TransactionInput transaction)
        {
            return _signer.SendTransactionAsync(transaction);
        }
    }
}