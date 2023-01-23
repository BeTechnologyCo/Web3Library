using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.BlockchainProcessing.Processor
{
    public class EventLogProcessorHandler<TEvent> : ProcessorHandler<FilterLog> where TEvent : new()
    {
        Func<EventLog<TEvent>, UniTask> _eventAction;
        Func<EventLog<TEvent>, UniTask<bool>> _eventCriteria;

        public EventLogProcessorHandler(
            Func<EventLog<TEvent>, UniTask> action) : this(action, null)
        {
        }

        public EventLogProcessorHandler(
            Func<EventLog<TEvent>, UniTask> action,
            Func<EventLog<TEvent>, UniTask<bool>> eventCriteria) 
        {
            _eventAction = action;
            _eventCriteria = eventCriteria;
            SetMatchCriteriaForEvent();
        }

        public EventLogProcessorHandler(
            Action<EventLog<TEvent>> action) : this(action, null)
        {
        }

        public EventLogProcessorHandler(
            Action<EventLog<TEvent>> action,
            Func<EventLog<TEvent>, bool> eventCriteria)
        {
            _eventAction = (l) => { action(l); return UniTask.FromResult(0); };
            if (eventCriteria != null)
            {
                _eventCriteria = async (l) => { return await UniTask.FromResult(eventCriteria(l)); };
            }
            SetMatchCriteriaForEvent();
        }

        private void SetMatchCriteriaForEvent()
        {
            base.SetMatchCriteria(async log =>
            {
                if (await UniTask.FromResult(log.IsLogForEvent<TEvent>()) == false) return false;

                if (_eventCriteria == null) return true;

                var eventLog = log.DecodeEvent<TEvent>();
                return await _eventCriteria(eventLog);
            });
        }

        protected override UniTask ExecuteInternalAsync(FilterLog value)
        {
            var eventLog = value.DecodeEvent<TEvent>();
            return _eventAction(eventLog);
        }
    }
}