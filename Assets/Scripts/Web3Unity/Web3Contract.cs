using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using WalletConnectSharp.Core;
using Web3Unity;

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

        private MetamaskProvider MetamaskProvider
        {
            get
            {
                return Web3Connect.Instance.MetamaskConnect;
            }
        }

        private WalletConnectSession WalletConnect
        {
            get
            {
                return Web3Connect.Instance.Web3WC.Client;
            }
        }

        private ConnectionType ConnectionType
        {
            get
            {
                return Web3Connect.Instance.ConnectionType;
            }
        }


        public Web3Contract(string _address)
        {
            this.Address = _address;
        }


        public async Task<U> Call<T, U>(T _function) where T : FunctionMessage, new() where U : IFunctionOutputDTO, new()
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskProvider.Call<T, U>(_function, Address);
            }
            else
            {
                var contractHandler = Web3.Eth.GetContractQueryHandler<T>();
                return await contractHandler.QueryAsync<U>(Address, _function);
            }

        }

        public async Task<string> Send<T>(T _function) where T : FunctionMessage, new()
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskProvider.Send<T>(_function, Address);
            }
            else
            {
                if (ConnectionType == ConnectionType.WalletConnect)
                {
                    _function.FromAddress = Web3Connect.Instance.AccountAddress;
                }
                var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
                return await contractHandler.SendRequestAsync(Address, _function);
            }
        }

        public async Task<TransactionReceipt> SendWaitForReceipt<T>(T _function) where T : FunctionMessage, new()
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskProvider.SendAndWaitForReceipt<T>(_function, Address);
            }
            else
            {
                if (ConnectionType == ConnectionType.WalletConnect)
                {
                    _function.FromAddress = Web3Connect.Instance.AccountAddress;
                }
                var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
                return await contractHandler.SendRequestAndWaitForReceiptAsync(Address, _function);
            }
        }

        public async Task<U> SendWaitForEvent<T, U>(T _function) where T : FunctionMessage, new() where U : new()
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

        public async Task<List<U>> SendWaitForEventList<T, U>(T _function) where T : FunctionMessage, new() where U : new()
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


        public async Task<HexBigInteger> EstimateGas<T>(T _function) where T : FunctionMessage, new()
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskProvider.EstimateGas<T>(_function, Address);
            }
            else
            {
                if (ConnectionType == ConnectionType.WalletConnect)
                {
                    _function.FromAddress = Web3Connect.Instance.AccountAddress;
                }
                var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
                return await contractHandler.EstimateGasAsync(Address, _function);
            }
        }

        public async Task<string> SignFunction<T>(T _function) where T : FunctionMessage, new()
        {
            if (ConnectionType == ConnectionType.Metamask)
            {
                return await MetamaskProvider.SignFunction<T>(_function, Address);
            }
            else
            {
                if (ConnectionType == ConnectionType.WalletConnect)
                {
                    _function.FromAddress = Web3Connect.Instance.AccountAddress;
                }
                var contractHandler = Web3.Eth.GetContractTransactionHandler<T>();
                return await contractHandler.SignTransactionAsync(Address, _function);
            }
        }
    }
}