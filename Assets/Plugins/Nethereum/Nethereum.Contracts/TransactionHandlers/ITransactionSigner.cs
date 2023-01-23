using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.Contracts.TransactionHandlers
{
    public interface ITransactionSigner<TFunctionMessage> where TFunctionMessage : FunctionMessage, new()
    {
        UniTask<string> SignTransactionAsync(string contractAddress, TFunctionMessage functionMessage = null);
    }
}