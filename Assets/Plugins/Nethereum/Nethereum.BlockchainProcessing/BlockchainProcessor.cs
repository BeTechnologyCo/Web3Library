using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nethereum.JsonRpc.Client;
using Nethereum.BlockchainProcessing.Orchestrator;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.RPC.Eth.Blocks;

namespace Nethereum.BlockchainProcessing
{
    public class BlockchainProcessor
    {
        protected IBlockchainProcessingOrchestrator BlockchainProcessingOrchestrator { get; set; }
        private IBlockProgressRepository _blockProgressRepository;
        private ILastConfirmedBlockNumberService _lastConfirmedBlockNumberService;
        private ILogger _log;

        public BlockchainProcessor(IBlockchainProcessingOrchestrator blockchainProcessingOrchestrator, IBlockProgressRepository blockProgressRepository, ILastConfirmedBlockNumberService lastConfirmedBlockNumberService,  ILogger log = null)
        {
            BlockchainProcessingOrchestrator = blockchainProcessingOrchestrator;
            _blockProgressRepository = blockProgressRepository;
            _lastConfirmedBlockNumberService = lastConfirmedBlockNumberService;
            _log = log;
            
        }

        //All scenarios have a repository (default in memory)

        //Scenario I have a repository and want to start from a block number if provided (if already processed I will use the latest one) and continue until cancellation
        public async UniTask ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken), BigInteger? startAtBlockNumberIfNotProcessed = null, int waitInterval = 0)
        {
            var fromBlockNumber = await GetStartBlockNumberAsync(startAtBlockNumberIfNotProcessed);
            
            while (!cancellationToken.IsCancellationRequested)
            {
				await UniTask.Delay(waitInterval);
				var blockToProcess = await _lastConfirmedBlockNumberService.GetLastConfirmedBlockNumberAsync(fromBlockNumber, cancellationToken);
                var progress = await BlockchainProcessingOrchestrator.ProcessAsync(fromBlockNumber, blockToProcess, cancellationToken, _blockProgressRepository);
                if (!progress.HasErrored)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        fromBlockNumber = progress.BlockNumberProcessTo.Value + 1;
                        await UpdateLastBlockProcessedAsync(progress.BlockNumberProcessTo);
                    }
                    else
                    {
                        //updating as other implementations might not have updated internally
                        await UpdateLastBlockProcessedAsync(progress.BlockNumberProcessTo);
                    }
                   
                }
                else
                {
                    await UpdateLastBlockProcessedAsync(progress.BlockNumberProcessTo);
                    throw progress.Exception;
                }
            }
        }

        //Scenario I have a repository and want to start from a block number if provided (if already processed I will use the latest one) and continue until the last block number provided
        public async UniTask ExecuteAsync(BigInteger toBlockNumber, CancellationToken cancellationToken = default(CancellationToken), BigInteger? startAtBlockNumberIfNotProcessed = null, int waitInterval = 0)
        {
            var fromBlockNumber = await GetStartBlockNumberAsync(startAtBlockNumberIfNotProcessed);

            while (!cancellationToken.IsCancellationRequested && fromBlockNumber <= toBlockNumber)
            {
				await UniTask.Delay(waitInterval);
				var blockToProcess = await _lastConfirmedBlockNumberService.GetLastConfirmedBlockNumberAsync(fromBlockNumber, cancellationToken);
                if (blockToProcess > toBlockNumber) blockToProcess = toBlockNumber;

                var progress = await BlockchainProcessingOrchestrator.ProcessAsync(fromBlockNumber, blockToProcess, cancellationToken, _blockProgressRepository);
                if (!progress.HasErrored)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        fromBlockNumber = progress.BlockNumberProcessTo.Value + 1;
                        await UpdateLastBlockProcessedAsync(progress.BlockNumberProcessTo);
                    }
                    else
                    {
                        //updating as other implementations might not have updated internally
                        await UpdateLastBlockProcessedAsync(progress.BlockNumberProcessTo);
                    }
                    }
                else
                {
                    await UpdateLastBlockProcessedAsync(progress.BlockNumberProcessTo);
                    throw progress.Exception;
                }
            }
        }

        //Checks the last number in the progress repository and if bigger than the startAtBlockNumber uses that one.
        private async UniTask<BigInteger> GetStartBlockNumberAsync(BigInteger? startAtBlockNumberIfNotProcessed)
        {
            var lastProcessedNumber = await _blockProgressRepository.GetLastBlockNumberProcessedAsync();

            if(lastProcessedNumber == null) //nothing previously processed
            {
                //return requested starting point else block 0 
                return startAtBlockNumberIfNotProcessed ?? 0;
            }

            //we have previously processed - assume we want the next block
            var fromBlockNumber = lastProcessedNumber.Value + 1;

            //check that the next block is not behind what has been requested
            if (startAtBlockNumberIfNotProcessed != null && startAtBlockNumberIfNotProcessed > fromBlockNumber)
            {
                fromBlockNumber = startAtBlockNumberIfNotProcessed.Value;
            }

            return fromBlockNumber;
        }

        private async UniTask UpdateLastBlockProcessedAsync(BigInteger? lastBlock)
        {
            if (lastBlock != null)
            {
                await _blockProgressRepository.UpsertProgressAsync(lastBlock.Value);
                _log?.LogInformation($"Last Block Processed: {lastBlock}");
            }
            else
            {
                _log?.LogInformation($"No Block Processed");
            }
        }
    }
}