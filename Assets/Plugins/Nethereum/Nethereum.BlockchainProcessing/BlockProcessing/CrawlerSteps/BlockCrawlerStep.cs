using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps
{
    public class BlockCrawlerStep : CrawlerStep<BigInteger, BlockWithTransactions>
    {
        public BlockCrawlerStep(IEthApiContractService ethApiContractService) : base(ethApiContractService)
        {

        }
        public override UniTask<BlockWithTransactions> GetStepDataAsync(BigInteger blockNumber)
        {
            return EthApi.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumber.ToHexBigInteger());
        }
    }
}