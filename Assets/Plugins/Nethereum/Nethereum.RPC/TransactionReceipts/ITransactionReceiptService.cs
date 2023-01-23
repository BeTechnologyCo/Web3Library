using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.TransactionReceipts
{
    public interface ITransactionReceiptService
    {
        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(Func<UniTask<string>> transactionFunction,
            CancellationToken cancellationToken = default);

        UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Func<UniTask<string>> deployFunction,
             CancellationToken cancellationToken = default);

        UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(TransactionInput transactionInput,
            CancellationToken cancellationToken = default);

        UniTask<string> DeployContractAndGetAddressAsync(Func<UniTask<string>> deployFunction,
            CancellationToken cancellationToken = default);

        UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(TransactionInput transactionInput,
            CancellationToken cancellationToken = default);

        UniTask<List<TransactionReceipt>> SendRequestsAndWaitForReceiptAsync(IEnumerable<TransactionInput> transactionInputs,
            CancellationToken cancellationToken = default);

        UniTask<List<TransactionReceipt>> SendRequestsAndWaitForReceiptAsync(IEnumerable<Func<UniTask<string>>> transactionFunctions,
          CancellationToken cancellationToken = default);
        UniTask<TransactionReceipt> PollForReceiptAsync(string transaction, CancellationToken cancellationToken = default);

        int GetPollingRetryIntervalInMilliseconds();
        void SetPollingRetryIntervalInMilliseconds(int retryMilliseconds);
    }
}