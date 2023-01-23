using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Eth.Blocks
{
    public interface ILastConfirmedBlockNumberService
    {
        UniTask<BigInteger> GetLastConfirmedBlockNumberAsync(BigInteger? waitForConfirmedBlockNumber, CancellationToken cancellationToken);
    }
}