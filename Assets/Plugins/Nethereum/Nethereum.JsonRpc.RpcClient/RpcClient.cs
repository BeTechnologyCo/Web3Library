using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static Nethereum.JsonRpc.Client.UserAuthentication;

namespace Nethereum.JsonRpc.Client
{
    public class RpcClient : ClientBase
    {
        private const int NUMBER_OF_SECONDS_TO_RECREATE_HTTP_CLIENT = 60;
        public static int MaximumConnectionsPerServer { get; set; } = 20;
        private readonly Uri _baseUrl;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public Dictionary<string, string> RequestHeaders { get; set; } = new Dictionary<string, string>();

        public RpcClient(Uri baseUrl, AuthenticationHeaderValue authHeaderValue = null,
            JsonSerializerSettings jsonSerializerSettings = null, HttpClientHandler httpClientHandler = null, Microsoft.Extensions.Logging.ILogger log = null)
        {
            _baseUrl = baseUrl;

            if (authHeaderValue == null)
            {
                SetBasicAuthenticationHeaderFromUri(baseUrl);
            }

            if (jsonSerializerSettings == null)
                jsonSerializerSettings = DefaultJsonSerializerSettingsFactory.BuildDefaultJsonSerializerSettings();

            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public BasicAuthenticationUserInfo GetBasicAuthenticationUserInfoFromUri(Uri uri)
        {
            if (uri.UserInfo != String.Empty)
            {
                var userInfo = uri.UserInfo?.Split(':');
                if (userInfo.Length == 2)
                {
                    var userName = userInfo[0];
                    var password = userInfo[1];
                    return new BasicAuthenticationUserInfo() { UserName = userName, Password = password };
                }
            }

            return null;
        }

        public void SetBasicAuthenticationHeaderFromUri(Uri uri)
        {
            var userInfo = GetBasicAuthenticationUserInfoFromUri(uri);
            if (userInfo != null)
            {
                SetBasicAuthenticationHeader(userInfo.UserName, userInfo.Password);
            }
        }

        public void SetBasicAuthenticationHeader(string userName, string password)
        {
            RequestHeaders.Add("AUTHORIZATION", GetBasicAuthentication(userName, password));
        }

        public RpcClient(Uri baseUrl, HttpClient httpClient, AuthenticationHeaderValue authHeaderValue = null,
           JsonSerializerSettings jsonSerializerSettings = null, Microsoft.Extensions.Logging.ILogger log = null)
        {
            _baseUrl = baseUrl;

            if (authHeaderValue == null)
            {
                SetBasicAuthenticationHeaderFromUri(baseUrl);
            }


            if (jsonSerializerSettings == null)
                jsonSerializerSettings = DefaultJsonSerializerSettingsFactory.BuildDefaultJsonSerializerSettings();
            _jsonSerializerSettings = jsonSerializerSettings;
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
                if (RequestHeaders != null)
                {
                    foreach (var requestHeader in RequestHeaders)
                    {
                        unityRequest.SetRequestHeader(requestHeader.Key, requestHeader.Value);
                    }
                }

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

                if (RequestHeaders != null)
                {
                    foreach (var requestHeader in RequestHeaders)
                    {
                        unityRequest.SetRequestHeader(requestHeader.Key, requestHeader.Value);
                    }
                }

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




    }
}