using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth
{
    public interface IEthGetStorageAt
    {
        BlockParameter DefaultBlock { get; set; }

        RpcRequest BuildRequest(string address, HexBigInteger position, BlockParameter block, object id = null);
        UniTask<string> SendRequestAsync(string address, HexBigInteger position, object id = null);
        UniTask<string> SendRequestAsync(string address, HexBigInteger position, BlockParameter block, object id = null);
    }
}