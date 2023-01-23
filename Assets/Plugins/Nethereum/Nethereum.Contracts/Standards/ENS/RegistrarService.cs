using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ENS.Registrar.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ENS
{
    public partial class RegistrarService
    {
        public string ContractAddress { get; }

        public ContractHandler ContractHandler { get; }

        public RegistrarService(IEthApiContractService ethApiContractService, string contractAddress)
        {
            ContractAddress = contractAddress;
#if !DOTNET35
            ContractHandler = ethApiContractService.GetContractHandler(contractAddress);
#endif
        }
#if !DOTNET35
        public UniTask<string> ReleaseDeedRequestAsync(ReleaseDeedFunction releaseDeedFunction)
        {
             return ContractHandler.SendRequestAsync(releaseDeedFunction);
        }

        public UniTask<TransactionReceipt> ReleaseDeedRequestAndWaitForReceiptAsync(ReleaseDeedFunction releaseDeedFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(releaseDeedFunction, cancellationToken);
        }

        public UniTask<string> ReleaseDeedRequestAsync(byte[] hash)
        {
            var releaseDeedFunction = new ReleaseDeedFunction();
                releaseDeedFunction.Hash = hash;
            
             return ContractHandler.SendRequestAsync(releaseDeedFunction);
        }

        public UniTask<TransactionReceipt> ReleaseDeedRequestAndWaitForReceiptAsync(byte[] hash, CancellationToken cancellationToken = default)
        {
            var releaseDeedFunction = new ReleaseDeedFunction();
                releaseDeedFunction.Hash = hash;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(releaseDeedFunction, cancellationToken);
        }

        public UniTask<BigInteger> GetAllowedTimeQueryAsync(GetAllowedTimeFunction getAllowedTimeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetAllowedTimeFunction, BigInteger>(getAllowedTimeFunction, blockParameter);
        }

        
        public UniTask<BigInteger> GetAllowedTimeQueryAsync(byte[] hash, BlockParameter blockParameter = null)
        {
            var getAllowedTimeFunction = new GetAllowedTimeFunction();
                getAllowedTimeFunction.Hash = hash;
            
            return ContractHandler.QueryAsync<GetAllowedTimeFunction, BigInteger>(getAllowedTimeFunction, blockParameter);
        }



        public UniTask<string> InvalidateNameRequestAsync(InvalidateNameFunction invalidateNameFunction)
        {
             return ContractHandler.SendRequestAsync(invalidateNameFunction);
        }

        public UniTask<TransactionReceipt> InvalidateNameRequestAndWaitForReceiptAsync(InvalidateNameFunction invalidateNameFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(invalidateNameFunction, cancellationToken);
        }

        public UniTask<string> InvalidateNameRequestAsync(string unhashedName)
        {
            var invalidateNameFunction = new InvalidateNameFunction();
                invalidateNameFunction.UnhashedName = unhashedName;
            
             return ContractHandler.SendRequestAsync(invalidateNameFunction);
        }

        public UniTask<TransactionReceipt> InvalidateNameRequestAndWaitForReceiptAsync(string unhashedName, CancellationToken cancellationToken = default)
        {
            var invalidateNameFunction = new InvalidateNameFunction();
                invalidateNameFunction.UnhashedName = unhashedName;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(invalidateNameFunction, cancellationToken);
        }

        public UniTask<byte[]> ShaBidQueryAsync(ShaBidFunction shaBidFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ShaBidFunction, byte[]>(shaBidFunction, blockParameter);
        }

        
        public UniTask<byte[]> ShaBidQueryAsync(byte[] hash, string owner, BigInteger value, byte[] salt, BlockParameter blockParameter = null)
        {
            var shaBidFunction = new ShaBidFunction();
                shaBidFunction.Hash = hash;
                shaBidFunction.Owner = owner;
                shaBidFunction.Value = value;
                shaBidFunction.Salt = salt;
            
            return ContractHandler.QueryAsync<ShaBidFunction, byte[]>(shaBidFunction, blockParameter);
        }



        public UniTask<string> CancelBidRequestAsync(CancelBidFunction cancelBidFunction)
        {
             return ContractHandler.SendRequestAsync(cancelBidFunction);
        }

        public UniTask<TransactionReceipt> CancelBidRequestAndWaitForReceiptAsync(CancelBidFunction cancelBidFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelBidFunction, cancellationToken);
        }

        public UniTask<string> CancelBidRequestAsync(string bidder, byte[] seal)
        {
            var cancelBidFunction = new CancelBidFunction();
                cancelBidFunction.Bidder = bidder;
                cancelBidFunction.Seal = seal;
            
             return ContractHandler.SendRequestAsync(cancelBidFunction);
        }

        public UniTask<TransactionReceipt> CancelBidRequestAndWaitForReceiptAsync(string bidder, byte[] seal, CancellationToken cancellationToken = default)
        {
            var cancelBidFunction = new CancelBidFunction();
                cancelBidFunction.Bidder = bidder;
                cancelBidFunction.Seal = seal;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelBidFunction, cancellationToken);
        }

        public UniTask<EntriesOutputDTO> EntriesQueryAsync(EntriesFunction entriesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<EntriesFunction, EntriesOutputDTO>(entriesFunction, blockParameter);
        }

        
        public UniTask<EntriesOutputDTO> EntriesQueryAsync(byte[] hash, BlockParameter blockParameter = null)
        {
            var entriesFunction = new EntriesFunction();
                entriesFunction.Hash = hash;
            
            return ContractHandler.QueryDeserializingToObjectAsync<EntriesFunction, EntriesOutputDTO>(entriesFunction, blockParameter);
        }



        public UniTask<string> EnsQueryAsync(EnsFunction ensFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<EnsFunction, string>(ensFunction, blockParameter);
        }

        
        public UniTask<string> EnsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<EnsFunction, string>(null, blockParameter);
        }



        public UniTask<string> UnsealBidRequestAsync(UnsealBidFunction unsealBidFunction)
        {
             return ContractHandler.SendRequestAsync(unsealBidFunction);
        }

        public UniTask<TransactionReceipt> UnsealBidRequestAndWaitForReceiptAsync(UnsealBidFunction unsealBidFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unsealBidFunction, cancellationToken);
        }

        public UniTask<string> UnsealBidRequestAsync(byte[] hash, BigInteger value, byte[] salt)
        {
            var unsealBidFunction = new UnsealBidFunction();
                unsealBidFunction.Hash = hash;
                unsealBidFunction.Value = value;
                unsealBidFunction.Salt = salt;
            
             return ContractHandler.SendRequestAsync(unsealBidFunction);
        }

        public UniTask<TransactionReceipt> UnsealBidRequestAndWaitForReceiptAsync(byte[] hash, BigInteger value, byte[] salt, CancellationToken cancellationToken = default)
        {
            var unsealBidFunction = new UnsealBidFunction();
                unsealBidFunction.Hash = hash;
                unsealBidFunction.Value = value;
                unsealBidFunction.Salt = salt;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unsealBidFunction, cancellationToken);
        }

        public UniTask<string> TransferRegistrarsRequestAsync(TransferRegistrarsFunction transferRegistrarsFunction)
        {
             return ContractHandler.SendRequestAsync(transferRegistrarsFunction);
        }

        public UniTask<TransactionReceipt> TransferRegistrarsRequestAndWaitForReceiptAsync(TransferRegistrarsFunction transferRegistrarsFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferRegistrarsFunction, cancellationToken);
        }

        public UniTask<string> TransferRegistrarsRequestAsync(byte[] hash)
        {
            var transferRegistrarsFunction = new TransferRegistrarsFunction();
                transferRegistrarsFunction.Hash = hash;
            
             return ContractHandler.SendRequestAsync(transferRegistrarsFunction);
        }

        public UniTask<TransactionReceipt> TransferRegistrarsRequestAndWaitForReceiptAsync(byte[] hash, CancellationToken cancellationToken = default)
        {
            var transferRegistrarsFunction = new TransferRegistrarsFunction();
                transferRegistrarsFunction.Hash = hash;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferRegistrarsFunction, cancellationToken);
        }

        public UniTask<string> SealedBidsQueryAsync(SealedBidsFunction sealedBidsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SealedBidsFunction, string>(sealedBidsFunction, blockParameter);
        }

        
        public UniTask<string> SealedBidsQueryAsync(string returnValue1, byte[] returnValue2, BlockParameter blockParameter = null)
        {
            var sealedBidsFunction = new SealedBidsFunction();
                sealedBidsFunction.ReturnValue1 = returnValue1;
                sealedBidsFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<SealedBidsFunction, string>(sealedBidsFunction, blockParameter);
        }



        public UniTask<byte> StateQueryAsync(StateFunction stateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StateFunction, byte>(stateFunction, blockParameter);
        }

        
        public UniTask<byte> StateQueryAsync(byte[] hash, BlockParameter blockParameter = null)
        {
            var stateFunction = new StateFunction();
                stateFunction.Hash = hash;
            
            return ContractHandler.QueryAsync<StateFunction, byte>(stateFunction, blockParameter);
        }



        public UniTask<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public UniTask<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public UniTask<string> TransferRequestAsync(byte[] hash, string newOwner)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Hash = hash;
                transferFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public UniTask<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(byte[] hash, string newOwner, CancellationToken cancellationToken = default)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Hash = hash;
                transferFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public UniTask<bool> IsAllowedQueryAsync(IsAllowedFunction isAllowedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsAllowedFunction, bool>(isAllowedFunction, blockParameter);
        }

        
        public UniTask<bool> IsAllowedQueryAsync(byte[] hash, BigInteger timestamp, BlockParameter blockParameter = null)
        {
            var isAllowedFunction = new IsAllowedFunction();
                isAllowedFunction.Hash = hash;
                isAllowedFunction.Timestamp = timestamp;
            
            return ContractHandler.QueryAsync<IsAllowedFunction, bool>(isAllowedFunction, blockParameter);
        }



        public UniTask<string> FinalizeAuctionRequestAsync(FinalizeAuctionFunction finalizeAuctionFunction)
        {
             return ContractHandler.SendRequestAsync(finalizeAuctionFunction);
        }

        public UniTask<TransactionReceipt> FinalizeAuctionRequestAndWaitForReceiptAsync(FinalizeAuctionFunction finalizeAuctionFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finalizeAuctionFunction, cancellationToken);
        }

        public UniTask<string> FinalizeAuctionRequestAsync(byte[] hash)
        {
            var finalizeAuctionFunction = new FinalizeAuctionFunction();
                finalizeAuctionFunction.Hash = hash;
            
             return ContractHandler.SendRequestAsync(finalizeAuctionFunction);
        }

        public UniTask<TransactionReceipt> FinalizeAuctionRequestAndWaitForReceiptAsync(byte[] hash, CancellationToken cancellationToken = default)
        {
            var finalizeAuctionFunction = new FinalizeAuctionFunction();
                finalizeAuctionFunction.Hash = hash;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finalizeAuctionFunction, cancellationToken);
        }

        public UniTask<BigInteger> RegistryStartedQueryAsync(RegistryStartedFunction registryStartedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RegistryStartedFunction, BigInteger>(registryStartedFunction, blockParameter);
        }

        
        public UniTask<BigInteger> RegistryStartedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RegistryStartedFunction, BigInteger>(null, blockParameter);
        }



        public UniTask<uint> LaunchLengthQueryAsync(LaunchLengthFunction launchLengthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LaunchLengthFunction, uint>(launchLengthFunction, blockParameter);
        }

        
        public UniTask<uint> LaunchLengthQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LaunchLengthFunction, uint>(null, blockParameter);
        }



        public UniTask<string> NewBidRequestAsync(NewBidFunction newBidFunction)
        {
             return ContractHandler.SendRequestAsync(newBidFunction);
        }

        public UniTask<TransactionReceipt> NewBidRequestAndWaitForReceiptAsync(NewBidFunction newBidFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(newBidFunction, cancellationToken);
        }

        public UniTask<string> NewBidRequestAsync(byte[] sealedBid)
        {
            var newBidFunction = new NewBidFunction();
                newBidFunction.SealedBid = sealedBid;
            
             return ContractHandler.SendRequestAsync(newBidFunction);
        }

        public UniTask<TransactionReceipt> NewBidRequestAndWaitForReceiptAsync(byte[] sealedBid, CancellationToken cancellationToken = default)
        {
            var newBidFunction = new NewBidFunction();
                newBidFunction.SealedBid = sealedBid;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(newBidFunction, cancellationToken);
        }

        public UniTask<string> EraseNodeRequestAsync(EraseNodeFunction eraseNodeFunction)
        {
             return ContractHandler.SendRequestAsync(eraseNodeFunction);
        }

        public UniTask<TransactionReceipt> EraseNodeRequestAndWaitForReceiptAsync(EraseNodeFunction eraseNodeFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(eraseNodeFunction, cancellationToken);
        }

        public UniTask<string> EraseNodeRequestAsync(List<byte[]> labels)
        {
            var eraseNodeFunction = new EraseNodeFunction();
                eraseNodeFunction.Labels = labels;
            
             return ContractHandler.SendRequestAsync(eraseNodeFunction);
        }

        public UniTask<TransactionReceipt> EraseNodeRequestAndWaitForReceiptAsync(List<byte[]> labels, CancellationToken cancellationToken = default)
        {
            var eraseNodeFunction = new EraseNodeFunction();
                eraseNodeFunction.Labels = labels;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(eraseNodeFunction, cancellationToken);
        }

        public UniTask<string> StartAuctionsRequestAsync(StartAuctionsFunction startAuctionsFunction)
        {
             return ContractHandler.SendRequestAsync(startAuctionsFunction);
        }

        public UniTask<TransactionReceipt> StartAuctionsRequestAndWaitForReceiptAsync(StartAuctionsFunction startAuctionsFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startAuctionsFunction, cancellationToken);
        }

        public UniTask<string> StartAuctionsRequestAsync(List<byte[]> hashes)
        {
            var startAuctionsFunction = new StartAuctionsFunction();
                startAuctionsFunction.Hashes = hashes;
            
             return ContractHandler.SendRequestAsync(startAuctionsFunction);
        }

        public UniTask<TransactionReceipt> StartAuctionsRequestAndWaitForReceiptAsync(List<byte[]> hashes, CancellationToken cancellationToken = default)
        {
            var startAuctionsFunction = new StartAuctionsFunction();
                startAuctionsFunction.Hashes = hashes;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startAuctionsFunction, cancellationToken);
        }

        public UniTask<string> AcceptRegistrarTransferRequestAsync(AcceptRegistrarTransferFunction acceptRegistrarTransferFunction)
        {
             return ContractHandler.SendRequestAsync(acceptRegistrarTransferFunction);
        }

        public UniTask<TransactionReceipt> AcceptRegistrarTransferRequestAndWaitForReceiptAsync(AcceptRegistrarTransferFunction acceptRegistrarTransferFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(acceptRegistrarTransferFunction, cancellationToken);
        }

        public UniTask<string> AcceptRegistrarTransferRequestAsync(byte[] hash, string deed, BigInteger registrationDate)
        {
            var acceptRegistrarTransferFunction = new AcceptRegistrarTransferFunction();
                acceptRegistrarTransferFunction.Hash = hash;
                acceptRegistrarTransferFunction.Deed = deed;
                acceptRegistrarTransferFunction.RegistrationDate = registrationDate;
            
             return ContractHandler.SendRequestAsync(acceptRegistrarTransferFunction);
        }

        public UniTask<TransactionReceipt> AcceptRegistrarTransferRequestAndWaitForReceiptAsync(byte[] hash, string deed, BigInteger registrationDate, CancellationToken cancellationToken = default)
        {
            var acceptRegistrarTransferFunction = new AcceptRegistrarTransferFunction();
                acceptRegistrarTransferFunction.Hash = hash;
                acceptRegistrarTransferFunction.Deed = deed;
                acceptRegistrarTransferFunction.RegistrationDate = registrationDate;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(acceptRegistrarTransferFunction, cancellationToken);
        }

        public UniTask<string> StartAuctionRequestAsync(StartAuctionFunction startAuctionFunction)
        {
             return ContractHandler.SendRequestAsync(startAuctionFunction);
        }

        public UniTask<TransactionReceipt> StartAuctionRequestAndWaitForReceiptAsync(StartAuctionFunction startAuctionFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startAuctionFunction, cancellationToken);
        }

        public UniTask<string> StartAuctionRequestAsync(byte[] hash)
        {
            var startAuctionFunction = new StartAuctionFunction();
                startAuctionFunction.Hash = hash;
            
             return ContractHandler.SendRequestAsync(startAuctionFunction);
        }

        public UniTask<TransactionReceipt> StartAuctionRequestAndWaitForReceiptAsync(byte[] hash, CancellationToken cancellationToken = default)
        {
            var startAuctionFunction = new StartAuctionFunction();
                startAuctionFunction.Hash = hash;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startAuctionFunction, cancellationToken);
        }

        public UniTask<byte[]> RootNodeQueryAsync(RootNodeFunction rootNodeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RootNodeFunction, byte[]>(rootNodeFunction, blockParameter);
        }

        
        public UniTask<byte[]> RootNodeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RootNodeFunction, byte[]>(null, blockParameter);
        }



        public UniTask<string> StartAuctionsAndBidRequestAsync(StartAuctionsAndBidFunction startAuctionsAndBidFunction)
        {
             return ContractHandler.SendRequestAsync(startAuctionsAndBidFunction);
        }

        public UniTask<TransactionReceipt> StartAuctionsAndBidRequestAndWaitForReceiptAsync(StartAuctionsAndBidFunction startAuctionsAndBidFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startAuctionsAndBidFunction, cancellationToken);
        }

        public UniTask<string> StartAuctionsAndBidRequestAsync(List<byte[]> hashes, byte[] sealedBid)
        {
            var startAuctionsAndBidFunction = new StartAuctionsAndBidFunction();
                startAuctionsAndBidFunction.Hashes = hashes;
                startAuctionsAndBidFunction.SealedBid = sealedBid;
            
             return ContractHandler.SendRequestAsync(startAuctionsAndBidFunction);
        }

        public UniTask<TransactionReceipt> StartAuctionsAndBidRequestAndWaitForReceiptAsync(List<byte[]> hashes, byte[] sealedBid, CancellationToken cancellationToken = default)
        {
            var startAuctionsAndBidFunction = new StartAuctionsAndBidFunction();
                startAuctionsAndBidFunction.Hashes = hashes;
                startAuctionsAndBidFunction.SealedBid = sealedBid;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startAuctionsAndBidFunction, cancellationToken);
        }
#endif
    }
}
