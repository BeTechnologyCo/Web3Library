using BestHTTP.PlatformSupport.Memory;
using BestHTTP.WebSocket;
using Cysharp.Threading.Tasks;
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

        public bool Connected => client?.IsOpen == true;

        public string URL { get; private set; }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        UniTaskCompletionSource taskConnected;

        UniTaskCompletionSource sent;
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
            Debug.Log("start Open!");
            taskConnected = new UniTaskCompletionSource();

            if (url.StartsWith("https"))
                url = url.Replace("https", "wss");
            else if (url.StartsWith("http"))
                url = url.Replace("http", "ws");

            if (client != null)
                return Task.CompletedTask;

            this.URL = url;

            client = new WebSocket(new Uri(url));
            client.OnMessage += OnMessageReceived;
            client.OnBinaryNoAlloc += OnBinaryNoAlloc;
            client.OnOpen += OnWebSocketOpen;

            client.Open();
            Debug.Log("End Open!");
            return taskConnected.Task.AsTask();
        }

        private void OnWebSocketOpen(WebSocket webSocket)
        {
            Debug.Log("WebSocket is now Open!");
            taskConnected.TrySetResult();
        }


        private void OnBinaryNoAlloc(WebSocket webSocket, BufferSegment buffer)
        {
            Debug.Log("Binary Message received from server. Length: " + buffer.Count);
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
            sent = new UniTaskCompletionSource();
            var finalJson = JsonConvert.SerializeObject(message);
            Debug.Log("[WebSocket] Send message " + finalJson);
            client.Send(finalJson);
            
            Debug.Log("[WebSocket] Sent " + finalJson);
            sent.TrySetResult();
            return sent.Task.AsTask();
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
