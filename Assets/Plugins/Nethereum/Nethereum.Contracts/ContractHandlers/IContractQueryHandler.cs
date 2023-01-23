using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.ContractHandlers
{
    public interface IContractQueryHandler<TFunctionMessage> where TFunctionMessage : FunctionMessage, new()
    {
        UniTask<TFunctionOutput> QueryAsync<TFunctionOutput>(string contractAddress, TFunctionMessage functionMessage = null, BlockParameter block = null);
        UniTask<TFunctionOutput> QueryDeserializingToObjectAsync<TFunctionOutput>(TFunctionMessage functionMessage, string contractAddress, BlockParameter block = null) where TFunctionOutput : IFunctionOutputDTO, new();
        UniTask<byte[]> QueryRawAsBytesAsync(string contractAddress, TFunctionMessage functionMessage = null, BlockParameter block = null);
        UniTask<string> QueryRawAsync(string contractAddress, TFunctionMessage functionMessage = null, BlockParameter block = null);
    }
}