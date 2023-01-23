using System;
using System.Collections.Generic;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Processor
{
    public class Processor<T>: ProcessorBaseHandler<T>, IProcessor<T>
    {
        public Processor()
        {

        }
        public Processor(Func<T, UniTask<bool>> criteria):base(criteria)
        {
            
        }

        public Processor(Func<T, bool> criteria) : base(criteria)
        {
            
        }
        protected List<IProcessorHandler<T>> ProcessorHandlers { get; set; } = new List<IProcessorHandler<T>>();

        public virtual void AddProcessorHandler(Func<T,UniTask> action)
        {
            ProcessorHandlers.Add(new ProcessorHandler<T>(action));
        }
        public virtual void AddProcessorHandler(Func<T, UniTask> action, Func<T, bool> criteria)
        {
            ProcessorHandlers.Add(new ProcessorHandler<T>(action, criteria));
        }

        public virtual void AddProcessorHandler(Func<T, UniTask> action, Func<T, UniTask<bool>> criteria)
        {
            ProcessorHandlers.Add(new ProcessorHandler<T>(action, criteria));
        }

        public virtual void AddProcessorHandler(IProcessorHandler<T> processorHandler)
        {
            ProcessorHandlers.Add(processorHandler);
        }

        protected override async UniTask ExecuteInternalAsync(T value)
        {
            foreach (var x in ProcessorHandlers)
            {
                await x.ExecuteAsync(value);
            }
        }
    }
}