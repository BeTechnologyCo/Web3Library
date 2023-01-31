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

    public class EventSubscription<T, T1, T2, T3> : EventSubscription<T, T1, T2> where T : IEventDTO, new()
    {
        public T3 ThirdFilterTopic { get; protected set; }

        /// <summary>
        /// Create a subscription for the event  (Not supported by all rpc/chain, use web3 instead)
        /// </summary>
        /// <param name="firstFilterTopic">filter for the first topic, can be null</param>
        /// <param name="secondFilterTopic">filter for the second topic, can be null</param>
        /// <param name="thirdFilterTopic">filter for the third topic, can be null</param>
        /// <param name="address">Address of the contract to subscribe, if null all matches event will handle</param>
        /// <param name="pollingInterval">Time between request, default 1000ms (1s)</param>
        /// <param name="fromBlock">Request event from block, default pending block</param>
        /// <param name="toBlock">Request event to block, default latest</param>
        public EventSubscription(T1 firstFilterTopic, T2 secondFilterTopic, T3 thirdFilterTopic, string address = null, int pollingInterval = 1000, BlockParameter fromBlock = null, BlockParameter toBlock = null, bool createFilter = true) : base(firstFilterTopic, secondFilterTopic, address, pollingInterval, fromBlock, toBlock, false)
        {
            ThirdFilterTopic = thirdFilterTopic;
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
                var filter = EventSubscriptionHandler.CreateFilterInput<T1, T2, T3>(FirstFilterTopic, SecondFilterTopic, ThirdFilterTopic, fromBlock: BlockParameter.CreatePending());
                FilterId = await EventSubscriptionHandler.CreateFilterAsync(filter);
            }
        }
    }

}