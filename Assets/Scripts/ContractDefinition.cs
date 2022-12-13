using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Web3Library.Contract.ContractDefinition
{


    public partial class ContractDeployment : ContractDeploymentBase
    {
        public ContractDeployment() : base(BYTECODE) { }
        public ContractDeployment(string byteCode) : base(byteCode) { }
    }

    public class ContractDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public ContractDeploymentBase() : base(BYTECODE) { }
        public ContractDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetResultsForPlayerFunction : GetResultsForPlayerFunctionBase { }

    [Function("getResultsForPlayer", "uint8[]")]
    public class GetResultsForPlayerFunctionBase : FunctionMessage
    {
        [Parameter("address", "player", 1)]
        public virtual string Player { get; set; }
    }

    public partial class PlayFunction : PlayFunctionBase { }

    [Function("play")]
    public class PlayFunctionBase : FunctionMessage
    {
        [Parameter("uint8", "playerChoice", 1)]
        public virtual byte PlayerChoice { get; set; }
    }

    public partial class PartyResultEventDTO : PartyResultEventDTOBase { }

    [Event("PartyResult")]
    public class PartyResultEventDTOBase : IEventDTO
    {
        [Parameter("address", "player", 1, true )]
        public virtual string Player { get; set; }
        [Parameter("uint8", "result", 2, false )]
        public virtual byte Result { get; set; }
        [Parameter("uint8", "playerChoice", 3, false )]
        public virtual byte PlayerChoice { get; set; }
        [Parameter("uint8", "chainChoice", 4, false )]
        public virtual byte ChainChoice { get; set; }
    }

    public partial class GetResultsForPlayerOutputDTO : GetResultsForPlayerOutputDTOBase { }

    [FunctionOutput]
    public class GetResultsForPlayerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8[]", "", 1)]
        public virtual List<byte> ReturnValue1 { get; set; }
    }


}
