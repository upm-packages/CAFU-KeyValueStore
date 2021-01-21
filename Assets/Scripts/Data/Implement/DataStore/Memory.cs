using System;
using System.Collections.Generic;
using System.Threading;
using CAFU.KeyValueStore.Data.Interface;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Zenject;

namespace CAFU.KeyValueStore.Data.Implement.DataStore
{
    [UsedImplicitly]
    internal class Memory : IAsyncGetter, IAsyncSetter, IAsyncChecker
    {
        [Inject]
        internal Memory()
        {
        }

        private IDictionary<string, object> Storage { get; } = new Dictionary<string, object>();

        async UniTask<T> IAsyncGetter.GetAsync<T>(string key, T defaultValue, Func<string, T> deserializeCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await ((IAsyncChecker) this).HasAsync(key, cancellationToken))
            {
                return await UniTask.FromResult((T) Storage[key]);
            }

            return await UniTask.FromResult(defaultValue);
        }

        async UniTask IAsyncSetter.SetAsync<T>(string key, T value, Func<T, string> serializeCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Storage[key] = value;

            await UniTask.CompletedTask;
        }

        async UniTask<bool> IAsyncChecker.HasAsync(string key, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await UniTask.FromResult(Storage.ContainsKey(key));
        }
    }
}