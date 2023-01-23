using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.Decoders;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ERC20
{
    public class ERC20ContractService
    {
        public string ContractAddress { get; }

        public ContractHandler ContractHandler { get; }

        public ERC20ContractService(IEthApiContractService ethApiContractService, string contractAddress)
        {
            ContractAddress = contractAddress;
#if !DOTNET35
            ContractHandler = ethApiContractService.GetContractHandler(contractAddress);
#endif
        }

        public Event<ApprovalEventDTO> GetApprovalEvent()
        {
            return ContractHandler.GetEvent<ApprovalEventDTO>();
        }

        public Event<TransferEventDTO> GetTransferEvent()
        {
            return ContractHandler.GetEvent<TransferEventDTO>();
        }
#if !DOTNET35
        public UniTask<string> NameQueryAsync(NameFunction nameFunction = null, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryRawAsync<NameFunction, StringBytes32Decoder, string>(nameFunction,
                blockParameter);
        }

        public UniTask<string> SymbolQueryAsync(SymbolFunction symbolFunction = null, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryRawAsync<SymbolFunction, StringBytes32Decoder, string>(symbolFunction,
                blockParameter);
        }

        public UniTask<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
            return ContractHandler.SendRequestAsync(approveFunction);
        }

        public UniTask<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction,
            CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public UniTask<string> ApproveRequestAsync(string spender, BigInteger value)
        {
            var approveFunction = new ApproveFunction();
            approveFunction.Spender = spender;
            approveFunction.Value = value;

            return ContractHandler.SendRequestAsync(approveFunction);
        }

        public UniTask<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger value,
            CancellationToken cancellationToken = default)
        {
            var approveFunction = new ApproveFunction();
            approveFunction.Spender = spender;
            approveFunction.Value = value;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public UniTask<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction,
            BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }


        public UniTask<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
            return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public UniTask<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(
            TransferFromFunction transferFromFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public UniTask<string> TransferFromRequestAsync(string from, string to, BigInteger value)
        {
            var transferFromFunction = new TransferFromFunction();
            transferFromFunction.From = from;
            transferFromFunction.To = to;
            transferFromFunction.Value = value;

            return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public UniTask<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string from, string to,
            BigInteger value, CancellationToken cancellationToken = default)
        {
            var transferFromFunction = new TransferFromFunction();
            transferFromFunction.From = from;
            transferFromFunction.To = to;
            transferFromFunction.Value = value;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public UniTask<BigInteger> BalancesQueryAsync(BalancesFunction balancesFunction,
            BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalancesFunction, BigInteger>(balancesFunction, blockParameter);
        }

        public UniTask<BigInteger> BalancesQueryAsync(string address, BlockParameter blockParameter = null)
        {
            var balancesFunction = new BalancesFunction();
            balancesFunction.Address = address;

            return ContractHandler.QueryAsync<BalancesFunction, BigInteger>(balancesFunction, blockParameter);
        }

        public UniTask<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        public UniTask<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public UniTask<BigInteger> AllowedQueryAsync(AllowedFunction allowedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowedFunction, BigInteger>(allowedFunction, blockParameter);
        }

        public UniTask<BigInteger> AllowedQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowedFunction = new AllowedFunction();
            allowedFunction.Owner = owner;
            allowedFunction.Spender = spender;

            return ContractHandler.QueryAsync<AllowedFunction, BigInteger>(allowedFunction, blockParameter);
        }

        public UniTask<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction,
            BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public UniTask<BigInteger> BalanceOfQueryAsync(string owner, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.Owner = owner;

            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public UniTask<string> TransferRequestAsync(TransferFunction transferFunction)
        {
            return ContractHandler.SendRequestAsync(transferFunction);
        }

        public UniTask<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction,
            CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public UniTask<string> TransferRequestAsync(string to, BigInteger value)
        {
            var transferFunction = new TransferFunction();
            transferFunction.To = to;
            transferFunction.Value = value;

            return ContractHandler.SendRequestAsync(transferFunction);
        }

        public UniTask<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string to, BigInteger value,
            CancellationToken cancellationToken = default)
        {
            var transferFunction = new TransferFunction();
            transferFunction.To = to;
            transferFunction.Value = value;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public UniTask<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction,
            BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public UniTask<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
            allowanceFunction.Owner = owner;
            allowanceFunction.Spender = spender;

            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }
#endif
    }
}