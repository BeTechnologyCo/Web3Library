using Nethereum.JsonRpc.Client;
using Nethereum.BlockchainProcessing.BlockProcessing;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.RPC.Eth.Blocks;
using Microsoft.Extensions.Logging;

namespace Nethereum.BlockchainProcessing
{
    public class BlockchainCrawlingProcessor : BlockchainProcessor
    {
        public BlockCrawlOrchestrator Orchestrator => (BlockCrawlOrchestrator)BlockchainProcessingOrchestrator;
        public BlockchainCrawlingProcessor(BlockCrawlOrchestrator blockchainProcessingOrchestrator, IBlockProgressRepository blockProgressRepository, ILastConfirmedBlockNumberService lastConfirmedBlockNumberService, ILogger log = null):base(blockchainProcessingOrchestrator, blockProgressRepository, lastConfirmedBlockNumberService, log)
        {
            
        }
    }
}