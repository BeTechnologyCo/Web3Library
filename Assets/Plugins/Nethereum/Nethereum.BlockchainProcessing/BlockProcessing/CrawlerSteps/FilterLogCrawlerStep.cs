using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Services;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps
{
    public class FilterLogCrawlerStep : CrawlerStep<FilterLogVO, FilterLogVO>
    {
        public FilterLogCrawlerStep(IEthApiContractService ethApiContractService) : base(ethApiContractService)
        {
        }

        public override UniTask<FilterLogVO> GetStepDataAsync(FilterLogVO filterLogVO)
        {
            return UniTask.FromResult(filterLogVO);
        }
    }
}