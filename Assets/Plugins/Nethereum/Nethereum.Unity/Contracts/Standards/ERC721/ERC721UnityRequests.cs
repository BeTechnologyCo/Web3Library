using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Unity.Contracts;
using Newtonsoft.Json;
using Nethereum.Contracts.Standards.ERC721.ContractDefinition;

namespace Nethereum.Unity.Contracts.Standards.ERC721
{
   
    public partial class ApproveTransactionRequest : ContractFunctionTransactionRequest<ApproveFunction>
    {

        public ApproveTransactionRequest(IContractTransactionUnityRequestFactory contractTransactionUnityRequestFactory, string contractAddress) : base(contractTransactionUnityRequestFactory, contractAddress)
        {
        }

        public ApproveTransactionRequest(string url, BigInteger chainId, string privateKey, string contractAddress, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, chainId, privateKey, contractAddress, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator SignAndSendTransaction(string to, BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.To = to;
                approveFunction.TokenId = tokenId;
            yield return SignAndSendTransaction(approveFunction);
        }

    }

    public partial class BalanceOfQueryRequest : ContractFunctionQueryRequest<BalanceOfFunction, BalanceOfOutputDTO>
    {

        public BalanceOfQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public BalanceOfQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(string owner, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Owner = owner;
            yield return Query(balanceOfFunction, blockParameter);
        }

    }

    public partial class GetApprovedQueryRequest : ContractFunctionQueryRequest<GetApprovedFunction, GetApprovedOutputDTO>
    {

        public GetApprovedQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public GetApprovedQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var getApprovedFunction = new GetApprovedFunction();
                getApprovedFunction.TokenId = tokenId;
            yield return Query(getApprovedFunction, blockParameter);
        }

    }

   

  

    public partial class IsApprovedForAllQueryRequest : ContractFunctionQueryRequest<IsApprovedForAllFunction, IsApprovedForAllOutputDTO>
    {

        public IsApprovedForAllQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public IsApprovedForAllQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(string owner, string @operator, BlockParameter blockParameter = null)
        {
            var isApprovedForAllFunction = new IsApprovedForAllFunction();
                isApprovedForAllFunction.Owner = owner;
                isApprovedForAllFunction.Operator = @operator;
            yield return Query(isApprovedForAllFunction, blockParameter);
        }

    }

   

    public partial class NameQueryRequest : ContractFunctionQueryRequest<NameFunction, NameOutputDTO>
    {

        public NameQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public NameQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BlockParameter blockParameter = null)
        {
            var nameFunction = new NameFunction();
            yield return Query(nameFunction, blockParameter);
        }

    }

    public partial class OwnerOfQueryRequest : ContractFunctionQueryRequest<OwnerOfFunction, OwnerOfOutputDTO>
    {

        public OwnerOfQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public OwnerOfQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var ownerOfFunction = new OwnerOfFunction();
                ownerOfFunction.TokenId = tokenId;
            yield return Query(ownerOfFunction, blockParameter);
        }

    }

  

 

    public partial class SafeTransferFromTransactionRequest : ContractFunctionTransactionRequest<SafeTransferFromFunction>
    {

        public SafeTransferFromTransactionRequest(IContractTransactionUnityRequestFactory contractTransactionUnityRequestFactory, string contractAddress) : base(contractTransactionUnityRequestFactory, contractAddress)
        {
        }

        public SafeTransferFromTransactionRequest(string url, BigInteger chainId, string privateKey, string contractAddress, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, chainId, privateKey, contractAddress, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator SignAndSendTransaction(string from, string to, BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.TokenId = tokenId;
            yield return SignAndSendTransaction(safeTransferFromFunction);
        }

    }

    public partial class SafeTransferFrom1TransactionRequest : ContractFunctionTransactionRequest<SafeTransferFrom1Function>
    {

        public SafeTransferFrom1TransactionRequest(IContractTransactionUnityRequestFactory contractTransactionUnityRequestFactory, string contractAddress) : base(contractTransactionUnityRequestFactory, contractAddress)
        {
        }

        public SafeTransferFrom1TransactionRequest(string url, BigInteger chainId, string privateKey, string contractAddress, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, chainId, privateKey, contractAddress, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator SignAndSendTransaction(string from, string to, BigInteger tokenId, byte[] data, BlockParameter blockParameter = null)
        {
            var safeTransferFrom1Function = new SafeTransferFrom1Function();
                safeTransferFrom1Function.From = from;
                safeTransferFrom1Function.To = to;
                safeTransferFrom1Function.TokenId = tokenId;
                safeTransferFrom1Function.Data = data;
            yield return SignAndSendTransaction(safeTransferFrom1Function);
        }

    }

    public partial class SetApprovalForAllTransactionRequest : ContractFunctionTransactionRequest<SetApprovalForAllFunction>
    {

        public SetApprovalForAllTransactionRequest(IContractTransactionUnityRequestFactory contractTransactionUnityRequestFactory, string contractAddress) : base(contractTransactionUnityRequestFactory, contractAddress)
        {
        }

        public SetApprovalForAllTransactionRequest(string url, BigInteger chainId, string privateKey, string contractAddress, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, chainId, privateKey, contractAddress, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator SignAndSendTransaction(string @operator, bool approved, BlockParameter blockParameter = null)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            yield return SignAndSendTransaction(setApprovalForAllFunction);
        }

    }

    public partial class SupportsInterfaceQueryRequest : ContractFunctionQueryRequest<SupportsInterfaceFunction, SupportsInterfaceOutputDTO>
    {

        public SupportsInterfaceQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public SupportsInterfaceQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(byte[] interfaceId, BlockParameter blockParameter = null)
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
                supportsInterfaceFunction.InterfaceId = interfaceId;
            yield return Query(supportsInterfaceFunction, blockParameter);
        }

    }

    public partial class SymbolQueryRequest : ContractFunctionQueryRequest<SymbolFunction, SymbolOutputDTO>
    {

        public SymbolQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public SymbolQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BlockParameter blockParameter = null)
        {
            var symbolFunction = new SymbolFunction();
            yield return Query(symbolFunction, blockParameter);
        }

    }

    public partial class TokenByIndexQueryRequest : ContractFunctionQueryRequest<TokenByIndexFunction, TokenByIndexOutputDTO>
    {

        public TokenByIndexQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public TokenByIndexQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BigInteger index, BlockParameter blockParameter = null)
        {
            var tokenByIndexFunction = new TokenByIndexFunction();
                tokenByIndexFunction.Index = index;
            yield return Query(tokenByIndexFunction, blockParameter);
        }

    }

    public partial class TokenOfOwnerByIndexQueryRequest : ContractFunctionQueryRequest<TokenOfOwnerByIndexFunction, TokenOfOwnerByIndexOutputDTO>
    {

        public TokenOfOwnerByIndexQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public TokenOfOwnerByIndexQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(string owner, BigInteger index, BlockParameter blockParameter = null)
        {
            var tokenOfOwnerByIndexFunction = new TokenOfOwnerByIndexFunction();
                tokenOfOwnerByIndexFunction.Owner = owner;
                tokenOfOwnerByIndexFunction.Index = index;
            yield return Query(tokenOfOwnerByIndexFunction, blockParameter);
        }

    }

    public partial class TokenURIQueryRequest : ContractFunctionQueryRequest<TokenURIFunction, TokenURIOutputDTO>
    {

        public TokenURIQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public TokenURIQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var tokenURIFunction = new TokenURIFunction();
                tokenURIFunction.TokenId = tokenId;
            yield return Query(tokenURIFunction, blockParameter);
        }

    }

    public partial class TotalSupplyQueryRequest : ContractFunctionQueryRequest<TotalSupplyFunction, TotalSupplyOutputDTO>
    {

        public TotalSupplyQueryRequest(IContractQueryUnityRequestFactory contractQueryUnityRequestFactory, string contractAddress) : base(contractQueryUnityRequestFactory, contractAddress)
        {
        }

        public TotalSupplyQueryRequest(string url, string contractAddress, string defaultAccount = null, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, contractAddress, defaultAccount, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator Query(BlockParameter blockParameter = null)
        {
            var totalSupplyFunction = new TotalSupplyFunction();
            yield return Query(totalSupplyFunction, blockParameter);
        }

    }

    public partial class TransferFromTransactionRequest : ContractFunctionTransactionRequest<TransferFromFunction>
    {

        public TransferFromTransactionRequest(IContractTransactionUnityRequestFactory contractTransactionUnityRequestFactory, string contractAddress) : base(contractTransactionUnityRequestFactory, contractAddress)
        {
        }

        public TransferFromTransactionRequest(string url, BigInteger chainId, string privateKey, string contractAddress, JsonSerializerSettings jsonSerializerSettings = null, Dictionary<string, string> requestHeaders = null) : base(url, chainId, privateKey, contractAddress, jsonSerializerSettings, requestHeaders)
        {
        }

        public IEnumerator SignAndSendTransaction(string from, string to, BigInteger tokenId, BlockParameter blockParameter = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.From = from;
                transferFromFunction.To = to;
                transferFromFunction.TokenId = tokenId;
            yield return SignAndSendTransaction(transferFromFunction);
        }

    }
}
