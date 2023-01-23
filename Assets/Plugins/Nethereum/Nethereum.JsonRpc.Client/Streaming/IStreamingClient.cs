using Cysharp.Threading.Tasks;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Nethereum.JsonRpc.Client.Streaming
{
    public interface IStreamingClient
    {
        bool IsStarted { get; }
        bool AddSubscription(string subscriptionId, IRpcStreamingResponseHandler handler);
        bool RemoveSubscription(string subscriptionId);
        UniTask SendRequestAsync(RpcRequest request, IRpcStreamingResponseHandler requestResponseHandler, string route = null);
        UniTask StartAsync();
        UniTask StopAsync();
    }
}