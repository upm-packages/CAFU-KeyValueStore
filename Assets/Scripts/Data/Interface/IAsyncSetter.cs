using System;
using System.Threading;
using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Repository.Interface.DataStore
{
    internal interface IAsyncSetter
    {
        UniTask SetAsync<T>(string key, T value, Func<T, string> serializeCallback = default, CancellationToken cancellationToken = default);
    }
}