using System;
using System.Threading;
using CAFU.KeyValueStore.Application.Interface;
using CAFU.KeyValueStore.Data.Repository.Interface.DataStore;
using JetBrains.Annotations;
using UniRx.Async;
using Zenject;

namespace CAFU.KeyValueStore.Data.Repository.Implement
{
    [UsedImplicitly]
    internal class KeyValueStoreHandler : IKeyValueStore
    {
        [Inject]
        internal KeyValueStoreHandler(IAsyncGetter getter, IAsyncSetter setter, IAsyncChecker checker)
        {
            Getter = getter;
            Setter = setter;
            Checker = checker;
        }

        private IAsyncGetter Getter { get; }
        private IAsyncSetter Setter { get; }
        private IAsyncChecker Checker { get; }

        async UniTask<T> IKeyValueStore.Get<T>(string key, T defaultValue, Func<string, T> deserializeCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Getter.GetAsync(key, defaultValue, deserializeCallback, cancellationToken);
        }

        async UniTask IKeyValueStore.Set<T>(string key, T value, Func<T, string> serializeCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Setter.SetAsync(key, value, serializeCallback, cancellationToken);
        }

        async UniTask<bool> IKeyValueStore.Has(string key, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Checker.HasAsync(key, cancellationToken);
        }
    }
}