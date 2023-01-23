using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Fee1559Suggestions;

namespace Nethereum.RPC.TransactionManagers
{
    public interface IEtherTransferService
    {
        UniTask<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null, CancellationToken cancellationToken = default);
        UniTask<string> TransferEtherAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null);
        UniTask<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, decimal gasPriceGwei, BigInteger? gas = null);
        UniTask<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address,
            BigInteger maxFeePerGas, BigInteger? gas = null);
        UniTask<string> TransferEtherAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee,
            BigInteger maxFeePerGas, BigInteger? gas = null,
            BigInteger? nonce = null);

        UniTask<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee,
            BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null,
            CancellationToken cancellationToken = default);
        UniTask<BigInteger> EstimateGasAsync(string toAddress, decimal etherAmount);

        UniTask<Fee1559> SuggestFeeToTransferWholeBalanceInEtherAsync(
            BigInteger? maxPriorityFeePerGas = null);
    }
}