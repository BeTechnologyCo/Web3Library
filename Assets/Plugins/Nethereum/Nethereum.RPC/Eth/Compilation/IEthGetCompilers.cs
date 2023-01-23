using Nethereum.RPC.Infrastructure;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Eth.Compilation
{
    public interface IEthGetCompilers: IGenericRpcRequestResponseHandlerNoParam<string[]>
    {
       
    }
}