using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Services;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps
{
    public class TransactionCrawlerStep : CrawlerStep<TransactionVO, TransactionVO>
    {
        public TransactionCrawlerStep(IEthApiContractService ethApiContractService) : base(ethApiContractService)
        {
        }

        public override UniTask<TransactionVO> GetStepDataAsync(TransactionVO parentStep)
        {
            return UniTask.FromResult(parentStep);
        }
    }
}