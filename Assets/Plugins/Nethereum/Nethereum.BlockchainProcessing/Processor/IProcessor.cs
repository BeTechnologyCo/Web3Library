using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Processor
{
    public interface IProcessor<T> : IProcessorHandler<T>
    {
        void AddProcessorHandler(Func<T, UniTask> action);
        void AddProcessorHandler(IProcessorHandler<T> processorHandler);
    }
}