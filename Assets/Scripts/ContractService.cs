using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Web3Library.Contract.ContractDefinition;

namespace Web3Library.Contract
{
    public partial class ContractService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ContractDeployment contractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ContractDeployment>().SendRequestAndWaitForReceiptAsync(contractDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ContractDeployment contractDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ContractDeployment>().SendRequestAsync(contractDeployment);
        }

        public static async Task<ContractService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ContractDeployment contractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, contractDeployment, cancellationTokenSource);
            return new ContractService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ContractService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public ContractService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<List<byte>> GetResultsForPlayerQueryAsync(GetResultsForPlayerFunction getResultsForPlayerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetResultsForPlayerFunction, List<byte>>(getResultsForPlayerFunction, blockParameter);
        }

        
        public Task<List<byte>> GetResultsForPlayerQueryAsync(string player, BlockParameter blockParameter = null)
        {
            var getResultsForPlayerFunction = new GetResultsForPlayerFunction();
                getResultsForPlayerFunction.Player = player;
            
            return ContractHandler.QueryAsync<GetResultsForPlayerFunction, List<byte>>(getResultsForPlayerFunction, blockParameter);
        }

        public Task<string> PlayRequestAsync(PlayFunction playFunction)
        {
             return ContractHandler.SendRequestAsync(playFunction);
        }

        public Task<TransactionReceipt> PlayRequestAndWaitForReceiptAsync(PlayFunction playFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(playFunction, cancellationToken);
        }

        public Task<string> PlayRequestAsync(byte playerChoice)
        {
            var playFunction = new PlayFunction();
                playFunction.PlayerChoice = playerChoice;
            
             return ContractHandler.SendRequestAsync(playFunction);
        }

        public Task<TransactionReceipt> PlayRequestAndWaitForReceiptAsync(byte playerChoice, CancellationTokenSource cancellationToken = null)
        {
            var playFunction = new PlayFunction();
                playFunction.PlayerChoice = playerChoice;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(playFunction, cancellationToken);
        }
    }
}
