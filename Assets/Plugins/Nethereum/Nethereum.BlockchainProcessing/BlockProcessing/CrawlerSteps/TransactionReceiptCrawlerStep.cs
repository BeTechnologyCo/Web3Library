using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Services;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps
{
    public class TransactionReceiptCrawlerStep : CrawlerStep<TransactionVO, TransactionReceiptVO>
    {
        public TransactionReceiptCrawlerStep(IEthApiContractService ethApiContractService) : base(ethApiContractService)
        {
        }

        public override async UniTask<TransactionReceiptVO> GetStepDataAsync(TransactionVO transactionVO)
        {
            var receipt = await EthApi.Transactions
                .GetTransactionReceipt.SendRequestAsync(transactionVO.Transaction.TransactionHash)
                ;
            return new TransactionReceiptVO(transactionVO.Block, transactionVO.Transaction, receipt, receipt.HasErrors()?? false);
        }
    }
}