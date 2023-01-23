using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ENS.ENSRegistry.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ENS
{
    public partial class ENSRegistryService
    {
        public string ContractAddress { get; }

        public ContractHandler ContractHandler { get; }

        public ENSRegistryService(IEthApiContractService ethApiContractService, string contractAddress)
        {
            ContractAddress = contractAddress;
#if !DOTNET35
            ContractHandler = ethApiContractService.GetContractHandler(contractAddress);
#endif
        }
#if !DOTNET35
        public UniTask<string> ResolverQueryAsync(ResolverFunction resolverFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ResolverFunction, string>(resolverFunction, blockParameter);
        }

        
        public UniTask<string> ResolverQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var resolverFunction = new ResolverFunction();
                resolverFunction.Node = node;
            
            return ContractHandler.QueryAsync<ResolverFunction, string>(resolverFunction, blockParameter);
        }

        public UniTask<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public UniTask<string> OwnerQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var ownerFunction = new OwnerFunction();
                ownerFunction.Node = node;
            
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        public UniTask<string> SetSubnodeOwnerRequestAsync(SetSubnodeOwnerFunction setSubnodeOwnerFunction)
        {
             return ContractHandler.SendRequestAsync(setSubnodeOwnerFunction);
        }

        public UniTask<TransactionReceipt> SetSubnodeOwnerRequestAndWaitForReceiptAsync(SetSubnodeOwnerFunction setSubnodeOwnerFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setSubnodeOwnerFunction, cancellationToken);
        }

        public UniTask<string> SetSubnodeOwnerRequestAsync(byte[] node, byte[] label, string owner)
        {
            var setSubnodeOwnerFunction = new SetSubnodeOwnerFunction();
                setSubnodeOwnerFunction.Node = node;
                setSubnodeOwnerFunction.Label = label;
                setSubnodeOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAsync(setSubnodeOwnerFunction);
        }

        public UniTask<TransactionReceipt> SetSubnodeOwnerRequestAndWaitForReceiptAsync(byte[] node, byte[] label, string owner, CancellationToken cancellationToken = default)
        {
            var setSubnodeOwnerFunction = new SetSubnodeOwnerFunction();
                setSubnodeOwnerFunction.Node = node;
                setSubnodeOwnerFunction.Label = label;
                setSubnodeOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setSubnodeOwnerFunction, cancellationToken);
        }

        public UniTask<string> SetTTLRequestAsync(SetTTLFunction setTTLFunction)
        {
             return ContractHandler.SendRequestAsync(setTTLFunction);
        }

        public UniTask<TransactionReceipt> SetTTLRequestAndWaitForReceiptAsync(SetTTLFunction setTTLFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTTLFunction, cancellationToken);
        }

        public UniTask<string> SetTTLRequestAsync(byte[] node, ulong ttl)
        {
            var setTTLFunction = new SetTTLFunction();
                setTTLFunction.Node = node;
                setTTLFunction.Ttl = ttl;
            
             return ContractHandler.SendRequestAsync(setTTLFunction);
        }

        public UniTask<TransactionReceipt> SetTTLRequestAndWaitForReceiptAsync(byte[] node, ulong ttl, CancellationToken cancellationToken = default)
        {
            var setTTLFunction = new SetTTLFunction();
                setTTLFunction.Node = node;
                setTTLFunction.Ttl = ttl;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTTLFunction, cancellationToken);
        }

        public UniTask<ulong> TtlQueryAsync(TtlFunction ttlFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TtlFunction, ulong>(ttlFunction, blockParameter);
        }

        
        public UniTask<ulong> TtlQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var ttlFunction = new TtlFunction();
                ttlFunction.Node = node;
            
            return ContractHandler.QueryAsync<TtlFunction, ulong>(ttlFunction, blockParameter);
        }

        public UniTask<string> SetResolverRequestAsync(SetResolverFunction setResolverFunction)
        {
             return ContractHandler.SendRequestAsync(setResolverFunction);
        }

        public UniTask<TransactionReceipt> SetResolverRequestAndWaitForReceiptAsync(SetResolverFunction setResolverFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setResolverFunction, cancellationToken);
        }

        public UniTask<string> SetResolverRequestAsync(byte[] node, string resolver)
        {
            var setResolverFunction = new SetResolverFunction();
                setResolverFunction.Node = node;
                setResolverFunction.Resolver = resolver;
            
             return ContractHandler.SendRequestAsync(setResolverFunction);
        }

        public UniTask<TransactionReceipt> SetResolverRequestAndWaitForReceiptAsync(byte[] node, string resolver, CancellationToken cancellationToken = default)
        {
            var setResolverFunction = new SetResolverFunction();
                setResolverFunction.Node = node;
                setResolverFunction.Resolver = resolver;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setResolverFunction, cancellationToken);
        }

        public UniTask<string> SetOwnerRequestAsync(SetOwnerFunction setOwnerFunction)
        {
             return ContractHandler.SendRequestAsync(setOwnerFunction);
        }

        public UniTask<TransactionReceipt> SetOwnerRequestAndWaitForReceiptAsync(SetOwnerFunction setOwnerFunction, CancellationToken cancellationToken = default)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setOwnerFunction, cancellationToken);
        }

        public UniTask<string> SetOwnerRequestAsync(byte[] node, string owner)
        {
            var setOwnerFunction = new SetOwnerFunction();
                setOwnerFunction.Node = node;
                setOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAsync(setOwnerFunction);
        }

        public UniTask<TransactionReceipt> SetOwnerRequestAndWaitForReceiptAsync(byte[] node, string owner, CancellationToken cancellationToken = default)
        {
            var setOwnerFunction = new SetOwnerFunction();
                setOwnerFunction.Node = node;
                setOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setOwnerFunction, cancellationToken);
        }
#endif
    }
}
