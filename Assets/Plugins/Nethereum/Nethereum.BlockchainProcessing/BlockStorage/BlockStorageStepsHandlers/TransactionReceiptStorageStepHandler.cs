using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
using Nethereum.BlockchainProcessing.Processor;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.BlockStorage.BlockStorageStepsHandlers
{
    public class TransactionReceiptStorageStepHandler : ProcessorBaseHandler<TransactionReceiptVO>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAddressTransactionRepository _addressTransactionRepository;

        public TransactionReceiptStorageStepHandler(ITransactionRepository transactionRepository, IAddressTransactionRepository addressTransactionRepository = null)
        {
            _transactionRepository = transactionRepository;
            _addressTransactionRepository = addressTransactionRepository;
        }

        protected override async UniTask ExecuteInternalAsync(TransactionReceiptVO transactionReceiptVO)
        {
            await _transactionRepository.UpsertAsync(transactionReceiptVO);

            if(_addressTransactionRepository != null)
            {
                var newContractAddress = transactionReceiptVO.IsForContractCreation() ? transactionReceiptVO.TransactionReceipt.ContractAddress : string.Empty;
                foreach (var address in transactionReceiptVO.GetAllRelatedAddresses())
                {
                    await _addressTransactionRepository.UpsertAsync(transactionReceiptVO,
                                                                    address, 
                                                                    null, 
                                                                    newContractAddress);
                }
            }
        }
    }
}
