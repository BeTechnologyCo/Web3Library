using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Fee1559Suggestions;
using Nethereum.Util;

namespace Nethereum.RPC.TransactionManagers
{
#if !DOTNET35
    public class EtherTransferService : IEtherTransferService
    {
        private readonly ITransactionManager _transactionManager;

        public EtherTransferService(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
        }

        public UniTask<string> TransferEtherAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null)
        {
            var fromAddress = _transactionManager?.Account?.Address;
            var transactionInput = EtherTransferTransactionInputBuilder.CreateTransactionInput(fromAddress, toAddress, etherAmount, gasPriceGwei, gas, nonce);
            return _transactionManager.SendTransactionAsync(transactionInput);
        }

        public UniTask<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null, CancellationToken cancellationToken = default)
        {
            var fromAddress = _transactionManager?.Account?.Address;
            var transactionInput = EtherTransferTransactionInputBuilder.CreateTransactionInput(fromAddress, toAddress, etherAmount, gasPriceGwei, gas, nonce);
            return _transactionManager.SendTransactionAndWaitForReceiptAsync(transactionInput, cancellationToken);
        }

        public async UniTask<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, decimal gasPriceGwei, BigInteger? gas = null)
        {
            var ethGetBalance = new EthGetBalance(_transactionManager.Client);
            var currentBalance = await ethGetBalance.SendRequestAsync(address);
            var gasPrice = UnitConversion.Convert.ToWei(gasPriceGwei, UnitConversion.EthUnit.Gwei);
            var gasAmount = gas ?? _transactionManager.DefaultGas;

            var totalAmount = currentBalance.Value - (gasAmount * gasPrice);
            if (totalAmount <= 0) throw new Exception("Insufficient balance to make a transfer");
            return UnitConversion.Convert.FromWei(totalAmount);
        }

    

        public UniTask<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFeePerGas, BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null, CancellationToken cancellationToken = default)
        {
            var fromAddress = _transactionManager?.Account?.Address;
            var transactionInput = EtherTransferTransactionInputBuilder.CreateTransactionInput(fromAddress, toAddress, etherAmount, maxPriorityFeePerGas, maxFeePerGas, gas, nonce);
            return _transactionManager.SendTransactionAndWaitForReceiptAsync(transactionInput, cancellationToken);
        }

        public UniTask<string> TransferEtherAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFeePerGas, BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null)
        {
            //Make the the maxPriorityFee and maxFeePerGas
            var fromAddress = _transactionManager?.Account?.Address;
            var transactionInput = EtherTransferTransactionInputBuilder.CreateTransactionInput(fromAddress, toAddress, etherAmount, maxPriorityFeePerGas, maxFeePerGas, gas, nonce);
            return _transactionManager.SendTransactionAsync(transactionInput);
        }

        public async UniTask<Fee1559> SuggestFeeToTransferWholeBalanceInEtherAsync(
            BigInteger? maxPriorityFeePerGas = null)
        {
            
            var fee1559 = await _transactionManager.Fee1559SuggestionStrategy.SuggestFeeAsync(maxPriorityFeePerGas);
            //Match it so there are not crumbs
            fee1559.MaxPriorityFeePerGas = fee1559.MaxFeePerGas;
            return fee1559;
        }

        public async UniTask<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, BigInteger maxFeePerGas, BigInteger? gas = null)
        {
            var ethGetBalance = new EthGetBalance(_transactionManager.Client);
            var currentBalance = await ethGetBalance.SendRequestAsync(address);

            var gasAmount = gas ?? _transactionManager.DefaultGas;

            var totalAmount = currentBalance.Value - (gasAmount * maxFeePerGas);
            if (totalAmount <= 0) throw new Exception("Insufficient balance to make a transfer");
            return UnitConversion.Convert.FromWei(totalAmount);
        }

        public async UniTask<BigInteger> EstimateGasAsync(string toAddress, decimal etherAmount)
        {
            var fromAddress = _transactionManager?.Account?.Address;
            var callInput = (CallInput)EtherTransferTransactionInputBuilder.CreateTransactionInput(fromAddress, toAddress, etherAmount);
            var hexEstimate = await _transactionManager.EstimateGasAsync(callInput);
            return hexEstimate.Value;
        }

        
    }
#endif
}