using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ERC1155.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ERC1155
{
    public class ERC1155ContractService
    {
        public string ContractAddress { get; }
        public ContractHandler ContractHandler { get; }

        public ERC1155ContractService(IEthApiContractService ethApiContractService, string contractAddress)
        {
            ContractAddress = contractAddress;
#if !DOTNET35
            ContractHandler = ethApiContractService.GetContractHandler(contractAddress);
#endif
        }

#if !DOTNET35

        public UniTask<bool> SupportsErc1155InterfaceQueryAsync()
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
            supportsInterfaceFunction.InterfaceId = "0xd9b67a26".HexToByteArray();
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction);
        }


        public UniTask<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }


        public UniTask<BigInteger> BalanceOfQueryAsync(string account, BigInteger id, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.Account = account;
            balanceOfFunction.Id = id;

            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public UniTask<List<BigInteger>> BalanceOfBatchQueryAsync(BalanceOfBatchFunction balanceOfBatchFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }


        public UniTask<List<BigInteger>> BalanceOfBatchQueryAsync(List<string> accounts, List<BigInteger> ids, BlockParameter blockParameter = null)
        {
            var balanceOfBatchFunction = new BalanceOfBatchFunction();
            balanceOfBatchFunction.Accounts = accounts;
            balanceOfBatchFunction.Ids = ids;

            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        public UniTask<string> BurnRequestAsync(BurnFunction burnFunction)
        {
            return ContractHandler.SendRequestAsync(burnFunction);
        }

        public UniTask<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BurnFunction burnFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public UniTask<string> BurnRequestAsync(string account, BigInteger id, BigInteger value)
        {
            var burnFunction = new BurnFunction();
            burnFunction.Account = account;
            burnFunction.Id = id;
            burnFunction.Value = value;

            return ContractHandler.SendRequestAsync(burnFunction);
        }

        public UniTask<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(string account, BigInteger id, BigInteger value, CancellationToken cancellationToken = default)
        {
            var burnFunction = new BurnFunction();
            burnFunction.Account = account;
            burnFunction.Id = id;
            burnFunction.Value = value;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public UniTask<string> BurnBatchRequestAsync(BurnBatchFunction burnBatchFunction)
        {
            return ContractHandler.SendRequestAsync(burnBatchFunction);
        }

        public UniTask<TransactionReceipt> BurnBatchRequestAndWaitForReceiptAsync(BurnBatchFunction burnBatchFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(burnBatchFunction, cancellationToken);
        }

        public UniTask<string> BurnBatchRequestAsync(string account, List<BigInteger> ids, List<BigInteger> values)
        {
            var burnBatchFunction = new BurnBatchFunction();
            burnBatchFunction.Account = account;
            burnBatchFunction.Ids = ids;
            burnBatchFunction.Values = values;

            return ContractHandler.SendRequestAsync(burnBatchFunction);
        }

        public UniTask<TransactionReceipt> BurnBatchRequestAndWaitForReceiptAsync(string account, List<BigInteger> ids, List<BigInteger> values, CancellationToken cancellationToken = default)
        {
            var burnBatchFunction = new BurnBatchFunction();
            burnBatchFunction.Account = account;
            burnBatchFunction.Ids = ids;
            burnBatchFunction.Values = values;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(burnBatchFunction, cancellationToken);
        }

        public UniTask<bool> ExistsQueryAsync(ExistsFunction existsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ExistsFunction, bool>(existsFunction, blockParameter);
        }


        public UniTask<bool> ExistsQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var existsFunction = new ExistsFunction();
            existsFunction.Id = id;

            return ContractHandler.QueryAsync<ExistsFunction, bool>(existsFunction, blockParameter);
        }

        public UniTask<bool> IsApprovedForAllQueryAsync(IsApprovedForAllFunction isApprovedForAllFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }


        public UniTask<bool> IsApprovedForAllQueryAsync(string account, string @operator, BlockParameter blockParameter = null)
        {
            var isApprovedForAllFunction = new IsApprovedForAllFunction();
            isApprovedForAllFunction.Account = account;
            isApprovedForAllFunction.Operator = @operator;

            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        public UniTask<string> MintRequestAsync(MintFunction mintFunction)
        {
            return ContractHandler.SendRequestAsync(mintFunction);
        }

        public UniTask<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public UniTask<string> MintRequestAsync(string account, BigInteger id, BigInteger amount, byte[] data)
        {
            var mintFunction = new MintFunction();
            mintFunction.Account = account;
            mintFunction.Id = id;
            mintFunction.Amount = amount;
            mintFunction.Data = data;

            return ContractHandler.SendRequestAsync(mintFunction);
        }

        public UniTask<TransactionReceipt> MintRequestAndWaitForReceiptAsync(string account, BigInteger id, BigInteger amount, byte[] data, CancellationToken cancellationToken = default)
        {
            var mintFunction = new MintFunction();
            mintFunction.Account = account;
            mintFunction.Id = id;
            mintFunction.Amount = amount;
            mintFunction.Data = data;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }
        
        public UniTask<string> MintBatchRequestAsync(MintBatchFunction mintBatchFunction)
        {
            return ContractHandler.SendRequestAsync(mintBatchFunction);
        }

        public UniTask<TransactionReceipt> MintBatchRequestAndWaitForReceiptAsync(MintBatchFunction mintBatchFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(mintBatchFunction, cancellationToken);
        }

        public UniTask<string> MintBatchRequestAsync(string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data)
        {
            var mintBatchFunction = new MintBatchFunction();
            mintBatchFunction.To = to;
            mintBatchFunction.Ids = ids;
            mintBatchFunction.Amounts = amounts;
            mintBatchFunction.Data = data;

            return ContractHandler.SendRequestAsync(mintBatchFunction);
        }

        public UniTask<TransactionReceipt> MintBatchRequestAndWaitForReceiptAsync(string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data, CancellationToken cancellationToken = default)
        {
            var mintBatchFunction = new MintBatchFunction();
            mintBatchFunction.To = to;
            mintBatchFunction.Ids = ids;
            mintBatchFunction.Amounts = amounts;
            mintBatchFunction.Data = data;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(mintBatchFunction, cancellationToken);
        }

        public UniTask<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }


        public UniTask<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public UniTask<string> PauseRequestAsync(PauseFunction pauseFunction)
        {
            return ContractHandler.SendRequestAsync(pauseFunction);
        }

        public UniTask<string> PauseRequestAsync()
        {
            return ContractHandler.SendRequestAsync<PauseFunction>();
        }

        public UniTask<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(PauseFunction pauseFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<PauseFunction>(null, cancellationToken);
        }

        public UniTask<bool> PausedQueryAsync(PausedFunction pausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(pausedFunction, blockParameter);
        }


        public UniTask<bool> PausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(null, blockParameter);
        }

        public UniTask<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
            return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public UniTask<string> RenounceOwnershipRequestAsync()
        {
            return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public UniTask<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public UniTask<string> SafeBatchTransferFromRequestAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction)
        {
            return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public UniTask<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public UniTask<string> SafeBatchTransferFromRequestAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
            safeBatchTransferFromFunction.From = from;
            safeBatchTransferFromFunction.To = to;
            safeBatchTransferFromFunction.Ids = ids;
            safeBatchTransferFromFunction.Amounts = amounts;
            safeBatchTransferFromFunction.Data = data;

            return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public UniTask<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data, CancellationToken cancellationToken = default)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
            safeBatchTransferFromFunction.From = from;
            safeBatchTransferFromFunction.To = to;
            safeBatchTransferFromFunction.Ids = ids;
            safeBatchTransferFromFunction.Amounts = amounts;
            safeBatchTransferFromFunction.Data = data;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public UniTask<string> SafeTransferFromRequestAsync(SafeTransferFromFunction safeTransferFromFunction)
        {
            return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public UniTask<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(SafeTransferFromFunction safeTransferFromFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public UniTask<string> SafeTransferFromRequestAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
            safeTransferFromFunction.From = from;
            safeTransferFromFunction.To = to;
            safeTransferFromFunction.Id = id;
            safeTransferFromFunction.Amount = amount;
            safeTransferFromFunction.Data = data;

            return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public UniTask<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data, CancellationToken cancellationToken = default)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
            safeTransferFromFunction.From = from;
            safeTransferFromFunction.To = to;
            safeTransferFromFunction.Id = id;
            safeTransferFromFunction.Amount = amount;
            safeTransferFromFunction.Data = data;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public UniTask<string> SetApprovalForAllRequestAsync(SetApprovalForAllFunction setApprovalForAllFunction)
        {
            return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public UniTask<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(SetApprovalForAllFunction setApprovalForAllFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public UniTask<string> SetApprovalForAllRequestAsync(string @operator, bool approved)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
            setApprovalForAllFunction.Operator = @operator;
            setApprovalForAllFunction.Approved = approved;

            return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public UniTask<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(string @operator, bool approved, CancellationToken cancellationToken = default)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
            setApprovalForAllFunction.Operator = @operator;
            setApprovalForAllFunction.Approved = approved;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public UniTask<string> SetTokenUriRequestAsync(SetTokenUriFunction setTokenUriFunction)
        {
            return ContractHandler.SendRequestAsync(setTokenUriFunction);
        }

        public UniTask<TransactionReceipt> SetTokenUriRequestAndWaitForReceiptAsync(SetTokenUriFunction setTokenUriFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setTokenUriFunction, cancellationToken);
        }

        public UniTask<string> SetTokenUriRequestAsync(BigInteger tokenId, string tokenURI)
        {
            var setTokenUriFunction = new SetTokenUriFunction();
            setTokenUriFunction.TokenId = tokenId;
            setTokenUriFunction.TokenURI = tokenURI;

            return ContractHandler.SendRequestAsync(setTokenUriFunction);
        }

        public UniTask<TransactionReceipt> SetTokenUriRequestAndWaitForReceiptAsync(BigInteger tokenId, string tokenURI, CancellationToken cancellationToken = default)
        {
            var setTokenUriFunction = new SetTokenUriFunction();
            setTokenUriFunction.TokenId = tokenId;
            setTokenUriFunction.TokenURI = tokenURI;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setTokenUriFunction, cancellationToken);
        }

        public UniTask<string> SetURIRequestAsync(SetURIFunction setURIFunction)
        {
            return ContractHandler.SendRequestAsync(setURIFunction);
        }

        public UniTask<TransactionReceipt> SetURIRequestAndWaitForReceiptAsync(SetURIFunction setURIFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setURIFunction, cancellationToken);
        }

        public UniTask<string> SetURIRequestAsync(string newuri)
        {
            var setURIFunction = new SetURIFunction();
            setURIFunction.Newuri = newuri;

            return ContractHandler.SendRequestAsync(setURIFunction);
        }

        public UniTask<TransactionReceipt> SetURIRequestAndWaitForReceiptAsync(string newuri, CancellationToken cancellationToken = default)
        {
            var setURIFunction = new SetURIFunction();
            setURIFunction.Newuri = newuri;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setURIFunction, cancellationToken);
        }

        public UniTask<bool> SupportsInterfaceQueryAsync(SupportsInterfaceFunction supportsInterfaceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }


        public UniTask<bool> SupportsInterfaceQueryAsync(byte[] interfaceId, BlockParameter blockParameter = null)
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
            supportsInterfaceFunction.InterfaceId = interfaceId;

            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }



        public UniTask<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }


        public UniTask<BigInteger> TotalSupplyQueryAsync(BigInteger id, BlockParameter blockParameter = null)
        {
            var totalSupplyFunction = new TotalSupplyFunction();
            totalSupplyFunction.Id = id;

            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        public UniTask<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
            return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public UniTask<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public UniTask<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;

            return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public UniTask<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationToken cancellationToken = default)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public UniTask<string> UnpauseRequestAsync(UnpauseFunction unpauseFunction)
        {
            return ContractHandler.SendRequestAsync(unpauseFunction);
        }

        public UniTask<string> UnpauseRequestAsync()
        {
            return ContractHandler.SendRequestAsync<UnpauseFunction>();
        }

        public UniTask<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(UnpauseFunction unpauseFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(unpauseFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<UnpauseFunction>(null, cancellationToken);
        }

        public UniTask<string> UriQueryAsync(UriFunction uriFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }


        public UniTask<string> UriQueryAsync(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var uriFunction = new UriFunction();
            uriFunction.TokenId = tokenId;

            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }

#endif
    }
}