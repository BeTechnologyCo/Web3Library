using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Web3Unity
{
    public static class TransactionReceiptExtension
    {
        /// <summary>
        /// Extract event from a transaction receipt
        /// </summary>
        /// <typeparam name="U">Event type to extract</typeparam>
        /// <param name="receipt">Transaction receipt</param>
        /// <returns>The first event to match</returns>
        public static U GetEvent<U>(this TransactionReceipt receipt) where U : new()
        {
            if (receipt != null && receipt.Succeeded())
            {
                var events = receipt.DecodeAllEvents<U>();
                if (events.Count > 0)
                {
                    return events[0].Event;
                }
            }
            return default;
        }

        /// <summary>
        /// Extract a list event from a transaction receipt
        /// </summary>
        /// <typeparam name="U">Event type to extract</typeparam>
        /// <param name="receipt">Transaction receipt</param>
        /// <returns>All event to match</returns>
        public static List<U> GetEventList<U>(this TransactionReceipt receipt) where U : new()
        {
            if (receipt != null && receipt.Succeeded())
            {
                var events = receipt.DecodeAllEvents<U>();
                if (events.Count > 0)
                {
                    return events.Select(x => x.Event).ToList();
                }
            }
            return new List<U>();
        }

    }
}
