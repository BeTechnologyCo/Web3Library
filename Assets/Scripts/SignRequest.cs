using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using System;


[Serializable]
public class SignRequest
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("message")]
    public RpcRequestMessage Message { get; set; }
    [JsonProperty("result")]
    public string Result { get; set; }
    [JsonProperty("deeplink")]
    public string DeepLink { get; set; }

    public SignRequest() { 
    }
}
