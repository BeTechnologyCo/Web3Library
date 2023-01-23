using System;
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
    public class ExternalAccountSignerTransactionManager : TransactionManagerBase
    {
        private readonly LegacyTransactionSigner _transactionSigner;

        public ExternalAccountSignerTransactionManager(IClient rpcClient, ExternalAccount account,
            BigInteger? chainId = null)
        {
            ChainId = chainId;
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Client = rpcClient;
            _transactionSigner = new LegacyTransactionSigner();
        }


        public override BigInteger DefaultGas { get; set; } = SignedLegacyTransaction.DEFAULT_GAS_LIMIT;


        public override UniTask<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            if (transactionInput == null) throw new ArgumentNullException(nameof(transactionInput));
            return SignAndSendTransactionAsync(transactionInput);
        }

        public override UniTask<string> SignTransactionAsync(TransactionInput transaction)
        {
            return SignTransactionRetrievingNextNonceAsync(transaction);
        }

        public async UniTask<string> SignTransactionExternallyAsync(TransactionInput transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (!transaction.From.IsTheSameAddress(Account.Address))
                throw new Exception("Invalid account used signing");

            SetDefaultGasPriceAndCostIfNotSet(transaction);

            var nonce = transaction.Nonce;
            if (nonce == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction nonce has not been set");

            var gasPrice = transaction.GasPrice;
            var gasLimit = transaction.Gas;

            var value = transaction.Value ?? new HexBigInteger(0);

            string signedTransaction;

            var externalSigner = ((ExternalAccount) Account).ExternalSigner;

            if (externalSigner.Supported1559 && transaction.Type != null &&
                transaction.Type.Value == TransactionType.EIP1559.AsByte())
            {
                var maxPriorityFeePerGas = transaction.MaxPriorityFeePerGas.Value;
                var maxFeePerGas = transaction.MaxFeePerGas.Value;
                if (ChainId == null) throw new ArgumentException("ChainId required for TransactionType 0X02 EIP1559");

                var transaction1559 = new Transaction1559(ChainId.Value, nonce, maxPriorityFeePerGas, maxFeePerGas,
                    gasLimit, transaction.To, value, transaction.Data,
                    transaction.AccessList.ToSignerAccessListItemArray());
                await transaction1559.SignExternallyAsync(externalSigner);
                signedTransaction = transaction1559.GetRLPEncoded().ToHex();
            }
            else
            {
                if (ChainId == null)
                    signedTransaction = await _transactionSigner.SignTransactionAsync(externalSigner,
                        transaction.To,
                        value.Value, nonce,
                        gasPrice.Value, gasLimit.Value, transaction.Data);
                else
                    signedTransaction = await _transactionSigner.SignTransactionAsync(externalSigner, ChainId.Value,
                        transaction.To,
                        value.Value, nonce,
                        gasPrice.Value, gasLimit.Value, transaction.Data);
            }

            return signedTransaction;
        }


        public string SignTransaction(TransactionInput transaction)
        {
            return SignTransactionRetrievingNextNonceAsync(transaction).AsTask().Result;
        }

        protected async UniTask<string> SignTransactionRetrievingNextNonceAsync(TransactionInput transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (!transaction.From.IsTheSameAddress(Account.Address))
                throw new Exception("Invalid account used signing");
            var nonce = await GetNonceAsync(transaction);
            transaction.Nonce = nonce;

            var externalSigner = ((ExternalAccount) Account).ExternalSigner;
            if (externalSigner.Supported1559)
            {
                await SetTransactionFeesOrPricingAsync(transaction);
            }
            else
            {
                var gasPrice = await GetGasPriceAsync(transaction);
                transaction.GasPrice = gasPrice;
            }

            return await SignTransactionExternallyAsync(transaction);
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