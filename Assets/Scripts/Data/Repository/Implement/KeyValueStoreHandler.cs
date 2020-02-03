using System;
using CAFU.KeyValueStore.Application.Interface;
using CAFU.KeyValueStore.Application.Utility;
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

        async UniTask<T> IKeyValueStore.GetValueType<T>(string key, T defaultValue, Func<string, ReferenceWrapper<T>> deserializeCallback)
        {
            return (await Getter.GetAsync(key, defaultValue.Wrap(), deserializeCallback)).Unwrap();
        }

        async UniTask<T> IKeyValueStore.GetReferenceType<T>(string key, T defaultValue, Func<string, T> deserializeCallback)
        {
            return await Getter.GetAsync(key, defaultValue, deserializeCallback);
        }

        async UniTask IKeyValueStore.SetValueType<T>(string key, T value, Func<ReferenceWrapper<T>, string> serializeCallback)
        {
            await Setter.SetAsync(key, value.Wrap(), serializeCallback);
        }

        async UniTask IKeyValueStore.SetReferenceType<T>(string key, T value, Func<T, string> serializeCallback)
        {
            await Setter.SetAsync(key, value, serializeCallback);
        }

        async UniTask<bool> IKeyValueStore.Has(string key)
        {
            return await Checker.Has(key);
        }
    }
}