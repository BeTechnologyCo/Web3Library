using System;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.ContractHandlers
{
    public interface IContractTransactionHandler<TContractMessage> where TContractMessage : FunctionMessage, new()
    {
        UniTask<TransactionInput> CreateTransactionInputEstimatingGasAsync(string contractAddress, TContractMessage functionMessage = null);
        UniTask<HexBigInteger> EstimateGasAsync(string contractAddress, TContractMessage functionMessage = null);     
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractAddress, TContractMessage functionMessage = null, CancellationTokenSource tokenSource = null);
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(string contractAddress, TContractMessage functionMessage, CancellationToken cancellationToken);
        UniTask<string> SendRequestAsync(string contractAddress, TContractMessage functionMessage = null);
        UniTask<string> SignTransactionAsync(string contractAddress, TContractMessage functionMessage = null);
    }
}