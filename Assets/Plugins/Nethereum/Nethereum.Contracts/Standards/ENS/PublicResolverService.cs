using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ENS.PublicResolver.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ENS
{
    public partial class PublicResolverService
    {
        public string ContractAddress { get; }

        public ContractHandler ContractHandler { get; }

        public PublicResolverService(IEthApiContractService ethApiContractService, string contractAddress)
        {
            ContractAddress = contractAddress;
#if !DOTNET35
            ContractHandler = ethApiContractService.GetContractHandler(contractAddress);
#endif
        }
#if !DOTNET35
        public UniTask<ABIOutputDTO> ABIQueryAsync(ABIFunction aBIFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ABIFunction, ABIOutputDTO>(aBIFunction, blockParameter);
        }

        public UniTask<ABIOutputDTO> ABIQueryAsync(byte[] node, BigInteger contentTypes, BlockParameter blockParameter = null)
        {
            var aBIFunction = new ABIFunction();
            aBIFunction.Node = node;
            aBIFunction.ContentTypes = contentTypes;

            return ContractHandler.QueryDeserializingToObjectAsync<ABIFunction, ABIOutputDTO>(aBIFunction, blockParameter);
        }

        public UniTask<string> AddrQueryAsync(AddrFunction addrFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AddrFunction, string>(addrFunction, blockParameter);
        }


        public UniTask<string> AddrQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var addrFunction = new AddrFunction();
            addrFunction.Node = node;

            return ContractHandler.QueryAsync<AddrFunction, string>(addrFunction, blockParameter);
        }

        public UniTask<byte[]> AddrQueryAsync(AddrFunction2 addrFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AddrFunction2, byte[]>(addrFunction, blockParameter);
        }


        public UniTask<byte[]> AddrQueryAsync(byte[] node, BigInteger coinType, BlockParameter blockParameter = null)
        {
            var addrFunction = new AddrFunction2();
            addrFunction.Node = node;
            addrFunction.CoinType = coinType;

            return ContractHandler.QueryAsync<AddrFunction2, byte[]>(addrFunction, blockParameter);
        }

        public UniTask<bool> AuthorisationsQueryAsync(AuthorisationsFunction authorisationsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AuthorisationsFunction, bool>(authorisationsFunction, blockParameter);
        }


        public UniTask<bool> AuthorisationsQueryAsync(byte[] returnValue1, string returnValue2, string returnValue3, BlockParameter blockParameter = null)
        {
            var authorisationsFunction = new AuthorisationsFunction();
            authorisationsFunction.ReturnValue1 = returnValue1;
            authorisationsFunction.ReturnValue2 = returnValue2;
            authorisationsFunction.ReturnValue3 = returnValue3;

            return ContractHandler.QueryAsync<AuthorisationsFunction, bool>(authorisationsFunction, blockParameter);
        }

        public UniTask<string> ClearDNSZoneRequestAsync(ClearDNSZoneFunction clearDNSZoneFunction)
        {
            return ContractHandler.SendRequestAsync(clearDNSZoneFunction);
        }

        public UniTask<TransactionReceipt> ClearDNSZoneRequestAndWaitForReceiptAsync(ClearDNSZoneFunction clearDNSZoneFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(clearDNSZoneFunction, cancellationToken);
        }

        public UniTask<string> ClearDNSZoneRequestAsync(byte[] node)
        {
            var clearDNSZoneFunction = new ClearDNSZoneFunction();
            clearDNSZoneFunction.Node = node;

            return ContractHandler.SendRequestAsync(clearDNSZoneFunction);
        }

        public UniTask<TransactionReceipt> ClearDNSZoneRequestAndWaitForReceiptAsync(byte[] node, CancellationToken cancellationToken = default)
        {
            var clearDNSZoneFunction = new ClearDNSZoneFunction();
            clearDNSZoneFunction.Node = node;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(clearDNSZoneFunction, cancellationToken);
        }

        public UniTask<byte[]> ContenthashQueryAsync(ContenthashFunction contenthashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContenthashFunction, byte[]>(contenthashFunction, blockParameter);
        }


        public UniTask<byte[]> ContenthashQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var contenthashFunction = new ContenthashFunction();
            contenthashFunction.Node = node;

            return ContractHandler.QueryAsync<ContenthashFunction, byte[]>(contenthashFunction, blockParameter);
        }

        public UniTask<byte[]> DnsRecordQueryAsync(DnsRecordFunction dnsRecordFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DnsRecordFunction, byte[]>(dnsRecordFunction, blockParameter);
        }


        public UniTask<byte[]> DnsRecordQueryAsync(byte[] node, byte[] name, ushort resource, BlockParameter blockParameter = null)
        {
            var dnsRecordFunction = new DnsRecordFunction();
            dnsRecordFunction.Node = node;
            dnsRecordFunction.Name = name;
            dnsRecordFunction.Resource = resource;

            return ContractHandler.QueryAsync<DnsRecordFunction, byte[]>(dnsRecordFunction, blockParameter);
        }

        public UniTask<bool> HasDNSRecordsQueryAsync(HasDNSRecordsFunction hasDNSRecordsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HasDNSRecordsFunction, bool>(hasDNSRecordsFunction, blockParameter);
        }


        public UniTask<bool> HasDNSRecordsQueryAsync(byte[] node, byte[] name, BlockParameter blockParameter = null)
        {
            var hasDNSRecordsFunction = new HasDNSRecordsFunction();
            hasDNSRecordsFunction.Node = node;
            hasDNSRecordsFunction.Name = name;

            return ContractHandler.QueryAsync<HasDNSRecordsFunction, bool>(hasDNSRecordsFunction, blockParameter);
        }

        public UniTask<string> InterfaceImplementerQueryAsync(InterfaceImplementerFunction interfaceImplementerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<InterfaceImplementerFunction, string>(interfaceImplementerFunction, blockParameter);
        }


        public UniTask<string> InterfaceImplementerQueryAsync(byte[] node, byte[] interfaceID, BlockParameter blockParameter = null)
        {
            var interfaceImplementerFunction = new InterfaceImplementerFunction();
            interfaceImplementerFunction.Node = node;
            interfaceImplementerFunction.InterfaceID = interfaceID;

            return ContractHandler.QueryAsync<InterfaceImplementerFunction, string>(interfaceImplementerFunction, blockParameter);
        }

        public UniTask<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }


        public UniTask<string> NameQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var nameFunction = new NameFunction();
            nameFunction.Node = node;

            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        public UniTask<PubkeyOutputDTO> PubkeyQueryAsync(PubkeyFunction pubkeyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<PubkeyFunction, PubkeyOutputDTO>(pubkeyFunction, blockParameter);
        }

        public UniTask<PubkeyOutputDTO> PubkeyQueryAsync(byte[] node, BlockParameter blockParameter = null)
        {
            var pubkeyFunction = new PubkeyFunction();
            pubkeyFunction.Node = node;

            return ContractHandler.QueryDeserializingToObjectAsync<PubkeyFunction, PubkeyOutputDTO>(pubkeyFunction, blockParameter);
        }

        public UniTask<string> SetABIRequestAsync(SetABIFunction setABIFunction)
        {
            return ContractHandler.SendRequestAsync(setABIFunction);
        }

        public UniTask<TransactionReceipt> SetABIRequestAndWaitForReceiptAsync(SetABIFunction setABIFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setABIFunction, cancellationToken);
        }

        public UniTask<string> SetABIRequestAsync(byte[] node, BigInteger contentType, byte[] data)
        {
            var setABIFunction = new SetABIFunction();
            setABIFunction.Node = node;
            setABIFunction.ContentType = contentType;
            setABIFunction.Data = data;

            return ContractHandler.SendRequestAsync(setABIFunction);
        }

        public UniTask<TransactionReceipt> SetABIRequestAndWaitForReceiptAsync(byte[] node, BigInteger contentType, byte[] data, CancellationToken cancellationToken = default)
        {
            var setABIFunction = new SetABIFunction();
            setABIFunction.Node = node;
            setABIFunction.ContentType = contentType;
            setABIFunction.Data = data;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setABIFunction, cancellationToken);
        }

        public UniTask<string> SetAddrRequestAsync(SetAddrFunction setAddrFunction)
        {
            return ContractHandler.SendRequestAsync(setAddrFunction);
        }

        public UniTask<TransactionReceipt> SetAddrRequestAndWaitForReceiptAsync(SetAddrFunction setAddrFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setAddrFunction, cancellationToken);
        }

        public UniTask<string> SetAddrRequestAsync(byte[] node, BigInteger coinType, byte[] a)
        {
            var setAddrFunction = new SetAddrFunction();
            setAddrFunction.Node = node;
            setAddrFunction.CoinType = coinType;
            setAddrFunction.A = a;

            return ContractHandler.SendRequestAsync(setAddrFunction);
        }

        public UniTask<TransactionReceipt> SetAddrRequestAndWaitForReceiptAsync(byte[] node, BigInteger coinType, byte[] a, CancellationToken cancellationToken = default)
        {
            var setAddrFunction = new SetAddrFunction();
            setAddrFunction.Node = node;
            setAddrFunction.CoinType = coinType;
            setAddrFunction.A = a;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setAddrFunction, cancellationToken);
        }

        public UniTask<string> SetAddrRequestAsync(SetAddrFunction2 setAddrFunction)
        {
            return ContractHandler.SendRequestAsync(setAddrFunction);
        }

        public UniTask<TransactionReceipt> SetAddrRequestAndWaitForReceiptAsync(SetAddrFunction2 setAddrFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setAddrFunction, cancellationToken);
        }

        public UniTask<string> SetAddrRequestAsync(byte[] node, string a)
        {
            var setAddrFunction = new SetAddrFunction2();
            setAddrFunction.Node = node;
            setAddrFunction.A = a;

            return ContractHandler.SendRequestAsync(setAddrFunction);
        }

        public UniTask<TransactionReceipt> SetAddrRequestAndWaitForReceiptAsync(byte[] node, string a, CancellationToken cancellationToken = default)
        {
            var setAddrFunction = new SetAddrFunction2();
            setAddrFunction.Node = node;
            setAddrFunction.A = a;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setAddrFunction, cancellationToken);
        }

        public UniTask<string> SetAuthorisationRequestAsync(SetAuthorisationFunction setAuthorisationFunction)
        {
            return ContractHandler.SendRequestAsync(setAuthorisationFunction);
        }

        public UniTask<TransactionReceipt> SetAuthorisationRequestAndWaitForReceiptAsync(SetAuthorisationFunction setAuthorisationFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setAuthorisationFunction, cancellationToken);
        }

        public UniTask<string> SetAuthorisationRequestAsync(byte[] node, string target, bool isAuthorised)
        {
            var setAuthorisationFunction = new SetAuthorisationFunction();
            setAuthorisationFunction.Node = node;
            setAuthorisationFunction.Target = target;
            setAuthorisationFunction.IsAuthorised = isAuthorised;

            return ContractHandler.SendRequestAsync(setAuthorisationFunction);
        }

        public UniTask<TransactionReceipt> SetAuthorisationRequestAndWaitForReceiptAsync(byte[] node, string target, bool isAuthorised, CancellationToken cancellationToken = default)
        {
            var setAuthorisationFunction = new SetAuthorisationFunction();
            setAuthorisationFunction.Node = node;
            setAuthorisationFunction.Target = target;
            setAuthorisationFunction.IsAuthorised = isAuthorised;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setAuthorisationFunction, cancellationToken);
        }

        public UniTask<string> SetContenthashRequestAsync(SetContenthashFunction setContenthashFunction)
        {
            return ContractHandler.SendRequestAsync(setContenthashFunction);
        }

        public UniTask<TransactionReceipt> SetContenthashRequestAndWaitForReceiptAsync(SetContenthashFunction setContenthashFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setContenthashFunction, cancellationToken);
        }

        public UniTask<string> SetContenthashRequestAsync(byte[] node, byte[] hash)
        {
            var setContenthashFunction = new SetContenthashFunction();
            setContenthashFunction.Node = node;
            setContenthashFunction.Hash = hash;

            return ContractHandler.SendRequestAsync(setContenthashFunction);
        }

        public UniTask<TransactionReceipt> SetContenthashRequestAndWaitForReceiptAsync(byte[] node, byte[] hash, CancellationToken cancellationToken = default)
        {
            var setContenthashFunction = new SetContenthashFunction();
            setContenthashFunction.Node = node;
            setContenthashFunction.Hash = hash;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setContenthashFunction, cancellationToken);
        }

        public UniTask<string> SetDNSRecordsRequestAsync(SetDNSRecordsFunction setDNSRecordsFunction)
        {
            return ContractHandler.SendRequestAsync(setDNSRecordsFunction);
        }

        public UniTask<TransactionReceipt> SetDNSRecordsRequestAndWaitForReceiptAsync(SetDNSRecordsFunction setDNSRecordsFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setDNSRecordsFunction, cancellationToken);
        }

        public UniTask<string> SetDNSRecordsRequestAsync(byte[] node, byte[] data)
        {
            var setDNSRecordsFunction = new SetDNSRecordsFunction();
            setDNSRecordsFunction.Node = node;
            setDNSRecordsFunction.Data = data;

            return ContractHandler.SendRequestAsync(setDNSRecordsFunction);
        }

        public UniTask<TransactionReceipt> SetDNSRecordsRequestAndWaitForReceiptAsync(byte[] node, byte[] data, CancellationToken cancellationToken = default)
        {
            var setDNSRecordsFunction = new SetDNSRecordsFunction();
            setDNSRecordsFunction.Node = node;
            setDNSRecordsFunction.Data = data;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setDNSRecordsFunction, cancellationToken);
        }

        public UniTask<string> SetInterfaceRequestAsync(SetInterfaceFunction setInterfaceFunction)
        {
            return ContractHandler.SendRequestAsync(setInterfaceFunction);
        }

        public UniTask<TransactionReceipt> SetInterfaceRequestAndWaitForReceiptAsync(SetInterfaceFunction setInterfaceFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setInterfaceFunction, cancellationToken);
        }

        public UniTask<string> SetInterfaceRequestAsync(byte[] node, byte[] interfaceID, string implementer)
        {
            var setInterfaceFunction = new SetInterfaceFunction();
            setInterfaceFunction.Node = node;
            setInterfaceFunction.InterfaceID = interfaceID;
            setInterfaceFunction.Implementer = implementer;

            return ContractHandler.SendRequestAsync(setInterfaceFunction);
        }

        public UniTask<TransactionReceipt> SetInterfaceRequestAndWaitForReceiptAsync(byte[] node, byte[] interfaceID, string implementer, CancellationToken cancellationToken = default)
        {
            var setInterfaceFunction = new SetInterfaceFunction();
            setInterfaceFunction.Node = node;
            setInterfaceFunction.InterfaceID = interfaceID;
            setInterfaceFunction.Implementer = implementer;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setInterfaceFunction, cancellationToken);
        }

        public UniTask<string> SetNameRequestAsync(SetNameFunction setNameFunction)
        {
            return ContractHandler.SendRequestAsync(setNameFunction);
        }

        public UniTask<TransactionReceipt> SetNameRequestAndWaitForReceiptAsync(SetNameFunction setNameFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setNameFunction, cancellationToken);
        }

        public UniTask<string> SetNameRequestAsync(byte[] node, string name)
        {
            var setNameFunction = new SetNameFunction();
            setNameFunction.Node = node;
            setNameFunction.Name = name;

            return ContractHandler.SendRequestAsync(setNameFunction);
        }

        public UniTask<TransactionReceipt> SetNameRequestAndWaitForReceiptAsync(byte[] node, string name, CancellationToken cancellationToken = default)
        {
            var setNameFunction = new SetNameFunction();
            setNameFunction.Node = node;
            setNameFunction.Name = name;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setNameFunction, cancellationToken);
        }

        public UniTask<string> SetPubkeyRequestAsync(SetPubkeyFunction setPubkeyFunction)
        {
            return ContractHandler.SendRequestAsync(setPubkeyFunction);
        }

        public UniTask<TransactionReceipt> SetPubkeyRequestAndWaitForReceiptAsync(SetPubkeyFunction setPubkeyFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setPubkeyFunction, cancellationToken);
        }

        public UniTask<string> SetPubkeyRequestAsync(byte[] node, byte[] x, byte[] y)
        {
            var setPubkeyFunction = new SetPubkeyFunction();
            setPubkeyFunction.Node = node;
            setPubkeyFunction.X = x;
            setPubkeyFunction.Y = y;

            return ContractHandler.SendRequestAsync(setPubkeyFunction);
        }

        public UniTask<TransactionReceipt> SetPubkeyRequestAndWaitForReceiptAsync(byte[] node, byte[] x, byte[] y, CancellationToken cancellationToken = default)
        {
            var setPubkeyFunction = new SetPubkeyFunction();
            setPubkeyFunction.Node = node;
            setPubkeyFunction.X = x;
            setPubkeyFunction.Y = y;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setPubkeyFunction, cancellationToken);
        }

        public UniTask<string> SetTextRequestAsync(SetTextFunction setTextFunction)
        {
            return ContractHandler.SendRequestAsync(setTextFunction);
        }

        public UniTask<TransactionReceipt> SetTextRequestAndWaitForReceiptAsync(SetTextFunction setTextFunction, CancellationToken cancellationToken = default)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(setTextFunction, cancellationToken);
        }

        public UniTask<string> SetTextRequestAsync(byte[] node, string key, string value)
        {
            var setTextFunction = new SetTextFunction();
            setTextFunction.Node = node;
            setTextFunction.Key = key;
            setTextFunction.Value = value;

            return ContractHandler.SendRequestAsync(setTextFunction);
        }

        public UniTask<TransactionReceipt> SetTextRequestAndWaitForReceiptAsync(byte[] node, string key, string value, CancellationToken cancellationToken = default)
        {
            var setTextFunction = new SetTextFunction();
            setTextFunction.Node = node;
            setTextFunction.Key = key;
            setTextFunction.Value = value;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(setTextFunction, cancellationToken);
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

        public UniTask<string> TextQueryAsync(TextFunction textFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TextFunction, string>(textFunction, blockParameter);
        }


        public UniTask<string> TextQueryAsync(byte[] node, string key, BlockParameter blockParameter = null)
        {
            var textFunction = new TextFunction();
            textFunction.Node = node;
            textFunction.Key = key;

            return ContractHandler.QueryAsync<TextFunction, string>(textFunction, blockParameter);
        }
#endif
    }

}
