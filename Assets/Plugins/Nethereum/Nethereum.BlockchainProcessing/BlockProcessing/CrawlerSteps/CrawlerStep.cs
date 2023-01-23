using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Services;

namespace Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps
{
    public abstract class CrawlerStep<TParentStep, TProcessStep>
    {
        public bool Enabled { get; set; } = true;
        protected IEthApiContractService EthApi { get; }
        public CrawlerStep(
            IEthApiContractService ethApi
        )
        {
            EthApi = ethApi;
        }

        public abstract UniTask<TProcessStep> GetStepDataAsync(TParentStep parentStep);

        public virtual async UniTask<CrawlerStepCompleted<TProcessStep>> ExecuteStepAsync(TParentStep parentStep, IEnumerable<BlockProcessingSteps> executionStepsCollection)
        {
            if (!Enabled) throw new Exception("Crawler step is not enabled");
            var processStepValue = await GetStepDataAsync(parentStep);
            if (processStepValue == null) return null;
            var stepsToProcesss =
                await executionStepsCollection.FilterMatchingStepAsync(processStepValue);

            if (stepsToProcesss.Any())
            {
                await stepsToProcesss.ExecuteCurrentStepAsync(processStepValue);
            }
            return new CrawlerStepCompleted<TProcessStep>(stepsToProcesss, processStepValue);

        }
    }
}