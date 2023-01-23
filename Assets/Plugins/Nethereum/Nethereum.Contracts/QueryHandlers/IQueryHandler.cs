using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.QueryHandlers
{
    public interface IQueryHandler<TFunctionMessage, TOutput> 
        where TFunctionMessage : FunctionMessage, new()
    {
        UniTask<TOutput> QueryAsync(
             string contractAddress,
             TFunctionMessage functionMessage = null,
             BlockParameter block = null);
    }
}