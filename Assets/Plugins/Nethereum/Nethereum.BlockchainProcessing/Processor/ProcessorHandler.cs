using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Processor
{
    public class ProcessorHandler<T> : ProcessorBaseHandler<T>
    {
        private Func<T, UniTask> _action;

        protected ProcessorHandler()
        {

        }

        public ProcessorHandler(Func<T, UniTask> action)
        {
            _action = action;
        }

        public ProcessorHandler(Func<T, UniTask> action, Func<T, UniTask<bool>> criteria):base(criteria)
        {
            _action = action;
        }

        public ProcessorHandler(Func<T, UniTask> action, Func<T, bool> criteria) : base(criteria)
        {
            _action = action;
        }

        public ProcessorHandler(Action<T> action, Func<T, UniTask<bool>> criteria) : base(criteria)
        {
            SetAction(action);
        }

        public ProcessorHandler(Action<T> action, Func<T, bool> criteria) : base(criteria)
        {
            SetAction(action);
        }

        private void SetAction(Action<T> action)
        {
            Func<T, UniTask> asyncAction = (t) =>
            {
                action(t);
                return UniTask.FromResult(0);
            };

            _action = asyncAction;
        }

        protected override UniTask ExecuteInternalAsync(T value)
        {
            return _action(value);
        }
    }
}