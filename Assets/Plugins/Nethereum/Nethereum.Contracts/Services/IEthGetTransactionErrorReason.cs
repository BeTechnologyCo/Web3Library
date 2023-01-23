using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.Contracts.Services
{
    public interface IEthGetContractTransactionErrorReason
    {
#if !DOTNET35
        UniTask<string> SendRequestAsync(string transactionHash);
#endif
    }
}