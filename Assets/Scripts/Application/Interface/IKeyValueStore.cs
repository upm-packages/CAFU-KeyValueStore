using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace CAFU.KeyValueStore.Application.Interface
{
    [PublicAPI]
    public interface IKeyValueStore
    {
        UniTask<T> Get<T>(string key, T defaultValue = default, Func<string, T> deserializeCallback = default, CancellationToken cancellationToken = default);
        UniTask Set<T>(string key, T value, Func<T, string> serializeCallback = default, CancellationToken cancellationToken = default);
        UniTask<bool> Has(string key, CancellationToken cancellationToken = default);
    }
}