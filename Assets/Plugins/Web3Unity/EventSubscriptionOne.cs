using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using UnityEngine;

namespace Web3Unity
{

    public class EventSubscription<T, T1> : EventSubscription<T> where T : IEventDTO, new()
    {
        public T1 FirstFilterTopic { get; protected set; }

        /// <summary>
        /// Create a subscription for the event  (Not supported by all rpc/chain, use web3 instead)
        /// </summary>
        /// <param name="firstFilterTopic">filter for the first topic, can be null</param>
        /// <param name="address">Address of the contract to subscribe, if null all matches event will handle</param>
        /// <param name="pollingInterval">Time between request, default 1000ms (1s)</param>
        /// <param name="fromBlock">Request event from block, default pending block</param>
        /// <param name="toBlock">Request event to block, default latest</param>
        public EventSubscription(T1 firstFilterTopic, string address = null, int pollingInterval = 1000, BlockParameter fromBlock = null, BlockParameter toBlock = null, bool createFilter = true) : base(address, pollingInterval, fromBlock, toBlock, false)
        {
            FirstFilterTopic = firstFilterTopic;
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
                var filter = EventSubscriptionHandler.CreateFilterInput<T1>(FirstFilterTopic, fromBlock: FromBlock, toBlock: ToBlock);
                FilterId = await EventSubscriptionHandler.CreateFilterAsync(filter);
            }
        }
    }

}