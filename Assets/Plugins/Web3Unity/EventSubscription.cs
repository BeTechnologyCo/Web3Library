using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using WalletConnectSharp.Core;
using UnityEngine;

namespace Web3Unity
{

    public class EventSubscription<T> : IDisposable where T : IEventDTO, new()
    {
        public string Address { get; protected set; }
        private BlockParameter _fromBlock;

        public BlockParameter FromBlock
        {
            get
            {
                if (_fromBlock == null)
                {
                    _fromBlock = BlockParameter.CreatePending();
                }
                return _fromBlock;
            }
            protected set { _fromBlock = value; }
        }

        public BlockParameter ToBlock { get; protected set; }

        protected Web3 Web3
        {
            get
            {
                return Web3Connect.Instance.Web3;
            }
        }

        protected int _retryMilliseconds = 1000;
        protected readonly object _lockingObject = new object();
        public int GetPollingRetryIntervalInMilliseconds()
        {
            lock (_lockingObject)
            {
                return _retryMilliseconds;
            }
        }

        public void SetPollingRetryIntervalInMilliseconds(int retryMilliseconds)
        {
            lock (_lockingObject)
            {
                _retryMilliseconds = retryMilliseconds;
            }
        }

        public event EventHandler<List<T>> OnEventsReceived;

        public HexBigInteger FilterId { get; protected set; }
        public Event<T> EventSubscriptionHandler { get; protected set; }

        protected bool suscribe = true;

        /// <summary>
        /// Create a subscription for the event (Not supported by all rpc/chain, use web3 instead)
        /// </summary>
        /// <param name="address">Address of the contract to subscribe, if null all matches event will handle</param>
        /// <param name="pollingInterval">Time between request, default 1000ms (1s)</param>
        /// <param name="fromBlock">Request event from block, default pending block</param>
        /// <param name="toBlock">Request event to block, default latest</param>
        public EventSubscription(string address = null, int pollingInterval = 1000, BlockParameter fromBlock = null, BlockParameter toBlock = null, bool createFilter = true)
        {
            this.Address = address;
            SetPollingRetryIntervalInMilliseconds(pollingInterval);
            if (createFilter)
            {
                CreateFilterAsync();
            }
        }

        protected void CreateEventSubscriptionHandler()
        {
            if (Web3Connect.Instance.Web3 != null)
            {
                if (!string.IsNullOrEmpty(Address))
                {
                    EventSubscriptionHandler = Web3Connect.Instance.Web3.Eth.GetEvent<T>(Address);
                }
                else
                {
                    EventSubscriptionHandler = Web3Connect.Instance.Web3.Eth.GetEvent<T>();
                }
            }
        }


        public async void CreateFilterAsync()
        {
            await CreateFilter();
            RequestEvent();
        }

        protected virtual async UniTask CreateFilter()
        {
            if (Web3Connect.Instance.Web3 != null)
            {
                CreateEventSubscriptionHandler();
                var filter = EventSubscriptionHandler.CreateFilterInput(fromBlock: FromBlock, toBlock: ToBlock);
                FilterId = await EventSubscriptionHandler.CreateFilterAsync(filter);
            }
        }

        protected async void RequestEvent()
        {
            do
            {
                if (FilterId == null)
                {
                    await CreateFilter();
                }
                if (OnEventsReceived != null && FilterId?.Value > 0)
                {
                    var filterEvents = await EventSubscriptionHandler.GetFilterChangesAsync(FilterId);
                    if (filterEvents?.Count > 0)
                    {
                        if (OnEventsReceived != null)
                        {
                            OnEventsReceived(this, filterEvents.Select(x => x.Event).ToList());
                        }

                    }
                }

                await UniTask.Delay(_retryMilliseconds);

            } while (suscribe);
        }

        public void Dispose()
        {
            suscribe = false;
        }
    }

}