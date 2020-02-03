using System;
using CAFU.KeyValueStore.Application.Utility;
using JetBrains.Annotations;
using UniRx.Async;

namespace CAFU.KeyValueStore.Application.Interface
{
    [PublicAPI]
    public interface IKeyValueStore
    {
        UniTask<T> GetValueType<T>(string key, T defaultValue = default, Func<string, ReferenceWrapper<T>> deserializeCallback = default) where T : struct;
        UniTask<T> GetReferenceType<T>(string key, T defaultValue = default, Func<string, T> deserializeCallback = default) where T : class;
        UniTask SetValueType<T>(string key, T value, Func<ReferenceWrapper<T>, string> serializeCallback = default) where T : struct;
        UniTask SetReferenceType<T>(string key, T value, Func<T, string> serializeCallback = default) where T : class;
        UniTask<bool> Has(string key);
    }
}