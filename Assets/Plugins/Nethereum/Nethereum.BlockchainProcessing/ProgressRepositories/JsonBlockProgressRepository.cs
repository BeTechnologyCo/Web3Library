using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.ProgressRepositories
{
    public class JsonBlockProgressRepository : IBlockProgressRepository
    {
        public class BlockProcessingProgress
        {
            public BigInteger? To { get; set; }
            public DateTimeOffset? LastUpdatedUTC { get; set; }
        }

        private BlockProcessingProgress _progress;
        private readonly Func<UniTask<bool>> _jsonSourceExists;
        private readonly Func<string, UniTask> _jsonWriter;
        private readonly Func<UniTask<string>> _jsonReader;
        private readonly BigInteger? _initialBlockNumber;

        public JsonBlockProgressRepository(
            Func<UniTask<bool>> jsonSourceExists,
            Func<string, UniTask> jsonWriter,
            Func<UniTask<string>> jsonRetriever,
            BigInteger? lastBlockProcessed = null)
        {
            this._jsonSourceExists = jsonSourceExists;
            _jsonWriter = jsonWriter;
            _jsonReader = jsonRetriever;
            _initialBlockNumber = lastBlockProcessed;
        }

        public async UniTask<BigInteger?> GetLastBlockNumberProcessedAsync()
        {
            await InitialiseAsync();
            return _progress.To;
        }

        public async UniTask UpsertProgressAsync(BigInteger blockNumber)
        {
            await InitialiseAsync();
            _progress.LastUpdatedUTC = DateTimeOffset.UtcNow;
            _progress.To = blockNumber;
            await PersistAsync();
        }

        private async UniTask InitialiseAsync()
        {
            if (_progress != null) return;

            _progress = await LoadAsync();

            if (_progress == null)
            {
                _progress = new BlockProcessingProgress { To = _initialBlockNumber };
                await PersistAsync();
            }
            else
            {
                if (_initialBlockNumber != null) // we've been given a starting point
                {
                    if (_progress.To == null || _progress.To < _initialBlockNumber)
                    {
                        await UpsertProgressAsync(_initialBlockNumber.Value);
                    }
                }
            }
        }

        private async UniTask<BlockProcessingProgress> LoadAsync()
        {
            if (await _jsonSourceExists.Invoke() == false) return null;

            var content = await _jsonReader.Invoke();
            if (content == null) return null;
            return JsonConvert.DeserializeObject<BlockProcessingProgress>(content);
        }

        private async UniTask PersistAsync()
        {
            await _jsonWriter.Invoke(JsonConvert.SerializeObject(_progress));
        }
    }
}
