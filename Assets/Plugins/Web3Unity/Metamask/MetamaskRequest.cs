using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using System;

namespace Web3Unity
{

    [JsonObject]
    public class MetamaskRequest : RpcRequestMessage
    {
        public MetamaskRequest(object id, string method, string from, params object[] parameterList) : base(id, method,
              parameterList)
        {
            From = from;
        }

        [JsonProperty("from")]
        public string From { get; private set; }
    }

}