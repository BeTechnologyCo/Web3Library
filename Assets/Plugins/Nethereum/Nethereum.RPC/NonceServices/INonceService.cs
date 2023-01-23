using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;

namespace Nethereum.RPC.NonceServices
{
    public interface INonceService
    {
        IClient Client { get; set; }
        UniTask<HexBigInteger> GetNextNonceAsync();
        UniTask ResetNonceAsync();
    }
}