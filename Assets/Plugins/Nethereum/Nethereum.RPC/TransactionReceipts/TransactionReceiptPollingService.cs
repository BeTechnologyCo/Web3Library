using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.Exceptions;

namespace Nethereum.RPC.TransactionReceipts
{

#if !DOTNET35
    public class TransactionReceiptServiceFactory
    {
        public static ITransactionReceiptService GetDefaultransactionReceiptService(ITransactionManager transactionManager)
        {
            return new TransactionReceiptPollingService(transactionManager);
        }
    }

    public class TransactionReceiptPollingService : ITransactionReceiptService
    {
        private readonly ITransactionManager _transactionManager;

        private int _retryMilliseconds;
        private readonly object _lockingObject = new object();
        public int GetPollingRetryIntervalInMilliseconds()
        {
            lock (_lockingObject)
            {
                return _retryMilliseconds;
            }
        }

        public void SetPollingRetryIntervalInMilliseconds(int retryMilliseconds)
        {
            lock (_lockingObject)
            {
                _retryMilliseconds = retryMilliseconds;
            }
        }

        public TransactionReceiptPollingService(ITransactionManager transactionManager, int retryMilliseconds = 100)
        {
            _transactionManager = transactionManager;
            _retryMilliseconds = retryMilliseconds;
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(TransactionInput transactionInput,
           CancellationToken cancellationToken = default)
        {
            return SendRequestAndWaitForReceiptAsync(() => _transactionManager.SendTransactionAsync(transactionInput), cancellationToken);
        }

        public UniTask<List<TransactionReceipt>> SendRequestsAndWaitForReceiptAsync(IEnumerable<TransactionInput> transactionInputs,
           CancellationToken cancellationToken = default)
        {
            var funcs = new List<Func<UniTask<string>>>();
            foreach (var transactionInput in transactionInputs)
            {
                funcs.Add(() => _transactionManager.SendTransactionAsync(transactionInput));
            }
            return SendRequestsAndWaitForReceiptAsync(funcs.ToArray(), cancellationToken);
        }

        public async UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync(Func<UniTask<string>> transactionFunction,
           CancellationToken cancellationToken = default)
        {
            var transaction = await transactionFunction();
            return await PollForReceiptAsync(transaction, cancellationToken);
        }

        public async UniTask<TransactionReceipt> PollForReceiptAsync(string transaction, CancellationToken cancellationToken = default)
        {
            var getTransactionReceipt = new EthGetTransactionReceipt(_transactionManager.Client);
            var receipt = await getTransactionReceipt.SendRequestAsync(transaction);
            while (receipt == null)
            {
                if (cancellationToken != CancellationToken.None)
                {
                    await UniTask.Delay(GetPollingRetryIntervalInMilliseconds(), cancellationToken: cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                }
                else
                {
                    await UniTask.Delay(GetPollingRetryIntervalInMilliseconds());

                }

                receipt = await getTransactionReceipt.SendRequestAsync(transaction);
            }
            return receipt;
        }


        public async UniTask<List<TransactionReceipt>> SendRequestsAndWaitForReceiptAsync(IEnumerable<Func<UniTask<string>>> transactionFunctions,
            CancellationToken cancellationToken = default)
        {
            var txnList = new List<string>();
            foreach (var transactionFunction in transactionFunctions)
            {
                txnList.Add(await transactionFunction());
            }

            var receipts = new List<TransactionReceipt>();
            foreach (var transaction in txnList)
            {
                var receipt = await PollForReceiptAsync(transaction, cancellationToken);
                receipts.Add(receipt);
            }
            return receipts;
        }

        public async UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Func<UniTask<string>> deployFunction,
           CancellationToken cancellationToken = default)
        {
            var transactionReceipt = await SendRequestAndWaitForReceiptAsync(deployFunction, cancellationToken);
            if (transactionReceipt.Status.Value != 1)
            {
                var contractAddress = transactionReceipt.ContractAddress;
                var ethGetCode = new EthGetCode(_transactionManager.Client);
                var code = await ethGetCode.SendRequestAsync(contractAddress);
                if (code == "0x")
                    throw new ContractDeploymentException("Contract code not deployed successfully", transactionReceipt);
            }

            return transactionReceipt;
        }

        public async UniTask<string> DeployContractAndGetAddressAsync(Func<UniTask<string>> deployFunction,
           CancellationToken cancellationToken = default)
        {
            var transactionReceipt = await DeployContractAndWaitForReceiptAsync(deployFunction, cancellationToken);
            return transactionReceipt.ContractAddress;
        }

        public UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(TransactionInput transactionInput, CancellationToken cancellationToken = default)
        {
            return DeployContractAndWaitForReceiptAsync(() => _transactionManager.SendTransactionAsync(transactionInput), cancellationToken);
        }
    }
#endif
}
