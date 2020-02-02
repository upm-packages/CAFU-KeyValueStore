using System;
using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Repository.Interface.DataStore
{
    public interface IAsyncGetter
    {
        UniTask<T> GetAsync<T>(string key, T defaultValue = default, Func<string, T> deserializeCallback = default);
    }
}