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

namespace Web3Unity
{

    public class Web3Contract
    {
        public string Address { get; private set; }

        private Web3 Web3
        {
            get
            {
                return Web3Connect.Instance.Web3;
            }
        }

        public Web3Contract(string _address)
        {
            this.Address = _address;
        }


        public async UniTask<U> Call<T, U>(T _function) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
        {

            var contractHandler = Web3.Eth.GetContractQueryHandler<T>();
            return await contractHandler.QueryAsync<U>(Address, _function);


        }

        public async UniTask<string> Send<T>(T _function) where T : FunctionMessage, new()
        {

            var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
            return await contractHandler.SendRequestAsync(Address, _function);

        }

        public async UniTask<TransactionReceipt> SendWaitForReceipt<T>(T _function) where T : FunctionMessage, new()
        {

            var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
            return await contractHandler.SendRequestAndWaitForReceiptAsync(Address, _function);

        }

        public async UniTask<U> SendWaitForEvent<T, U>(T _function) where T : FunctionMessage, new() where U : new()
        {
            TransactionReceipt receipt = await SendWaitForReceipt(_function);

            if (receipt != null && receipt.Succeeded())
            {
                var events = receipt.DecodeAllEvents<U>();
                if (events.Count > 0)
                {
                    return events[0].Event;
                }
                throw (new Exception("No event found"));
            }
            throw (new Exception($"Transaction failed, tx hash : ${receipt?.TransactionHash}"));
        }

        public async UniTask<List<U>> SendWaitForEventList<T, U>(T _function) where T : FunctionMessage, new() where U : new()
        {
            TransactionReceipt receipt = await SendWaitForReceipt(_function);
            if (receipt != null && receipt.Succeeded())
            {
                var events = receipt.DecodeAllEvents<U>();
                if (events.Count > 0)
                {
                    return events.Select(x => x.Event).ToList();
                }
                throw (new Exception("No event found"));
            }
            throw (new Exception($"Transaction failed, tx hash : ${receipt?.TransactionHash}"));
        }


        public async UniTask<HexBigInteger> EstimateGas<T>(T _function) where T : FunctionMessage, new()
        {
            var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
            return await contractHandler.EstimateGasAsync(Address, _function);
        }

        public async UniTask<string> SignFunction<T>(T _function) where T : FunctionMessage, new()
        {
            var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
            return await contractHandler.SignTransactionAsync(Address, _function);
        }
    }
}