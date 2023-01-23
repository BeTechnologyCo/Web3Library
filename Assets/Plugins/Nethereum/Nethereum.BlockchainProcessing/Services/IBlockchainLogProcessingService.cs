using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nethereum.JsonRpc.Client;
using Nethereum.BlockchainProcessing.Processor;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.BlockchainProcessing.Services.SmartContracts;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.Blocks;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.Services
{
    public interface IBlockchainLogProcessingService
    {
        BlockchainProcessor CreateProcessor<TEventDTO>(
            Action<EventLog<TEventDTO>> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<EventLog<TEventDTO>, bool> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class, new();

        BlockchainProcessor CreateProcessorForContract<TEventDTO>(
            string contractAddress,
            Action<EventLog<TEventDTO>> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<EventLog<TEventDTO>, bool> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class, new();

        BlockchainProcessor CreateProcessorForContracts<TEventDTO>(
            string[] contractAddresses,
            Action<EventLog<TEventDTO>> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<EventLog<TEventDTO>, bool> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class, new();

        BlockchainProcessor CreateProcessor<TEventDTO>(
            Func<EventLog<TEventDTO>, UniTask> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<EventLog<TEventDTO>, UniTask<bool>> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class, new();

        BlockchainProcessor CreateProcessorForContract<TEventDTO>(
            string contractAddress,
            Func<EventLog<TEventDTO>, UniTask> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<EventLog<TEventDTO>, UniTask<bool>> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class, new();

        BlockchainProcessor CreateProcessorForContracts<TEventDTO>(
            string[] contractAddresses,
            Func<EventLog<TEventDTO>, UniTask> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<EventLog<TEventDTO>, UniTask<bool>> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class, new();

        BlockchainProcessor CreateProcessorForContracts<TEventDTO>(
            ProcessorHandler<FilterLog> logProcessor,
            string[] contractAddresses,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null) where TEventDTO : class;

        BlockchainProcessor CreateProcessorForContract(

            string contractAddress,
            Action<FilterLog> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<FilterLog, bool> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null);

        BlockchainProcessor CreateProcessorForContracts(

            string[] contractAddresses,
            Action<FilterLog> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<FilterLog, bool> criteria = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null);

        BlockchainProcessor CreateProcessor(

            Action<FilterLog> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<FilterLog, bool> criteria = null,
            NewFilterInput filter = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null);

        BlockchainProcessor CreateProcessor(

            Func<FilterLog, UniTask> action,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            Func<FilterLog, UniTask<bool>> criteria = null,
            NewFilterInput filter = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null);

        BlockchainProcessor CreateProcessor(

            ProcessorHandler<FilterLog> logProcessor,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            NewFilterInput filter = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null);

        BlockchainProcessor CreateProcessor(

            IEnumerable<ProcessorHandler<FilterLog>> logProcessors,
            uint minimumBlockConfirmations = LastConfirmedBlockNumberService.DEFAULT_BLOCK_CONFIRMATIONS,
            NewFilterInput filter = null,
            IBlockProgressRepository blockProgressRepository = null,
            ILogger log = null, int defaultNumberOfBlocksPerRequest = 100, int retryWeight = 0);

        UniTask<List<EventLog<TEventDTO>>> GetAllEvents<TEventDTO>(NewFilterInput filterInput,
            BigInteger? fromBlockNumber, BigInteger? toBlockNumber, CancellationToken cancellationToken, int numberOfBlocksPerRequest = BlockchainLogProcessingService.DefaultNumberOfBlocksPerRequest,
            int retryWeight = BlockchainLogProcessingService.RetryWeight) where TEventDTO : class, new();

        UniTask<List<EventLog<TEventDTO>>> GetAllEventsForContracts<TEventDTO>(string[] contractAddresses,
            BigInteger? fromBlockNumber, BigInteger? toBlockNumber, CancellationToken cancellationToken, int numberOfBlocksPerRequest = BlockchainLogProcessingService.DefaultNumberOfBlocksPerRequest,
            int retryWeight = BlockchainLogProcessingService.RetryWeight) where TEventDTO : class, new();

        UniTask<List<EventLog<TEventDTO>>> GetAllEventsForContract<TEventDTO>(string contractAddress,
            BigInteger? fromBlockNumber, BigInteger? toBlockNumber, CancellationToken cancellationToken, int numberOfBlocksPerRequest = BlockchainLogProcessingService.DefaultNumberOfBlocksPerRequest,
            int retryWeight = BlockchainLogProcessingService.RetryWeight) where TEventDTO : class, new();

        UniTask<List<FilterLog>> GetAllEvents(NewFilterInput filterInput,
            BigInteger? fromBlockNumber, BigInteger? toBlockNumber, CancellationToken cancellationToken, int numberOfBlocksPerRequest = BlockchainLogProcessingService.DefaultNumberOfBlocksPerRequest,
            int retryWeight = BlockchainLogProcessingService.RetryWeight);

        UniTask<List<FilterLog>> GetAllEventsForContracts(string[] contractAddresses,
            BigInteger? fromBlockNumber, BigInteger? toBlockNumber, CancellationToken cancellationToken, int numberOfBlocksPerRequest = BlockchainLogProcessingService.DefaultNumberOfBlocksPerRequest,
            int retryWeight = BlockchainLogProcessingService.RetryWeight);

        UniTask<List<FilterLog>> GetAllEventsForContract(string contractAddress,
            BigInteger? fromBlockNumber, BigInteger? toBlockNumber, CancellationToken cancellationToken, int numberOfBlocksPerRequest = BlockchainLogProcessingService.DefaultNumberOfBlocksPerRequest,
            int retryWeight = BlockchainLogProcessingService.RetryWeight);

        IERC20LogProcessingService ERC20 { get; }
        IERC721LogProcessingService ERC721 { get; }
    }
}