using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Web3Unity
{
    public enum MetamaskSignature
    {
        eth_sign = 0,
        personal_sign = 1,
        signTypedData_v1 = 2,
        signTypedData_v3 = 3,
        signTypedData_v4 = 4,
    }

}
