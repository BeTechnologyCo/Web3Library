using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Fee1559Suggestions
{

    public interface IFee1559SuggestionStrategy
    {
#if !DOTNET35
        UniTask<Fee1559> SuggestFeeAsync(BigInteger? maxPriorityFeePerGas = null);
#endif
    }

}