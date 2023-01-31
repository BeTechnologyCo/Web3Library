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

    public class EventSubscription<T, T1, T2> : EventSubscription<T, T1> where T : IEventDTO, new()
    {
        public T2 SecondFilterTopic { get; protected set; }

        /// <summary>
        /// Create a subscription for the event  (Not supported by all rpc/chain, use web3 instead)
        /// </summary>
        /// <param name="firstFilterTopic">filter for the first topic, can be null</param>
        /// <param name="secondFilterTopic">filter for the second topic, can be null</param>
        /// <param name="address">Address of the contract to subscribe, if null all matches event will handle</param>
        /// <param name="pollingInterval">Time between request, default 1000ms (1s)</param>
        /// <param name="fromBlock">Request event from block, default pending block</param>
        /// <param name="toBlock">Request event to block, default latest</param>
        public EventSubscription(T1 firstFilterTopic, T2 secondFilterTopic, string address = null, int pollingInterval = 1000, BlockParameter fromBlock = null, BlockParameter toBlock = null, bool createFilter = true) : base(firstFilterTopic, address, pollingInterval, fromBlock, toBlock, false)
        {
            SecondFilterTopic = secondFilterTopic;
            if (createFilter)
            {
                CreateFilterAsync();
            }
        }

        protected override async UniTask CreateFilter()
        {
            if (Web3Connect.Instance.Web3 != null)
            {
                CreateEventSubscriptionHandler();
                var filter = EventSubscriptionHandler.CreateFilterInput<T1, T2>(FirstFilterTopic, SecondFilterTopic, fromBlock: BlockParameter.CreatePending());
                FilterId = await EventSubscriptionHandler.CreateFilterAsync(filter);
            }
        }
    }

}