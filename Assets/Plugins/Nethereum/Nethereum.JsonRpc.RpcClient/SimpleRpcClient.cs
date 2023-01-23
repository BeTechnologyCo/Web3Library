using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Nethereum.JsonRpc.Client
{
    public class SimpleRpcClient : ClientBase
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly Uri _baseUrl;

        public SimpleRpcClient(Uri baseUrl, HttpClient httpClient,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
                jsonSerializerSettings = DefaultJsonSerializerSettingsFactory.BuildDefaultJsonSerializerSettings();
            _jsonSerializerSettings = jsonSerializerSettings;
            _baseUrl = baseUrl;
        }

        protected override async UniTask<RpcResponseMessage> SendAsync(RpcRequestMessage request, string route = null)
        {
            string uri = new Uri(_baseUrl, route).AbsoluteUri;
            var rpcRequestJson = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
            var requestBytes = Encoding.UTF8.GetBytes(rpcRequestJson);
            using (var unityRequest = new UnityWebRequest(uri, "POST"))
            {
                var uploadHandler = new UploadHandlerRaw(requestBytes);
                unityRequest.SetRequestHeader("Content-Type", "application/json");
                uploadHandler.contentType = "application/json";
                unityRequest.uploadHandler = uploadHandler;

                unityRequest.downloadHandler = new DownloadHandlerBuffer();

                await unityRequest.SendWebRequest();

                if (unityRequest.error != null)
                {
#if DEBUG
                    Debug.Log(unityRequest.error);
#endif
                    throw new RpcClientUnknownException("Error occurred when trying to send rpc requests(s): " + request.Method, new Exception(unityRequest.error));
                }
                else
                {
                    try
                    {
                        byte[] results = unityRequest.downloadHandler.data;
                        var responseJson = Encoding.UTF8.GetString(results);
#if DEBUG
                        Debug.Log(responseJson);
#endif
                        return JsonConvert.DeserializeObject<RpcResponseMessage>(responseJson, _jsonSerializerSettings);
                    }
                    catch (Exception ex)
                    {
                        throw new RpcClientUnknownException("Error occurred when trying to send rpc requests(s): " + request.Method, ex);
#if DEBUG
                        Debug.Log(ex.Message);
#endif
                    }
                }

            }

        }

        protected override async UniTask<RpcResponseMessage[]> SendAsync(RpcRequestMessage[] requests)
        {
            var rpcRequestJson = JsonConvert.SerializeObject(requests, _jsonSerializerSettings);
            var requestBytes = Encoding.UTF8.GetBytes(rpcRequestJson);
            using (var unityRequest = new UnityWebRequest(_baseUrl.AbsoluteUri, "POST"))
            {
                var uploadHandler = new UploadHandlerRaw(requestBytes);
                unityRequest.SetRequestHeader("Content-Type", "application/json");
                uploadHandler.contentType = "application/json";
                unityRequest.uploadHandler = uploadHandler;

                unityRequest.downloadHandler = new DownloadHandlerBuffer();

                await unityRequest.SendWebRequest();

                if (unityRequest.error != null)
                {
#if DEBUG
                    Debug.Log(unityRequest.error);
#endif
                    throw new RpcClientUnknownException("Error occurred when trying to send rpc requests(s): ", new Exception(unityRequest.error));
                }
                else
                {
                    try
                    {
                        byte[] results = unityRequest.downloadHandler.data;
                        var responseJson = Encoding.UTF8.GetString(results);
#if DEBUG
                        Debug.Log(responseJson);
#endif
                        return JsonConvert.DeserializeObject<RpcResponseMessage[]>(responseJson, _jsonSerializerSettings);
                    }
                    catch (Exception ex)
                    {
                        throw new RpcClientUnknownException("Error occurred when trying to send rpc requests(s): ", ex);
#if DEBUG
                        Debug.Log(ex.Message);
#endif
                    }
                }

            }
        }
    }
}