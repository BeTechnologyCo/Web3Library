using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using WalletConnectSharp.Core.Events.Request;
using WalletConnectSharp.Core.Events.Response;
using WalletConnectSharp.Core.Models;

namespace WalletConnectSharp.Core.Network
{
    public interface ITransport : IDisposable
    {
        bool Connected { get; }

        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        string URL { get; }

        UniTask Open(string bridgeURL, bool clearSubscriptions = true);

        UniTask Close();

        UniTask SendMessage(NetworkMessage message);

        UniTask Subscribe(string topic);

        UniTask Subscribe<T>(string topic, EventHandler<JsonRpcResponseEvent<T>> callback) where T : JsonRpcResponse;

        UniTask Subscribe<T>(string topic, EventHandler<JsonRpcRequestEvent<T>> callback) where T : JsonRpcRequest;

        void ClearSubscriptions();
    }
}