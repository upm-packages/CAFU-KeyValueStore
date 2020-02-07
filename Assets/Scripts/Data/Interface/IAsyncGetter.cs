using System;
using System.Threading;
using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Repository.Interface.DataStore
{
    internal interface IAsyncGetter
    {
        UniTask<T> GetAsync<T>(string key, T defaultValue = default, Func<string, T> deserializeCallback = default, CancellationToken cancellationToken = default);
    }
}