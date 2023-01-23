using System;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Constants;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ENS.PublicResolver.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ENS
{
    public class ENSService
    {
        private readonly IEthApiContractService _ethApiContractService;
        public static string REVERSE_NAME_SUFFIX = ".addr.reverse";

        public ENSService(IEthApiContractService ethApiContractService, string ensRegistryAddress = CommonAddresses.ENS_REGISTRY_ADDRESS)
        {
            if (ethApiContractService == null) throw new ArgumentNullException(nameof(ethApiContractService));
            _ethApiContractService = ethApiContractService;
            EnsRegistryAddress = ensRegistryAddress ?? throw new ArgumentNullException(nameof(ensRegistryAddress));
            _ensUtil = new EnsUtil();
            ENSRegistryService = new ENSRegistryService(ethApiContractService, EnsRegistryAddress);
        }

        public string EnsRegistryAddress { get; }
        public ENSRegistryService ENSRegistryService { get; private set; }
        
        private readonly EnsUtil _ensUtil;
#if !DOTNET35
        public async UniTask<string> ResolveAddressAsync(string fullName)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.AddrQueryAsync(fullNameNode);
        }

        public async UniTask<ABIOutputDTO> ResolveABIAsync(string fullName, AbiTypeContentType abiTypeContentType)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.ABIQueryAsync(fullNameNode, (int)abiTypeContentType);
        }

        public UniTask<string> SetSubnodeOwnerRequestAsync(string fullName, string label, string owner)
        {
            var fullNameHash = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var labelHash = _ensUtil.GetLabelHash(label).HexToByteArray();
            return ENSRegistryService.SetSubnodeOwnerRequestAsync(fullNameHash, labelHash, owner);
        }

        public UniTask<TransactionReceipt> SetSubnodeOwnerRequestAndWaitForReceiptAsync(string fullName, string label, string owner)
        {
            var fullNameHash = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var labelHash = _ensUtil.GetLabelHash(label).HexToByteArray();
            return ENSRegistryService.SetSubnodeOwnerRequestAndWaitForReceiptAsync(fullNameHash, labelHash, owner);
        }

        public async UniTask<string> ResolveTextAsync(string fullName, TextDataKey textDataKey)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.TextQueryAsync(fullNameNode, textDataKey.GetDataKeyAsString());
        }

        public async UniTask<string> SetTextRequestAsync(string fullName, TextDataKey textDataKey, string value)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.SetTextRequestAsync(fullNameNode, textDataKey.GetDataKeyAsString(), value);
        }

        public async UniTask<string> SetAddressRequestAsync(string fullName, string address)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.SetAddrRequestAsync(fullNameNode, address);
        }

        public async UniTask<string> SetContentHashRequestAsync(string fullName, string contentHashInHex)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.SetContenthashRequestAsync(fullNameNode, contentHashInHex.HexToByteArray());
        }

        public async UniTask<byte[]> GetContentHashAsync(string fullName)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.ContenthashQueryAsync(fullNameNode);
        }

        public async UniTask<TransactionReceipt> SetTextRequestAndWaitForReceiptAsync(string fullName, TextDataKey textDataKey, string value, CancellationToken cancellationToken = default)
        {
            var fullNameNode = _ensUtil.GetNameHash(fullName).HexToByteArray();
            var resolverService = await GetResolverAsync(fullNameNode);
            return await resolverService.SetTextRequestAndWaitForReceiptAsync(fullNameNode, textDataKey.GetDataKeyAsString(), value, cancellationToken);
        }

        public UniTask<PublicResolverService> GetResolverAsync(string fullNameNode)
        {
            var fullNameNodeAsBytes = new EnsUtil().GetNameHash(fullNameNode).HexToByteArray();
            return GetResolverAsync(fullNameNodeAsBytes);
        }

        public async UniTask<string> ReverseResolveAsync(string address)
        {
           var addressReverse = address.RemoveHexPrefix().ToLower() + REVERSE_NAME_SUFFIX;
           var fullNameNode = _ensUtil.GetNameHash(addressReverse).HexToByteArray();
           var resolverService = await GetResolverAsync(fullNameNode);
           return await resolverService.NameQueryAsync(fullNameNode);
        }

        public async UniTask<PublicResolverService> GetResolverAsync(byte[] fullNameNode)
        {
            var resolverAddress = await ENSRegistryService.ResolverQueryAsync(fullNameNode);
            var resolverService = new PublicResolverService(_ethApiContractService, resolverAddress);
            return resolverService;

        }
#endif
    }

}
