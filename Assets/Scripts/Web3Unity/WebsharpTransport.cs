using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WalletConnectSharp.Core.Events;
using WalletConnectSharp.Core.Events.Request;
using WalletConnectSharp.Core.Events.Response;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Network;
using WebSocketSharp;

namespace Web3Unity
{
    public class WebsharpTransport : ITransport
    {
        private WebSocket client;
        private EventDelegator _eventDelegator;

        public bool Connected => client?.ReadyState == WebSocketState.Open;

        public string URL { get; private set; }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public WebsharpTransport()
        {
        }

        public WebsharpTransport(EventDelegator eventDelegator)
        {
            this._eventDelegator = eventDelegator;
        }

        public void ClearSubscriptions()
        {

        }

        public Task Close()
        {
            client.Close();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (client != null)
                client.Close();
        }

        public async Task Open(string url, bool clearSubscriptions = true)
        {
            if (url.StartsWith("https"))
                url = url.Replace("https", "wss");
            else if (url.StartsWith("http"))
                url = url.Replace("http", "ws");

            if (client != null)
                return;

            this.URL = url;

            client = new WebSocket(url);
            client.OnMessage += Client_OnMessage;
            client.Connect();

        }

        private void Client_OnMessage(object sender, MessageEventArgs e)
        {
            var json = e.Data;

            var msg = JsonConvert.DeserializeObject<NetworkMessage>(json);

            SendMessage(new NetworkMessage()
            {
                Payload = "",
                Type = "ack",
                Silent = true,
                Topic = msg.Topic
            });


            if (this.MessageReceived != null)
                MessageReceived(this, new MessageReceivedEventArgs(msg, this));
        }


        public Task SendMessage(NetworkMessage message)
        {
            var finalJson = JsonConvert.SerializeObject(message);

            client.Send(finalJson);

            return Task.CompletedTask;
        }

        public Task Subscribe(string topic)
        {
            return SendMessage(new NetworkMessage()
            {
                Payload = "",
                Type = "sub",
                Silent = true,
                Topic = topic
            });
        }

        public Task Subscribe<T>(string topic, EventHandler<JsonRpcResponseEvent<T>> callback) where T : JsonRpcResponse
        {
            Subscribe(topic);

            _eventDelegator.ListenFor(topic, callback);

            return Task.CompletedTask;
        }

        public Task Subscribe<T>(string topic, EventHandler<JsonRpcRequestEvent<T>> callback) where T : JsonRpcRequest
        {
            Subscribe(topic);

            _eventDelegator.ListenFor(topic, callback);

            return Task.CompletedTask;
        }
    }
}
