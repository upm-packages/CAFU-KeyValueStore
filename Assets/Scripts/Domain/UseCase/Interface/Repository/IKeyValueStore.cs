using System;
using UniRx.Async;

namespace CAFU.KeyValueStore.Domain.UseCase.Interface.Repository
{
    public interface IKeyValueStore
    {
        UniTask<T> Get<T>(string key, T defaultValue = default, Func<string, T> deserializeCallback = default);
        UniTask Set<T>(string key, T value, Func<T, string> serializeCallback = default);
        UniTask<bool> Has(string key);
    }
}