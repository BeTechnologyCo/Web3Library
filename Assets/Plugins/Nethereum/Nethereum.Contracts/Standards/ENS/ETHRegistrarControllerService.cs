using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ENS.ETHRegistrarController.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ENS
{
    public partial class ETHRegistrarControllerService
    {
        public string ContractAddress { get; }

        public ContractHandler ContractHandler { get; }

        public ETHRegistrarControllerService(IEthApiContractService ethApiContractService, string contractAddress)
        {
            ContractAddress = contractAddress;
#if !DOTNET35
            ContractHandler = ethApiContractService.GetContractHandler(contractAddress);
#endif
        }
#if !DOTNET35
        public UniTask<BigInteger> MinRegistrationDurationQueryAsync(MinRegistrationDurationFunction minRegistrationDurationFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinRegistrationDurationFunction, BigInteger>(minRegistrationDurationFunction, blockParameter);
        }

        
        public UniTask<BigInteger> MinRegistrationDurationQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinRegistrationDurationFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<bool> AvailableQueryAsync(AvailableFunction availableFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AvailableFunction, bool>(availableFunction, blockParameter);
        }

        
        public UniTask<bool> AvailableQueryAsync(string name, BlockParameter blockParameter = null)
        {
            var availableFunction = new AvailableFunction();
                availableFunction.Name = name;
            
            return ContractHandler.QueryAsync<AvailableFunction, bool>(availableFunction, blockParameter);
        }

        public UniTask<string> CommitRequestAsync(CommitFunction commitFunction)
        {
             return ContractHandler.SendRequestAsync(commitFunction);
        }

        public UniTask<TransactionReceipt> CommitRequestAndWaitForReceiptAsync(CommitFunction commitFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(commitFunction, cancellationToken);
        }

        public UniTask<string> CommitRequestAsync(byte[] commitment)
        {
            var commitFunction = new CommitFunction();
                commitFunction.Commitment = commitment;
            
             return ContractHandler.SendRequestAsync(commitFunction);
        }

        public UniTask<TransactionReceipt> CommitRequestAndWaitForReceiptAsync(byte[] commitment, CancellationToken cancellationToken = default)
        {
            var commitFunction = new CommitFunction();
                commitFunction.Commitment = commitment;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(commitFunction, cancellationToken);
        }

        public UniTask<BigInteger> CommitmentsQueryAsync(CommitmentsFunction commitmentsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CommitmentsFunction, BigInteger>(commitmentsFunction, blockParameter);
        }

        
        public UniTask<BigInteger> CommitmentsQueryAsync(byte[] returnValue1, BlockParameter blockParameter = null)
        {
            var commitmentsFunction = new CommitmentsFunction();
                commitmentsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<CommitmentsFunction, BigInteger>(commitmentsFunction, blockParameter);
        }

        public UniTask<bool> IsOwnerQueryAsync(IsOwnerFunction isOwnerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsOwnerFunction, bool>(isOwnerFunction, blockParameter);
        }

        
        public UniTask<bool> IsOwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsOwnerFunction, bool>(null, blockParameter);
        }

        public UniTask<byte[]> MakeCommitmentQueryAsync(MakeCommitmentFunction makeCommitmentFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MakeCommitmentFunction, byte[]>(makeCommitmentFunction, blockParameter);
        }

        
        public UniTask<byte[]> MakeCommitmentQueryAsync(string name, string owner, byte[] secret, BlockParameter blockParameter = null)
        {
            var makeCommitmentFunction = new MakeCommitmentFunction();
                makeCommitmentFunction.Name = name;
                makeCommitmentFunction.Owner = owner;
                makeCommitmentFunction.Secret = secret;
            
            return ContractHandler.QueryAsync<MakeCommitmentFunction, byte[]>(makeCommitmentFunction, blockParameter);
        }

        public UniTask<byte[]> MakeCommitmentWithConfigQueryAsync(MakeCommitmentWithConfigFunction makeCommitmentWithConfigFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MakeCommitmentWithConfigFunction, byte[]>(makeCommitmentWithConfigFunction, blockParameter);
        }

        
        public UniTask<byte[]> MakeCommitmentWithConfigQueryAsync(string name, string owner, byte[] secret, string resolver, string addr, BlockParameter blockParameter = null)
        {
            var makeCommitmentWithConfigFunction = new MakeCommitmentWithConfigFunction();
                makeCommitmentWithConfigFunction.Name = name;
                makeCommitmentWithConfigFunction.Owner = owner;
                makeCommitmentWithConfigFunction.Secret = secret;
                makeCommitmentWithConfigFunction.Resolver = resolver;
                makeCommitmentWithConfigFunction.Addr = addr;
            
            return ContractHandler.QueryAsync<MakeCommitmentWithConfigFunction, byte[]>(makeCommitmentWithConfigFunction, blockParameter);
        }

        public UniTask<BigInteger> MaxCommitmentAgeQueryAsync(MaxCommitmentAgeFunction maxCommitmentAgeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxCommitmentAgeFunction, BigInteger>(maxCommitmentAgeFunction, blockParameter);
        }

        
        public UniTask<BigInteger> MaxCommitmentAgeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxCommitmentAgeFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<BigInteger> MinCommitmentAgeQueryAsync(MinCommitmentAgeFunction minCommitmentAgeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinCommitmentAgeFunction, BigInteger>(minCommitmentAgeFunction, blockParameter);
        }

        
        public UniTask<BigInteger> MinCommitmentAgeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinCommitmentAgeFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public UniTask<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public UniTask<string> RegisterRequestAsync(RegisterFunction registerFunction)
        {
             return ContractHandler.SendRequestAsync(registerFunction);
        }

        public UniTask<TransactionReceipt> RegisterRequestAndWaitForReceiptAsync(RegisterFunction registerFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunction, cancellationToken);
        }

        public UniTask<string> RegisterRequestAsync(string name, string owner, BigInteger duration, byte[] secret)
        {
            var registerFunction = new RegisterFunction();
                registerFunction.Name = name;
                registerFunction.Owner = owner;
                registerFunction.Duration = duration;
                registerFunction.Secret = secret;
            
             return ContractHandler.SendRequestAsync(registerFunction);
        }

        public UniTask<TransactionReceipt> RegisterRequestAndWaitForReceiptAsync(string name, string owner, BigInteger duration, byte[] secret, CancellationToken cancellationToken = default)
        {
            var registerFunction = new RegisterFunction();
                registerFunction.Name = name;
                registerFunction.Owner = owner;
                registerFunction.Duration = duration;
                registerFunction.Secret = secret;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunction, cancellationToken);
        }

        public UniTask<string> RegisterWithConfigRequestAsync(RegisterWithConfigFunction registerWithConfigFunction)
        {
             return ContractHandler.SendRequestAsync(registerWithConfigFunction);
        }

        public UniTask<TransactionReceipt> RegisterWithConfigRequestAndWaitForReceiptAsync(RegisterWithConfigFunction registerWithConfigFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerWithConfigFunction, cancellationToken);
        }

        public UniTask<string> RegisterWithConfigRequestAsync(string name, string owner, BigInteger duration, byte[] secret, string resolver, string addr)
        {
            var registerWithConfigFunction = new RegisterWithConfigFunction();
                registerWithConfigFunction.Name = name;
                registerWithConfigFunction.Owner = owner;
                registerWithConfigFunction.Duration = duration;
                registerWithConfigFunction.Secret = secret;
                registerWithConfigFunction.Resolver = resolver;
                registerWithConfigFunction.Addr = addr;
            
             return ContractHandler.SendRequestAsync(registerWithConfigFunction);
        }

        public UniTask<TransactionReceipt> RegisterWithConfigRequestAndWaitForReceiptAsync(string name, string owner, BigInteger duration, byte[] secret, string resolver, string addr, CancellationToken cancellationToken = default)
        {
            var registerWithConfigFunction = new RegisterWithConfigFunction();
                registerWithConfigFunction.Name = name;
                registerWithConfigFunction.Owner = owner;
                registerWithConfigFunction.Duration = duration;
                registerWithConfigFunction.Secret = secret;
                registerWithConfigFunction.Resolver = resolver;
                registerWithConfigFunction.Addr = addr;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerWithConfigFunction, cancellationToken);
        }

        public UniTask<string> RenewRequestAsync(RenewFunction renewFunction)
        {
             return ContractHandler.SendRequestAsync(renewFunction);
        }

        public UniTask<TransactionReceipt> RenewRequestAndWaitForReceiptAsync(RenewFunction renewFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renewFunction, cancellationToken);
        }

        public UniTask<string> RenewRequestAsync(string name, BigInteger duration)
        {
            var renewFunction = new RenewFunction();
                renewFunction.Name = name;
                renewFunction.Duration = duration;
            
             return ContractHandler.SendRequestAsync(renewFunction);
        }

        public UniTask<TransactionReceipt> RenewRequestAndWaitForReceiptAsync(string name, BigInteger duration, CancellationToken cancellationToken = default)
        {
            var renewFunction = new RenewFunction();
                renewFunction.Name = name;
                renewFunction.Duration = duration;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renewFunction, cancellationToken);
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

        public UniTask<BigInteger> RentPriceQueryAsync(RentPriceFunction rentPriceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RentPriceFunction, BigInteger>(rentPriceFunction, blockParameter);
        }

        
        public UniTask<BigInteger> RentPriceQueryAsync(string name, BigInteger duration, BlockParameter blockParameter = null)
        {
            var rentPriceFunction = new RentPriceFunction();
                rentPriceFunction.Name = name;
                rentPriceFunction.Duration = duration;
            
            return ContractHandler.QueryAsync<RentPriceFunction, BigInteger>(rentPriceFunction, blockParameter);
        }

        public UniTask<string> SetCommitmentAgesRequestAsync(SetCommitmentAgesFunction setCommitmentAgesFunction)
        {
             return ContractHandler.SendRequestAsync(setCommitmentAgesFunction);
        }

        public UniTask<TransactionReceipt> SetCommitmentAgesRequestAndWaitForReceiptAsync(SetCommitmentAgesFunction setCommitmentAgesFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCommitmentAgesFunction, cancellationToken);
        }

        public UniTask<string> SetCommitmentAgesRequestAsync(BigInteger minCommitmentAge, BigInteger maxCommitmentAge)
        {
            var setCommitmentAgesFunction = new SetCommitmentAgesFunction();
                setCommitmentAgesFunction.MinCommitmentAge = minCommitmentAge;
                setCommitmentAgesFunction.MaxCommitmentAge = maxCommitmentAge;
            
             return ContractHandler.SendRequestAsync(setCommitmentAgesFunction);
        }

        public UniTask<TransactionReceipt> SetCommitmentAgesRequestAndWaitForReceiptAsync(BigInteger minCommitmentAge, BigInteger maxCommitmentAge, CancellationToken cancellationToken = default)
        {
            var setCommitmentAgesFunction = new SetCommitmentAgesFunction();
                setCommitmentAgesFunction.MinCommitmentAge = minCommitmentAge;
                setCommitmentAgesFunction.MaxCommitmentAge = maxCommitmentAge;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCommitmentAgesFunction, cancellationToken);
        }

        public UniTask<string> SetPriceOracleRequestAsync(SetPriceOracleFunction setPriceOracleFunction)
        {
             return ContractHandler.SendRequestAsync(setPriceOracleFunction);
        }

        public UniTask<TransactionReceipt> SetPriceOracleRequestAndWaitForReceiptAsync(SetPriceOracleFunction setPriceOracleFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPriceOracleFunction, cancellationToken);
        }

        public UniTask<string> SetPriceOracleRequestAsync(string prices)
        {
            var setPriceOracleFunction = new SetPriceOracleFunction();
                setPriceOracleFunction.Prices = prices;
            
             return ContractHandler.SendRequestAsync(setPriceOracleFunction);
        }

        public UniTask<TransactionReceipt> SetPriceOracleRequestAndWaitForReceiptAsync(string prices, CancellationToken cancellationToken = default)
        {
            var setPriceOracleFunction = new SetPriceOracleFunction();
                setPriceOracleFunction.Prices = prices;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPriceOracleFunction, cancellationToken);
        }

        public UniTask<bool> SupportsInterfaceQueryAsync(SupportsInterfaceFunction supportsInterfaceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        
        public UniTask<bool> SupportsInterfaceQueryAsync(byte[] interfaceID, BlockParameter blockParameter = null)
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
                supportsInterfaceFunction.InterfaceID = interfaceID;
            
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
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

        public UniTask<bool> ValidQueryAsync(ValidFunction validFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ValidFunction, bool>(validFunction, blockParameter);
        }

        
        public UniTask<bool> ValidQueryAsync(string name, BlockParameter blockParameter = null)
        {
            var validFunction = new ValidFunction();
                validFunction.Name = name;
            
            return ContractHandler.QueryAsync<ValidFunction, bool>(validFunction, blockParameter);
        }

        public UniTask<string> WithdrawRequestAsync(WithdrawFunction withdrawFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public UniTask<string> WithdrawRequestAsync()
        {
             return ContractHandler.SendRequestAsync<WithdrawFunction>();
        }

        public UniTask<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(WithdrawFunction withdrawFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<WithdrawFunction>(null, cancellationToken);
        }
#endif
    }
}
