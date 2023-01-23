using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps;
using Nethereum.BlockchainProcessing.Orchestrator;
using Nethereum.Contracts.Services;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using System.Linq;
using Nethereum.BlockchainProcessing.ProgressRepositories;

namespace Nethereum.BlockchainProcessing.BlockProcessing
{

    public class BlockCrawlOrchestrator: IBlockchainProcessingOrchestrator
    {
        public IEthApiContractService EthApi { get; set; }
        public IEnumerable<BlockProcessingSteps> ProcessingStepsCollection { get; }
        public BlockCrawlerStep BlockCrawlerStep { get; }
        public TransactionCrawlerStep TransactionWithBlockCrawlerStep { get; }
        public TransactionReceiptCrawlerStep TransactionWithReceiptCrawlerStep { get; }
        public ContractCreatedCrawlerStep ContractCreatedCrawlerStep { get; }

        public FilterLogCrawlerStep FilterLogCrawlerStep { get; }

        public BlockCrawlOrchestrator(IEthApiContractService ethApi, BlockProcessingSteps blockProcessingSteps)
            :this(ethApi, new[] { blockProcessingSteps })
        {

        }

        public BlockCrawlOrchestrator(IEthApiContractService ethApi, IEnumerable<BlockProcessingSteps> processingStepsCollection)
        {
            
            this.ProcessingStepsCollection = processingStepsCollection;
            EthApi = ethApi;
            BlockCrawlerStep = new BlockCrawlerStep(ethApi);
            TransactionWithBlockCrawlerStep = new TransactionCrawlerStep(ethApi);
            TransactionWithReceiptCrawlerStep = new TransactionReceiptCrawlerStep(ethApi);
            ContractCreatedCrawlerStep = new ContractCreatedCrawlerStep(ethApi);
            FilterLogCrawlerStep = new FilterLogCrawlerStep(ethApi);
        }

        public virtual async UniTask CrawlBlockAsync(BigInteger blockNumber)
        {
            var blockCrawlerStepCompleted = await BlockCrawlerStep.ExecuteStepAsync(blockNumber, ProcessingStepsCollection);
            await CrawlTransactionsAsync(blockCrawlerStepCompleted);

        }
        protected virtual async UniTask CrawlTransactionsAsync(CrawlerStepCompleted<BlockWithTransactions> completedStep)
        {
            if (completedStep != null)
            {
                foreach (var txn in completedStep.StepData.Transactions)
                {
                    await CrawlTransactionAsync(completedStep, txn);
                }
            }
        }
        protected virtual async UniTask CrawlTransactionAsync(CrawlerStepCompleted<BlockWithTransactions> completedStep, Transaction txn)
        {
            var currentStepCompleted = await TransactionWithBlockCrawlerStep.ExecuteStepAsync(
                new TransactionVO(txn, completedStep.StepData), completedStep.ExecutedStepsCollection);

            if(currentStepCompleted.ExecutedStepsCollection.Any() && TransactionWithReceiptCrawlerStep.Enabled)
            { 
                await CrawlTransactionReceiptAsync(currentStepCompleted);
            }
        }

        protected virtual async UniTask CrawlTransactionReceiptAsync(CrawlerStepCompleted<TransactionVO> completedStep)
        {
            if (TransactionWithReceiptCrawlerStep.Enabled)
            {
                var currentStepCompleted = await TransactionWithReceiptCrawlerStep.ExecuteStepAsync(
                    completedStep.StepData,
                    completedStep.ExecutedStepsCollection);
                if (currentStepCompleted != null && currentStepCompleted.StepData.IsForContractCreation() &&
                    ContractCreatedCrawlerStep.Enabled)
                {
                    await ContractCreatedCrawlerStep.ExecuteStepAsync(currentStepCompleted.StepData,
                        completedStep.ExecutedStepsCollection);
                }

                await CrawlFilterLogsAsync(currentStepCompleted);
            }
        }


        protected virtual async UniTask CrawlFilterLogsAsync(CrawlerStepCompleted<TransactionReceiptVO> completedStep)
        {
            if (completedStep != null && FilterLogCrawlerStep.Enabled)
            {
                foreach (var log in completedStep.StepData.TransactionReceipt.Logs.ConvertToFilterLog())
                {
                    await CrawlFilterLogAsync(completedStep, log);
                }
            }
        }

        protected virtual async UniTask CrawlFilterLogAsync(CrawlerStepCompleted<TransactionReceiptVO> completedStep, FilterLog filterLog)
        {
            if (FilterLogCrawlerStep.Enabled)
            {
                var currentStepCompleted = await FilterLogCrawlerStep.ExecuteStepAsync(
                    new FilterLogVO(completedStep.StepData.Transaction, completedStep.StepData.TransactionReceipt,
                        filterLog), completedStep.ExecutedStepsCollection);
            }
        }

        public async UniTask<OrchestrationProgress> ProcessAsync(BigInteger fromNumber, BigInteger toNumber, CancellationToken cancellationToken = default(CancellationToken), IBlockProgressRepository blockProgressRepository = null)
        {
            var progress = new OrchestrationProgress();
            try
            {
                var currentBlockNumber = fromNumber;
                while (currentBlockNumber <= toNumber && !cancellationToken.IsCancellationRequested)
                {

                    await CrawlBlockAsync(currentBlockNumber);
                    progress.BlockNumberProcessTo = currentBlockNumber;
                    if (blockProgressRepository != null)
                    {
                        await blockProgressRepository.UpsertProgressAsync(progress.BlockNumberProcessTo.Value);
                    }
                    currentBlockNumber = currentBlockNumber + 1;
                }
            }
            catch (Exception ex)
            {
                progress.Exception = ex;
            }

            return progress;
        }
    }
}