using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.KeyValueStore.Data.Interface
{
    internal interface IAsyncGetter
    {
        UniTask<T> GetAsync<T>(string key, T defaultValue = default, Func<string, T> deserializeCallback = default, CancellationToken cancellationToken = default);
    }
}