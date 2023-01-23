using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Processor
{
    public interface IProcessorHandler<T>
    {
        void SetMatchCriteria(Func<T, bool> criteria);
        void SetMatchCriteria(Func<T, UniTask<bool>> criteria);
        UniTask ExecuteAsync(T value);
        UniTask<bool> IsMatchAsync(T value);
    }
}