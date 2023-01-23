using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Processor
{
    public abstract class ProcessorBaseHandler<T> : IProcessorHandler<T>
    {
        public Func<T, UniTask<bool>> Criteria { get; protected set; }
        protected ProcessorBaseHandler()
        {
            
        }
        protected ProcessorBaseHandler(Func<T, UniTask<bool>> criteria)
        {
            SetMatchCriteria(criteria);
        }

        protected ProcessorBaseHandler(Func<T, bool> criteria)
        {
            SetMatchCriteria(criteria);
        }

        public void SetMatchCriteria(Func<T, bool> criteria)
        {
            if(criteria == null) return;

            Func<T, UniTask<bool>> asyncCriteria = async (t) => criteria(t);

            SetMatchCriteria(asyncCriteria);
        }
        public void SetMatchCriteria(Func<T, UniTask<bool>> criteria)
        {
            Criteria = criteria;
        }

        public virtual async UniTask<bool> IsMatchAsync(T value)
        {
            if (Criteria == null) return true;
            return await Criteria(value);
        }

        public virtual async UniTask ExecuteAsync(T value)
        {
            if (await IsMatchAsync(value))
            {
                await ExecuteInternalAsync(value);
            }
        }
        protected abstract UniTask ExecuteInternalAsync(T value);

    }
}