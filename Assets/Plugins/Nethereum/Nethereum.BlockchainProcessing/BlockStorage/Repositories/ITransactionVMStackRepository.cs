using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Newtonsoft.Json.Linq;

namespace Nethereum.BlockchainProcessing.BlockStorage.Repositories
{
    public interface ITransactionVMStackRepository
    { 
        UniTask UpsertAsync(string transactionHash, string address, JObject stackTrace);
        UniTask<ITransactionVmStackView> FindByTransactionHashAsync(string hash);
        UniTask<ITransactionVmStackView> FindByAddressAndTransactionHashAsync(string address, string hash);
    }
}