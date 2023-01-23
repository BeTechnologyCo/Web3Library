using System;
using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.Util;

namespace Nethereum.Web3.Accounts.Basic
{
    public class BasicAccountTransactionManager : TransactionManagerBase
    {
        public BasicAccountTransactionManager(IClient client, BasicAccount account)
        {
            Account = account;
            Client = client;
        }

        public BasicAccountTransactionManager(IClient client, string accountAddress)
        {
            Account = new BasicAccount(accountAddress, this);
            Client = client;
        }

        public BasicAccountTransactionManager(string accountAddress) : this(null, accountAddress
            )
        {
        }

        public override BigInteger DefaultGas { get; set; } = SignedLegacyTransaction.DEFAULT_GAS_LIMIT;

        public void SetAccount(BasicAccount account)
        {
            Account = account;
        }

        public async UniTask<HexBigInteger> GetNonceAsync(TransactionInput transaction)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            var nonce = transaction.Nonce;
            if (nonce == null)
                if (Account.NonceService != null)
                {
                    Account.NonceService.Client = Client;
                    nonce = await Account.NonceService.GetNextNonceAsync();
                }

            return nonce;
        }


        public override async UniTask<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (transactionInput == null) throw new ArgumentNullException(nameof(transactionInput));
            if (!transactionInput.From.IsTheSameAddress(Account.Address)) throw new Exception("Invalid account used");

            await SetTransactionFeesOrPricingAsync(transactionInput);
            SetDefaultGasIfNotSet(transactionInput);

            var nonce = await GetNonceAsync(transactionInput);
            if (nonce != null) transactionInput.Nonce = nonce;
            var ethSendTransaction = new EthSendTransaction(Client);
            return await ethSendTransaction.SendRequestAsync(transactionInput)
                ;
        }

        public override UniTask<string> SendTransactionAsync(string from, string to, HexBigInteger amount)
        {
            if (!from.IsTheSameAddress(Account.Address)) throw new Exception("Invalid account used");
            var transactionInput = new TransactionInput(null, to, from, null, null, amount);
            return SendTransactionAsync(transactionInput);
        }

        public override UniTask<string> SignTransactionAsync(TransactionInput transaction)
        {
            throw new InvalidOperationException("Basic accounts cannot sign offline transactions");
        }
    }
}