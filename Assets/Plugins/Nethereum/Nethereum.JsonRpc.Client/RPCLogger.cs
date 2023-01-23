
using System;
using Microsoft.Extensions.Logging;
using Nethereum.JsonRpc.Client.RpcMessages;

namespace Nethereum.JsonRpc.Client
{


    public class RpcLogger
    {
        public RpcLogger(ILogger log)
        {
            Log = log;
        }
        public ILogger Log { get; private set; }
        public string RequestJsonMessage { get; private set; }
        public RpcResponseMessage ResponseMessage { get; private set; }

        public void LogRequest(string requestJsonMessage)
        {
            if (Log != null)
            {
                RequestJsonMessage = requestJsonMessage;
                Log.LogTrace(GetRPCRequestLogMessage());
            }
        }

        private string GetRPCRequestLogMessage()
        {
            return $"RPC Request: {RequestJsonMessage}";
        }

        private string GetRPCResponseLogMessage()
        {
            return ResponseMessage != null ? $"RPC Response: {ResponseMessage.Result}" : String.Empty;
        }
        private bool IsLogErrorEnabled()
        {
            return Log != null && Log.IsEnabled(LogLevel.Error);
        }

        public void LogResponse(RpcResponseMessage responseMessage)
        {
            if (Log != null)
            {
                ResponseMessage = responseMessage;

                Log.LogTrace(GetRPCResponseLogMessage());

                if (HasError(responseMessage))
                {

                    Log.LogError($"RPC Response Error: {responseMessage.Error.Message}");
                }
            }
        }

        public void LogException(Exception ex)
        {
            if (Log != null)
            {
                Log.LogError(ex, "RPC Exception, " + GetRPCRequestLogMessage() + GetRPCResponseLogMessage());
            }
        }

        private bool HasError(RpcResponseMessage message)
        {
            return message.Error != null && message.HasError;
        }

        private bool IsLogTraceEnabled()
        {
            return Log != null && Log.IsEnabled(LogLevel.Trace);
        }

    }

}