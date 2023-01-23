using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.Contracts.TransactionHandlers
{
    public interface ITransactionSenderHandler<TFunctionMessage> where TFunctionMessage : FunctionMessage, new()
    {
        UniTask<string> SendTransactionAsync(string contractAddress, TFunctionMessage functionMessage = null);
    }
}