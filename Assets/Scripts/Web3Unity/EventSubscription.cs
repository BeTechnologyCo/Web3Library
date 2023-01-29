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
using System.Diagnostics;
using UnityEngine;

namespace Web3Unity
{

    public class EventSubscription<T> : IDisposable where T : IEventDTO, new()
    {
        public string Address { get; private set; }

        private Web3 Web3
        {
            get
            {
                return Web3Connect.Instance.Web3;
            }
        }

        private int _retryMilliseconds = 1000;
        private readonly object _lockingObject = new object();
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

        public event EventHandler<T> EventReceived;

        public HexBigInteger FilterId { get; private set; }
        public Event<T> EventSubscriptionHandler { get; private set; }

        private bool suscribe = true;


        public EventSubscription()
        {
        }

        public EventSubscription(string _address) : this()
        {
            this.Address = _address;
        }
        public EventSubscription(string _address, int pollingInterval) : this(_address)
        {
            SetPollingRetryIntervalInMilliseconds(pollingInterval);
        }

        private void CreateEventSubscriptionHandler()
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


        public async UniTask CreateFilterAsync()
        {
            await CreateFilter();
            RequestEvent();
        }

        private async Task CreateFilter()
        {
            if (Web3Connect.Instance.Web3 != null)
            {
                CreateEventSubscriptionHandler();
                var filter = EventSubscriptionHandler.CreateFilterInput(fromBlock:BlockParameter.CreateLatest());
                FilterId = await EventSubscriptionHandler.CreateFilterAsync(filter);
            }
        }

        private async void RequestEvent()
        {
            do
            {
                if (FilterId == null)
                {
                    await CreateFilter();
                }
                if (FilterId?.Value > 0)
                {
                    var filterEvents = await EventSubscriptionHandler.GetFilterChangesAsync(FilterId);
                    if (filterEvents?.Count > 0)
                    {
                        filterEvents.ForEach(fe =>
                        {
                            if (EventReceived != null)
                            {
                                EventReceived(this, fe.Event);
                            }
                        });
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