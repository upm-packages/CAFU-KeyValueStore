using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.KeyValueStore.Data.Interface
{
    internal interface IAsyncSetter
    {
        UniTask SetAsync<T>(string key, T value, Func<T, string> serializeCallback = default, CancellationToken cancellationToken = default);
    }
}