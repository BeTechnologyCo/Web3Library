using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.Transactions
{
    public interface IEthGetTransactionByBlockNumberAndIndex
    {
        RpcRequest BuildRequest(HexBigInteger blockNumber, HexBigInteger transactionIndex, object id = null);
        UniTask<Transaction> SendRequestAsync(HexBigInteger blockNumber, HexBigInteger transactionIndex, object id = null);
    }
}