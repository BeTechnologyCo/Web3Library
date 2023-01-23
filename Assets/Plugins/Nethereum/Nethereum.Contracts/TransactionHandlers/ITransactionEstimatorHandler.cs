using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Nethereum.Contracts.TransactionHandlers
{
    public interface ITransactionEstimatorHandler<TFunctionMessage> where TFunctionMessage : FunctionMessage, new()
    {
        UniTask<HexBigInteger> EstimateGasAsync(string contractAddress, TFunctionMessage functionMessage = null);
    }
}