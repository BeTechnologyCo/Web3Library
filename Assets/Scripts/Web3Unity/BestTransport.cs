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

        private Queue<NetworkMessage> _queuedMessages = new Queue<NetworkMessage>();
        private List<string> subscribedTopics = new List<string>();

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
            Debug.Log("[WebSocket] Subs Cleared");
            subscribedTopics.Clear();
            _queuedMessages.Clear();
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
            if (clearSubscriptions)
            {
                ClearSubscriptions();
            }
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
            QueueSubscriptions();
            FlushQueue();
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


        public async Task SendMessage(NetworkMessage message)
        {
            sent = new UniTaskCompletionSource();

            if (!Connected)
            {
                _queuedMessages.Enqueue(message);
                await Open(URL);
            }
            else
            {
                var finalJson = JsonConvert.SerializeObject(message);
                Debug.Log("[WebSocket] Send message " + finalJson);
                client.Send(finalJson);

                Debug.Log("[WebSocket] Sent " + finalJson);
                sent.TrySetResult();
            }

            sent.Task.AsTask();
        }

        public async Task Subscribe(string topic)
        {
            Debug.Log("[WebSocket] Subscribe " + topic);
            var msg = GenerateSubscribeMessage(topic);
            await SendMessage(msg);
            //sent.Task.AsTask();
        }

        public async Task Subscribe<T>(string topic, EventHandler<JsonRpcResponseEvent<T>> callback) where T : JsonRpcResponse
        {
            Debug.Log("[WebSocket] Subscribe Response " + topic);
            await Subscribe(topic);

            _eventDelegator.ListenFor(topic, callback);
        }

        public async Task Subscribe<T>(string topic, EventHandler<JsonRpcRequestEvent<T>> callback) where T : JsonRpcRequest
        {
            Debug.Log("[WebSocket] Subscribe Response " + topic);
            await Subscribe(topic);
            _eventDelegator.ListenFor(topic, callback);
        }

        private async void FlushQueue()
        {
            Debug.Log("[WebSocket] Flushing Queue");
            Debug.Log("[WebSocket] Queue Count: " + _queuedMessages.Count);
            while (_queuedMessages.Count > 0)
            {
                var msg = _queuedMessages.Dequeue();
                await SendMessage(msg);
            }

            Debug.Log("[WebSocket] Queue Flushed");
        }

        private void QueueSubscriptions()
        {
            foreach (var topic in subscribedTopics)
            {
                this._queuedMessages.Enqueue(GenerateSubscribeMessage(topic));
            }

            Debug.Log("[WebSocket] Queued " + subscribedTopics.Count + " subscriptions");
        }

        private NetworkMessage GenerateSubscribeMessage(string topic)
        {
            return new NetworkMessage()
            {
                Payload = "",
                Type = "sub",
                Silent = true,
                Topic = topic
            };
        }
    }
}
