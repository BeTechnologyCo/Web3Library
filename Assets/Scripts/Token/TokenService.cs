using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using InfernalTower.Contracts.Token.ContractDefinition;

namespace InfernalTower.Contracts.Token
{
    public partial class TokenService
    {
        public static UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TokenDeployment tokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TokenDeployment>().SendRequestAndWaitForReceiptAsync(tokenDeployment, cancellationTokenSource);
        }

        public static UniTask<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TokenDeployment tokenDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TokenDeployment>().SendRequestAsync(tokenDeployment);
        }

        public static async UniTask<TokenService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TokenDeployment tokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, tokenDeployment, cancellationTokenSource);
            return new TokenService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TokenService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public UniTask<byte[]> DEFAULT_ADMIN_ROLEQueryAsync(DEFAULT_ADMIN_ROLEFunction dEFAULT_ADMIN_ROLEFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DEFAULT_ADMIN_ROLEFunction, byte[]>(dEFAULT_ADMIN_ROLEFunction, blockParameter);
        }

        
        public UniTask<byte[]> DEFAULT_ADMIN_ROLEQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DEFAULT_ADMIN_ROLEFunction, byte[]>(null, blockParameter);
        }

        public UniTask<byte[]> MINTER_ROLEQueryAsync(MINTER_ROLEFunction mINTER_ROLEFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MINTER_ROLEFunction, byte[]>(mINTER_ROLEFunction, blockParameter);
        }

        
        public UniTask<byte[]> MINTER_ROLEQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MINTER_ROLEFunction, byte[]>(null, blockParameter);
        }

        public UniTask<byte[]> PAUSER_ROLEQueryAsync(PAUSER_ROLEFunction pAUSER_ROLEFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PAUSER_ROLEFunction, byte[]>(pAUSER_ROLEFunction, blockParameter);
        }

        
        public UniTask<byte[]> PAUSER_ROLEQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PAUSER_ROLEFunction, byte[]>(null, blockParameter);
        }

        public UniTask<BigInteger> SellTaxQueryAsync(SellTaxFunction sellTaxFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SellTaxFunction, BigInteger>(sellTaxFunction, blockParameter);
        }

        
        public UniTask<BigInteger> SellTaxQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SellTaxFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> SwapV2LiquidityPairQueryAsync(SwapV2LiquidityPairFunction swapV2LiquidityPairFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SwapV2LiquidityPairFunction, string>(swapV2LiquidityPairFunction, blockParameter);
        }

        
        public UniTask<string> SwapV2LiquidityPairQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SwapV2LiquidityPairFunction, string>(null, blockParameter);
        }

        public UniTask<string> SwapV2RouterQueryAsync(SwapV2RouterFunction swapV2RouterFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SwapV2RouterFunction, string>(swapV2RouterFunction, blockParameter);
        }

        
        public UniTask<string> SwapV2RouterQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SwapV2RouterFunction, string>(null, blockParameter);
        }

        public UniTask<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
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

        public UniTask<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public UniTask<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public UniTask<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public UniTask<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public UniTask<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public UniTask<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public UniTask<string> BurnRequestAsync(BurnFunction burnFunction)
        {
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public UniTask<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BurnFunction burnFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public UniTask<string> BurnRequestAsync(BigInteger amount)
        {
            var burnFunction = new BurnFunction();
                burnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public UniTask<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var burnFunction = new BurnFunction();
                burnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public UniTask<string> BurnFromRequestAsync(BurnFromFunction burnFromFunction)
        {
             return ContractHandler.SendRequestAsync(burnFromFunction);
        }

        public UniTask<TransactionReceipt> BurnFromRequestAndWaitForReceiptAsync(BurnFromFunction burnFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFromFunction, cancellationToken);
        }

        public UniTask<string> BurnFromRequestAsync(string account, BigInteger amount)
        {
            var burnFromFunction = new BurnFromFunction();
                burnFromFunction.Account = account;
                burnFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(burnFromFunction);
        }

        public UniTask<TransactionReceipt> BurnFromRequestAndWaitForReceiptAsync(string account, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var burnFromFunction = new BurnFromFunction();
                burnFromFunction.Account = account;
                burnFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFromFunction, cancellationToken);
        }

        public UniTask<string> ChangeRouterRequestAsync(ChangeRouterFunction changeRouterFunction)
        {
             return ContractHandler.SendRequestAsync(changeRouterFunction);
        }

        public UniTask<TransactionReceipt> ChangeRouterRequestAndWaitForReceiptAsync(ChangeRouterFunction changeRouterFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(changeRouterFunction, cancellationToken);
        }

        public UniTask<string> ChangeRouterRequestAsync(string router)
        {
            var changeRouterFunction = new ChangeRouterFunction();
                changeRouterFunction.Router = router;
            
             return ContractHandler.SendRequestAsync(changeRouterFunction);
        }

        public UniTask<TransactionReceipt> ChangeRouterRequestAndWaitForReceiptAsync(string router, CancellationTokenSource cancellationToken = null)
        {
            var changeRouterFunction = new ChangeRouterFunction();
                changeRouterFunction.Router = router;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(changeRouterFunction, cancellationToken);
        }

        public UniTask<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        
        public UniTask<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public UniTask<string> DecreaseAllowanceRequestAsync(DecreaseAllowanceFunction decreaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public UniTask<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(DecreaseAllowanceFunction decreaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public UniTask<string> DecreaseAllowanceRequestAsync(string spender, BigInteger subtractedValue)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public UniTask<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger subtractedValue, CancellationTokenSource cancellationToken = null)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public UniTask<string> ExcludeFromFeeRequestAsync(ExcludeFromFeeFunction excludeFromFeeFunction)
        {
             return ContractHandler.SendRequestAsync(excludeFromFeeFunction);
        }

        public UniTask<TransactionReceipt> ExcludeFromFeeRequestAndWaitForReceiptAsync(ExcludeFromFeeFunction excludeFromFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromFeeFunction, cancellationToken);
        }

        public UniTask<string> ExcludeFromFeeRequestAsync(string account)
        {
            var excludeFromFeeFunction = new ExcludeFromFeeFunction();
                excludeFromFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(excludeFromFeeFunction);
        }

        public UniTask<TransactionReceipt> ExcludeFromFeeRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var excludeFromFeeFunction = new ExcludeFromFeeFunction();
                excludeFromFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromFeeFunction, cancellationToken);
        }

        public UniTask<byte[]> GetRoleAdminQueryAsync(GetRoleAdminFunction getRoleAdminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRoleAdminFunction, byte[]>(getRoleAdminFunction, blockParameter);
        }

        
        public UniTask<byte[]> GetRoleAdminQueryAsync(byte[] role, BlockParameter blockParameter = null)
        {
            var getRoleAdminFunction = new GetRoleAdminFunction();
                getRoleAdminFunction.Role = role;
            
            return ContractHandler.QueryAsync<GetRoleAdminFunction, byte[]>(getRoleAdminFunction, blockParameter);
        }

        public UniTask<string> GetRoleMemberQueryAsync(GetRoleMemberFunction getRoleMemberFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRoleMemberFunction, string>(getRoleMemberFunction, blockParameter);
        }

        
        public UniTask<string> GetRoleMemberQueryAsync(byte[] role, BigInteger index, BlockParameter blockParameter = null)
        {
            var getRoleMemberFunction = new GetRoleMemberFunction();
                getRoleMemberFunction.Role = role;
                getRoleMemberFunction.Index = index;
            
            return ContractHandler.QueryAsync<GetRoleMemberFunction, string>(getRoleMemberFunction, blockParameter);
        }

        public UniTask<BigInteger> GetRoleMemberCountQueryAsync(GetRoleMemberCountFunction getRoleMemberCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRoleMemberCountFunction, BigInteger>(getRoleMemberCountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> GetRoleMemberCountQueryAsync(byte[] role, BlockParameter blockParameter = null)
        {
            var getRoleMemberCountFunction = new GetRoleMemberCountFunction();
                getRoleMemberCountFunction.Role = role;
            
            return ContractHandler.QueryAsync<GetRoleMemberCountFunction, BigInteger>(getRoleMemberCountFunction, blockParameter);
        }

        public UniTask<string> GrantRoleRequestAsync(GrantRoleFunction grantRoleFunction)
        {
             return ContractHandler.SendRequestAsync(grantRoleFunction);
        }

        public UniTask<TransactionReceipt> GrantRoleRequestAndWaitForReceiptAsync(GrantRoleFunction grantRoleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(grantRoleFunction, cancellationToken);
        }

        public UniTask<string> GrantRoleRequestAsync(byte[] role, string account)
        {
            var grantRoleFunction = new GrantRoleFunction();
                grantRoleFunction.Role = role;
                grantRoleFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(grantRoleFunction);
        }

        public UniTask<TransactionReceipt> GrantRoleRequestAndWaitForReceiptAsync(byte[] role, string account, CancellationTokenSource cancellationToken = null)
        {
            var grantRoleFunction = new GrantRoleFunction();
                grantRoleFunction.Role = role;
                grantRoleFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(grantRoleFunction, cancellationToken);
        }

        public UniTask<bool> HasRoleQueryAsync(HasRoleFunction hasRoleFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HasRoleFunction, bool>(hasRoleFunction, blockParameter);
        }

        
        public UniTask<bool> HasRoleQueryAsync(byte[] role, string account, BlockParameter blockParameter = null)
        {
            var hasRoleFunction = new HasRoleFunction();
                hasRoleFunction.Role = role;
                hasRoleFunction.Account = account;
            
            return ContractHandler.QueryAsync<HasRoleFunction, bool>(hasRoleFunction, blockParameter);
        }

        public UniTask<string> IncludeInFeeRequestAsync(IncludeInFeeFunction includeInFeeFunction)
        {
             return ContractHandler.SendRequestAsync(includeInFeeFunction);
        }

        public UniTask<TransactionReceipt> IncludeInFeeRequestAndWaitForReceiptAsync(IncludeInFeeFunction includeInFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInFeeFunction, cancellationToken);
        }

        public UniTask<string> IncludeInFeeRequestAsync(string account)
        {
            var includeInFeeFunction = new IncludeInFeeFunction();
                includeInFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(includeInFeeFunction);
        }

        public UniTask<TransactionReceipt> IncludeInFeeRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var includeInFeeFunction = new IncludeInFeeFunction();
                includeInFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInFeeFunction, cancellationToken);
        }

        public UniTask<string> IncreaseAllowanceRequestAsync(IncreaseAllowanceFunction increaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public UniTask<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(IncreaseAllowanceFunction increaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public UniTask<string> IncreaseAllowanceRequestAsync(string spender, BigInteger addedValue)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public UniTask<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger addedValue, CancellationTokenSource cancellationToken = null)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public UniTask<string> MintRequestAsync(MintFunction mintFunction)
        {
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public UniTask<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public UniTask<string> MintRequestAsync(string to, BigInteger amount)
        {
            var mintFunction = new MintFunction();
                mintFunction.To = to;
                mintFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public UniTask<TransactionReceipt> MintRequestAndWaitForReceiptAsync(string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var mintFunction = new MintFunction();
                mintFunction.To = to;
                mintFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public UniTask<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public UniTask<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public UniTask<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public UniTask<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
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

        public UniTask<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public UniTask<string> RenounceRoleRequestAsync(RenounceRoleFunction renounceRoleFunction)
        {
             return ContractHandler.SendRequestAsync(renounceRoleFunction);
        }

        public UniTask<TransactionReceipt> RenounceRoleRequestAndWaitForReceiptAsync(RenounceRoleFunction renounceRoleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceRoleFunction, cancellationToken);
        }

        public UniTask<string> RenounceRoleRequestAsync(byte[] role, string account)
        {
            var renounceRoleFunction = new RenounceRoleFunction();
                renounceRoleFunction.Role = role;
                renounceRoleFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(renounceRoleFunction);
        }

        public UniTask<TransactionReceipt> RenounceRoleRequestAndWaitForReceiptAsync(byte[] role, string account, CancellationTokenSource cancellationToken = null)
        {
            var renounceRoleFunction = new RenounceRoleFunction();
                renounceRoleFunction.Role = role;
                renounceRoleFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceRoleFunction, cancellationToken);
        }

        public UniTask<string> RevokeRoleRequestAsync(RevokeRoleFunction revokeRoleFunction)
        {
             return ContractHandler.SendRequestAsync(revokeRoleFunction);
        }

        public UniTask<TransactionReceipt> RevokeRoleRequestAndWaitForReceiptAsync(RevokeRoleFunction revokeRoleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(revokeRoleFunction, cancellationToken);
        }

        public UniTask<string> RevokeRoleRequestAsync(byte[] role, string account)
        {
            var revokeRoleFunction = new RevokeRoleFunction();
                revokeRoleFunction.Role = role;
                revokeRoleFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(revokeRoleFunction);
        }

        public UniTask<TransactionReceipt> RevokeRoleRequestAndWaitForReceiptAsync(byte[] role, string account, CancellationTokenSource cancellationToken = null)
        {
            var revokeRoleFunction = new RevokeRoleFunction();
                revokeRoleFunction.Role = role;
                revokeRoleFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(revokeRoleFunction, cancellationToken);
        }

        public UniTask<string> SetTaxRequestAsync(SetTaxFunction setTaxFunction)
        {
             return ContractHandler.SendRequestAsync(setTaxFunction);
        }

        public UniTask<TransactionReceipt> SetTaxRequestAndWaitForReceiptAsync(SetTaxFunction setTaxFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTaxFunction, cancellationToken);
        }

        public UniTask<string> SetTaxRequestAsync(BigInteger tax)
        {
            var setTaxFunction = new SetTaxFunction();
                setTaxFunction.Tax = tax;
            
             return ContractHandler.SendRequestAsync(setTaxFunction);
        }

        public UniTask<TransactionReceipt> SetTaxRequestAndWaitForReceiptAsync(BigInteger tax, CancellationTokenSource cancellationToken = null)
        {
            var setTaxFunction = new SetTaxFunction();
                setTaxFunction.Tax = tax;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTaxFunction, cancellationToken);
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

        public UniTask<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public UniTask<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public UniTask<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public UniTask<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public UniTask<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public UniTask<string> TransferRequestAsync(string to, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
                transferFunction.To = to;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public UniTask<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.To = to;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public UniTask<string> TransferAndApproveRequestAsync(TransferAndApproveFunction transferAndApproveFunction)
        {
             return ContractHandler.SendRequestAsync(transferAndApproveFunction);
        }

        public UniTask<TransactionReceipt> TransferAndApproveRequestAndWaitForReceiptAsync(TransferAndApproveFunction transferAndApproveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferAndApproveFunction, cancellationToken);
        }

        public UniTask<string> TransferAndApproveRequestAsync(string token)
        {
            var transferAndApproveFunction = new TransferAndApproveFunction();
                transferAndApproveFunction.Token = token;
            
             return ContractHandler.SendRequestAsync(transferAndApproveFunction);
        }

        public UniTask<TransactionReceipt> TransferAndApproveRequestAndWaitForReceiptAsync(string token, CancellationTokenSource cancellationToken = null)
        {
            var transferAndApproveFunction = new TransferAndApproveFunction();
                transferAndApproveFunction.Token = token;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferAndApproveFunction, cancellationToken);
        }

        public UniTask<string> TransferAndApproveAmountRequestAsync(TransferAndApproveAmountFunction transferAndApproveAmountFunction)
        {
             return ContractHandler.SendRequestAsync(transferAndApproveAmountFunction);
        }

        public UniTask<TransactionReceipt> TransferAndApproveAmountRequestAndWaitForReceiptAsync(TransferAndApproveAmountFunction transferAndApproveAmountFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferAndApproveAmountFunction, cancellationToken);
        }

        public UniTask<string> TransferAndApproveAmountRequestAsync(string token, BigInteger amount)
        {
            var transferAndApproveAmountFunction = new TransferAndApproveAmountFunction();
                transferAndApproveAmountFunction.Token = token;
                transferAndApproveAmountFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferAndApproveAmountFunction);
        }

        public UniTask<TransactionReceipt> TransferAndApproveAmountRequestAndWaitForReceiptAsync(string token, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferAndApproveAmountFunction = new TransferAndApproveAmountFunction();
                transferAndApproveAmountFunction.Token = token;
                transferAndApproveAmountFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferAndApproveAmountFunction, cancellationToken);
        }

        public UniTask<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public UniTask<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public UniTask<string> TransferFromRequestAsync(string from, string to, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.From = from;
                transferFromFunction.To = to;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public UniTask<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.From = from;
                transferFromFunction.To = to;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public UniTask<string> TransferFromAndApproveRequestAsync(TransferFromAndApproveFunction transferFromAndApproveFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromAndApproveFunction);
        }

        public UniTask<TransactionReceipt> TransferFromAndApproveRequestAndWaitForReceiptAsync(TransferFromAndApproveFunction transferFromAndApproveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromAndApproveFunction, cancellationToken);
        }

        public UniTask<string> TransferFromAndApproveRequestAsync(string token)
        {
            var transferFromAndApproveFunction = new TransferFromAndApproveFunction();
                transferFromAndApproveFunction.Token = token;
            
             return ContractHandler.SendRequestAsync(transferFromAndApproveFunction);
        }

        public UniTask<TransactionReceipt> TransferFromAndApproveRequestAndWaitForReceiptAsync(string token, CancellationTokenSource cancellationToken = null)
        {
            var transferFromAndApproveFunction = new TransferFromAndApproveFunction();
                transferFromAndApproveFunction.Token = token;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromAndApproveFunction, cancellationToken);
        }

        public UniTask<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public UniTask<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public UniTask<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public UniTask<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public UniTask<string> WithdrawRequestAsync(WithdrawFunction withdrawFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public UniTask<string> WithdrawRequestAsync()
        {
             return ContractHandler.SendRequestAsync<WithdrawFunction>();
        }

        public UniTask<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(WithdrawFunction withdrawFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<WithdrawFunction>(null, cancellationToken);
        }

        public UniTask<string> WithdrawAmountToRequestAsync(WithdrawAmountToFunction withdrawAmountToFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawAmountToFunction);
        }

        public UniTask<TransactionReceipt> WithdrawAmountToRequestAndWaitForReceiptAsync(WithdrawAmountToFunction withdrawAmountToFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawAmountToFunction, cancellationToken);
        }

        public UniTask<string> WithdrawAmountToRequestAsync(string to, BigInteger amount)
        {
            var withdrawAmountToFunction = new WithdrawAmountToFunction();
                withdrawAmountToFunction.To = to;
                withdrawAmountToFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(withdrawAmountToFunction);
        }

        public UniTask<TransactionReceipt> WithdrawAmountToRequestAndWaitForReceiptAsync(string to, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var withdrawAmountToFunction = new WithdrawAmountToFunction();
                withdrawAmountToFunction.To = to;
                withdrawAmountToFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawAmountToFunction, cancellationToken);
        }

        public UniTask<string> WithdrawToRequestAsync(WithdrawToFunction withdrawToFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawToFunction);
        }

        public UniTask<TransactionReceipt> WithdrawToRequestAndWaitForReceiptAsync(WithdrawToFunction withdrawToFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawToFunction, cancellationToken);
        }

        public UniTask<string> WithdrawToRequestAsync(string to)
        {
            var withdrawToFunction = new WithdrawToFunction();
                withdrawToFunction.To = to;
            
             return ContractHandler.SendRequestAsync(withdrawToFunction);
        }

        public UniTask<TransactionReceipt> WithdrawToRequestAndWaitForReceiptAsync(string to, CancellationTokenSource cancellationToken = null)
        {
            var withdrawToFunction = new WithdrawToFunction();
                withdrawToFunction.To = to;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawToFunction, cancellationToken);
        }
    }
}
