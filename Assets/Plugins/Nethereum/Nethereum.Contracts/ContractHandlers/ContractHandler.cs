using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.Decoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.ContractHandlers
{
    public class ContractHandler
    {
        public ContractHandler(string contractAddress, EthApiContractService ethApiContractService,
            string addressFrom = null)
        {
            ContractAddress = contractAddress;
            EthApiContractService = ethApiContractService;
            AddressFrom = addressFrom;
        }

        protected string AddressFrom { get; set; }

        public string ContractAddress { get; }
        public EthApiContractService EthApiContractService { get; }

        public Event<TEventType> GetEvent<TEventType>() where TEventType : IEventDTO, new()
        {
            if (!EventAttribute.IsEventType(typeof(TEventType))) return null;
            return new Event<TEventType>(EthApiContractService.Client, ContractAddress);
        }

        public Function<TEthereumContractFunctionMessage> GetFunction<TEthereumContractFunctionMessage>() where TEthereumContractFunctionMessage : new()
        {
            var contract = EthApiContractService.GetContract<TEthereumContractFunctionMessage>(ContractAddress);
            return contract.GetFunction<TEthereumContractFunctionMessage>();
        }

        protected void SetAddressFrom(ContractMessageBase contractMessage)
        {
            contractMessage.FromAddress = contractMessage.FromAddress ?? AddressFrom;
        }

#if !DOTNET35

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null, CancellationTokenSource tokenSource = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SendRequestAndWaitForReceiptAsync(ContractAddress, transactionMessage, tokenSource);
        }

        public UniTask<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage, CancellationToken cancellationToken)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SendRequestAndWaitForReceiptAsync(ContractAddress, transactionMessage, cancellationToken);
        }

        public UniTask<string> SendRequestAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if(transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SendRequestAsync(ContractAddress, transactionMessage);
        }

        public UniTask<string> SignTransactionAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SignTransactionAsync(ContractAddress, transactionMessage);
        }

        public UniTask<HexBigInteger> EstimateGasAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.EstimateGasAsync(ContractAddress, transactionMessage);
        }

        public UniTask<TEthereumFunctionReturn> QueryDeserializingToObjectAsync<TEthereumContractFunctionMessage,
            TEthereumFunctionReturn>(TEthereumContractFunctionMessage ethereumContractFunctionMessage = null,
            BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
            where TEthereumFunctionReturn : IFunctionOutputDTO, new()
        {
            if (ethereumContractFunctionMessage == null) ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            SetAddressFrom(ethereumContractFunctionMessage);
            var queryHandler = EthApiContractService.GetContractQueryHandler<TEthereumContractFunctionMessage>();
            return queryHandler.QueryDeserializingToObjectAsync<TEthereumFunctionReturn>(
                ethereumContractFunctionMessage,
                ContractAddress, blockParameter);
        }

        public UniTask<TReturn> QueryAsync<TEthereumContractFunctionMessage, TReturn>(
            TEthereumContractFunctionMessage ethereumContractFunctionMessage = null, BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if(ethereumContractFunctionMessage == null) ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            SetAddressFrom(ethereumContractFunctionMessage);
            var queryHandler = EthApiContractService.GetContractQueryHandler<TEthereumContractFunctionMessage>();
            return queryHandler.QueryAsync<TReturn>(
                ContractAddress, ethereumContractFunctionMessage, blockParameter);
        }

        public UniTask<byte[]> QueryRawAsync<TEthereumContractFunctionMessage>(TEthereumContractFunctionMessage ethereumContractFunctionMessage = null, BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (ethereumContractFunctionMessage == null) ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            SetAddressFrom(ethereumContractFunctionMessage);
            var queryHandler = EthApiContractService.GetContractQueryHandler<TEthereumContractFunctionMessage>();
            return queryHandler.QueryRawAsBytesAsync(
                ContractAddress, ethereumContractFunctionMessage, blockParameter);
        }
        
        public UniTask<TReturn> QueryRawAsync<TEthereumContractFunctionMessage, TCustomDecoder, TReturn>(BlockParameter blockParameter = null)
           where TEthereumContractFunctionMessage : FunctionMessage, new()
           where TCustomDecoder : ICustomRawDecoder<TReturn>, new()
        {
            var ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            return QueryRawAsync<TEthereumContractFunctionMessage, TCustomDecoder, TReturn>(ethereumContractFunctionMessage, blockParameter);
        }

        public async UniTask<TReturn> QueryRawAsync<TEthereumContractFunctionMessage, TCustomDecoder, TReturn>(TEthereumContractFunctionMessage ethereumContractFunctionMessage, BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
            where TCustomDecoder : ICustomRawDecoder<TReturn>, new()
        {
          
            var result = await QueryRawAsync<TEthereumContractFunctionMessage>(ethereumContractFunctionMessage,
                blockParameter);
            var decoder = new TCustomDecoder();
            return decoder.Decode(result);
        }
#endif

    }

}