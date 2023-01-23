﻿using System;
using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.Util;

namespace Nethereum.Web3.Accounts
{
    public class AccountSignerTransactionManager : TransactionManagerBase
    {
        private readonly AccountOfflineTransactionSigner _transactionSigner;


        public AccountSignerTransactionManager(IClient rpcClient, Account account, BigInteger? overridingAccountChainId = null)
        {
            if (overridingAccountChainId == null)
            {
                ChainId = account.ChainId;
            }
            else
            {
                ChainId = overridingAccountChainId;
            }
            
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Client = rpcClient;
            _transactionSigner = new AccountOfflineTransactionSigner();
        }


        public AccountSignerTransactionManager(IClient rpcClient, string privateKey, BigInteger? chainId = null)
        {
            ChainId = chainId;
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            Client = rpcClient;
            Account = new Account(privateKey, chainId);
            Account.NonceService = new InMemoryNonceService(Account.Address, rpcClient);
            _transactionSigner = new AccountOfflineTransactionSigner();
        }

        public AccountSignerTransactionManager(string privateKey, BigInteger? chainId = null) : this(null, privateKey,
            chainId)
        {
        }

       

        public override BigInteger DefaultGas { get; set; } = SignedLegacyTransaction.DEFAULT_GAS_LIMIT;


        public async override UniTask<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            if (transactionInput == null) throw new ArgumentNullException(nameof(transactionInput));
            await EnsureChainIdAndChainFeatureIsSetAsync();
            return await SignAndSendTransactionAsync(transactionInput);
        }

        public async override UniTask<string> SignTransactionAsync(TransactionInput transaction)
        {
            await EnsureChainIdAndChainFeatureIsSetAsync();
            return await SignTransactionRetrievingNextNonceAsync(transaction);
        }

        public string SignTransaction(TransactionInput transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            SetDefaultGasIfNotSet(transaction);

            return _transactionSigner.SignTransaction((Account) Account, transaction, ChainId);
        }

        protected async UniTask<string> SignTransactionRetrievingNextNonceAsync(TransactionInput transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (!transaction.From.IsTheSameAddress(Account.Address))
                throw new Exception("Invalid account used signing");

            var nonce = await GetNonceAsync(transaction);
            transaction.Nonce = nonce;

            await SetTransactionFeesOrPricingAsync(transaction);

            return SignTransaction(transaction);
        }

        public async UniTask<HexBigInteger> GetNonceAsync(TransactionInput transaction)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            var nonce = transaction.Nonce;
            if (nonce == null)
            {
                if (Account.NonceService == null)
                    Account.NonceService = new InMemoryNonceService(Account.Address, Client);
                Account.NonceService.Client = Client;
                nonce = await Account.NonceService.GetNextNonceAsync();
            }

            return nonce;
        }

        private async UniTask<string> SignAndSendTransactionAsync(TransactionInput transaction)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (!transaction.From.IsTheSameAddress(Account.Address))
                throw new Exception("Invalid account used signing");

            var ethSendTransaction = new EthSendRawTransaction(Client);
            var signedTransaction = await SignTransactionRetrievingNextNonceAsync(transaction);
            return await ethSendTransaction.SendRequestAsync(signedTransaction.EnsureHexPrefix());
        }
    }
}