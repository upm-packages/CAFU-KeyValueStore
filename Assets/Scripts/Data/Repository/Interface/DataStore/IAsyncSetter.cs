using System;
using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Repository.Interface.DataStore
{
    public interface IAsyncSetter
    {
        UniTask SetAsync<T>(string key, T value, Func<T, string> serializeCallback = default);
    }
}