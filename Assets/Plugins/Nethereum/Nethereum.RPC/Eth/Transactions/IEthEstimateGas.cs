using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthEstimateGas
    {
        RpcRequest BuildRequest(CallInput callInput, object id = null);
        UniTask<HexBigInteger> SendRequestAsync(CallInput callInput, object id = null);
    }
}