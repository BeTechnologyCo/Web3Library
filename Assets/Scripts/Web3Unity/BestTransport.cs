using BestHTTP.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WalletConnectSharp.Core.Events;
using WalletConnectSharp.Core.Events.Request;
using WalletConnectSharp.Core.Events.Response;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Network;

namespace Web3Unity
{
    public class BestTransport : ITransport
    {
        private WebSocket client;
        private EventDelegator _eventDelegator;

        public bool Connected => client?.State == WebSocketStates.Open;

        public string URL { get; private set; }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public BestTransport()
        {
        }

        public BestTransport(EventDelegator eventDelegator)
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

        public Task Open(string url, bool clearSubscriptions = true)
        {
            if (url.StartsWith("https"))
                url = url.Replace("https", "wss");
            else if (url.StartsWith("http"))
                url = url.Replace("http", "ws");

            if (client != null)
                return Task.CompletedTask;

            this.URL = url;

            client = new WebSocket(new Uri(url));
            client.OnMessage += OnMessageReceived;
            client.Open();
            return Task.CompletedTask;
        }

        private void OnMessageReceived(WebSocket webSocket, string message)
        {
            Debug.Log("Text Message received from server: " + message);
            var json = message;

            var msg = JsonConvert.DeserializeObject<NetworkMessage>(json);

            Debug.Log("[WebSocket] Received message " + json);
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
            Debug.Log("[WebSocket] Send message " + finalJson);
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
