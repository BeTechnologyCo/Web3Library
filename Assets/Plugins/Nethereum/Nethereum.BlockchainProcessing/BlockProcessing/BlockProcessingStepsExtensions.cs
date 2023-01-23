using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.BlockchainProcessing.Processor;

namespace Nethereum.BlockchainProcessing.BlockProcessing
{
    public static class BlockProcessingStepsExtensions
    {
        public static async UniTask<bool> HasAnyStepMatchAsync<T>(this IEnumerable<BlockProcessingSteps> list,
            T value)
        {
            foreach (var item in list)
            {
                if (await item.GetStep<T>().IsMatchAsync(value))
                    return true;
            }

            return false;
        }

        public static async UniTask<bool> IsStepMatchAsync<T>(this IEnumerable<IProcessor<T>> list, T value)
        {
            foreach (var item in list)
            {
                if (await item.IsMatchAsync(value))
                    return true;
            }

            return false;
        }

        public static IEnumerable<IProcessor<T>> GetAllSteps<T>(
            this IEnumerable<BlockProcessingSteps> list)
        {
            return list.Select(x => x.GetStep<T>());
        }

        public static async UniTask<IEnumerable<BlockProcessingSteps>> FilterMatchingStepAsync<T>(
            this IEnumerable<BlockProcessingSteps> list, T value)
        {
            var listResult = new List<BlockProcessingSteps>();
            foreach (var item in list)
            {
                if (await item.GetStep<T>().IsMatchAsync(value))
                    listResult.Add(item);
            }

            return listResult;
        }

        public static async UniTask ExecuteCurrentStepAsync<T>(
            this IEnumerable<BlockProcessingSteps> list, T value)
        {
            var steps = list.GetAllSteps<T>();
            foreach (var step in steps)
            {
                await step.ExecuteAsync(value);
            }
        }
        
    }
}