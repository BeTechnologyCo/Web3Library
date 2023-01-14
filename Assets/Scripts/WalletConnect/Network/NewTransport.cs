using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;
using WalletConnectSharp.Core.Events;
using WalletConnectSharp.Core.Events.Request;
using WalletConnectSharp.Core.Events.Response;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Network;

namespace WalletConnectSharp.Unity.Network
{
    public class NewTransport : ITransport
    {
        private bool opened = false;
        private bool closed = false;

        private WebSocket client;
        private EventDelegator _eventDelegator;
        private bool wasPaused;
        private string currentUrl;
        private List<string> subscribedTopics = new List<string>();

        public bool Connected
        {
            get
            {
                return client != null && (client.State == WebSocketState.Open || client.State == WebSocketState.Closing) && opened;
            }
        }

        public NewTransport()
        {

        }

        public NewTransport(EventDelegator eventDelegator)
        {
            this._eventDelegator = eventDelegator;
        }


        public void AttachEventDelegator(EventDelegator eventDelegator)
        {
            this._eventDelegator = eventDelegator;
        }

        public void Dispose()
        {
            if (client != null)
            {
                client.CancelConnection();
            }
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<MessageReceivedEventArgs> OpenReceived;

        public string URL
        {
            get
            {
                return currentUrl;
            }
        }

        public async Task Open(string url, bool clearSubscriptions = true)
        {
            if (currentUrl != url || clearSubscriptions)
            {
                ClearSubscriptions();
            }

            currentUrl = url;

            await _socketOpen();
        }

        private async Task _socketOpen()
        {
            if (client != null)
            {
                return;
            }

            string url = currentUrl;
            if (url.StartsWith("https"))
                url = url.Replace("https", "wss");
            else if (url.StartsWith("http"))
                url = url.Replace("http", "ws");

            client = new WebSocket(url);

            TaskCompletionSource<bool> eventCompleted = new TaskCompletionSource<bool>(TaskCreationOptions.None);

            client.OnOpen += () =>
            {
                // subscribe now
                if (this.OpenReceived != null)
                    OpenReceived(this, null);

                Debug.Log("[WebSocket] Opened " + url);

                eventCompleted.SetResult(true);
            };

            client.OnMessage += OnMessageReceived;
            client.OnClose += ClientTryReconnect;
            client.OnError += (e) =>
            {

                Debug.Log("[WebSocket] OnError " + e);
                HandleError(new Exception(e));
            };

            client.Connect().ContinueWith(t => HandleError(t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            Debug.Log("[WebSocket] Waiting for Open " + url);
            await eventCompleted.Task;
            Debug.Log("[WebSocket] Open Completed");
        }

        private void HandleError(Exception e)
        {
            Debug.LogError(e);
        }




        private async void ClientTryReconnect(WebSocketCloseCode closeCode)
        {
            if (wasPaused)
            {
                Debug.Log("[WebSocket] Application paused, retry attempt aborted");
                return;
            }

            client = null;
            await _socketOpen();
        }

        public void CancelConnection()
        {
            client.CancelConnection();
        }


        private async void OnMessageReceived(byte[] bytes)
        {
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("[WebSocket] Received message " + json);
            try
            {
                var msg = JsonConvert.DeserializeObject<NetworkMessage>(json);


                await SendMessage(new NetworkMessage()
                {
                    Payload = "",
                    Type = "ack",
                    Silent = true,
                    Topic = msg.Topic
                });

                if (this.MessageReceived != null)
                    MessageReceived(this, new MessageReceivedEventArgs(msg, this));
            }
            catch (Exception e)
            {
                Debug.Log("[WebSocket] Exception " + e.Message);
            }
        }

        public async Task Close()
        {
            Debug.Log("Closing Websocket");
            try
            {
                if (client != null)
                {
                    this.opened = false;
                    client.OnClose -= ClientTryReconnect;
                    await client.Close();
                }
            }
            catch (WebSocketInvalidStateException e)
            {
                if (e.Message.Contains("WebSocket is not connected"))
                    Debug.LogWarning("Tried to close a websocket when it's already closed");
                else
                    throw;
            }
        }

        public async Task SendMessage(NetworkMessage message)
        {
            string finalJson = JsonConvert.SerializeObject(message);
            Debug.Log("[WebSocket] Send message " + finalJson);
            await this.client.SendText(finalJson);

        }

        public async Task Subscribe(string topic)
        {
            Debug.Log("[WebSocket] Subscribe to " + topic);

            var msg = GenerateSubscribeMessage(topic);

            await SendMessage(msg);

            if (!subscribedTopics.Contains(topic))
            {
                subscribedTopics.Add(topic);
            }

            opened = true;
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

        public async Task Subscribe<T>(string topic, EventHandler<JsonRpcResponseEvent<T>> callback) where T : JsonRpcResponse
        {
            await Subscribe(topic);

            _eventDelegator.ListenFor(topic, callback);
        }

        public async Task Subscribe<T>(string topic, EventHandler<JsonRpcRequestEvent<T>> callback) where T : JsonRpcRequest
        {
            await Subscribe(topic);

            _eventDelegator.ListenFor(topic, callback);
        }

        public void ClearSubscriptions()
        {
            Debug.Log("[WebSocket] Subs Cleared");
            subscribedTopics.Clear();
        }

        //#if UNITY_IOS
        private IEnumerator OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("[WebSocket] Pausing");
                wasPaused = true;

                //We need to close the Websocket Properly
                var closeTask = Task.Run(Close);
                var coroutineInstruction = new WaitForTask(closeTask);
                yield return coroutineInstruction;
            }
            else if (wasPaused)
            {
                Debug.Log("[WebSocket] Resuming");
                var openTask = Task.Run(() => Open(currentUrl, false));
                var coroutineInstruction = new WaitForTask(openTask);
                yield return coroutineInstruction;

                foreach (var topic in subscribedTopics)
                {
                    var subTask = Task.Run(() => Subscribe(topic));
                    var coroutineSubInstruction = new WaitForTask(subTask);
                    yield return coroutineSubInstruction;
                }
            }
        }
        //#endif
    }
}