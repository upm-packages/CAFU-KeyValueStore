using System;
using System.Threading;
using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Interface
{
    internal interface IAsyncSetter
    {
        UniTask SetAsync<T>(string key, T value, Func<T, string> serializeCallback = default, CancellationToken cancellationToken = default);
    }
}