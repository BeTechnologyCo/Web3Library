﻿using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Shh.KeyPair
{
    public interface IShhHasKeyPair : IGenericRpcRequestResponseHandlerParamString<bool>
    { 
    }
}
