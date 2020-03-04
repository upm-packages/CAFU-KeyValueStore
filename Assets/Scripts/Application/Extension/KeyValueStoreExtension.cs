using System.Threading;
using CAFU.KeyValueStore.Application.Interface;
using JetBrains.Annotations;
using UniRx.Async;

namespace CAFU.KeyValueStore.Application.Extension
{
    [PublicAPI]
    public static class KeyValueStoreExtension
    {
        public static async UniTask Add(this IKeyValueStore keyValueStore, string key, int value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) + value, cancellationToken: cancellationToken);
        }

        public static async UniTask Subtract(this IKeyValueStore keyValueStore, string key, int value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) - value, cancellationToken: cancellationToken);
        }

        public static async UniTask Multiply(this IKeyValueStore keyValueStore, string key, int value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) * value, cancellationToken: cancellationToken);
        }

        public static async UniTask Divide(this IKeyValueStore keyValueStore, string key, int value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) / value, cancellationToken: cancellationToken);
        }

        public static async UniTask Increment(this IKeyValueStore keyValueStore, string key, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Add(key, 1, cancellationToken);
        }

        public static async UniTask Decrement(this IKeyValueStore keyValueStore, string key, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Subtract(key, 1, cancellationToken);
        }

        public static async UniTask Add(this IKeyValueStore keyValueStore, string key, float value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) + value, cancellationToken: cancellationToken);
        }

        public static async UniTask Subtract(this IKeyValueStore keyValueStore, string key, float value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) - value, cancellationToken: cancellationToken);
        }

        public static async UniTask Multiply(this IKeyValueStore keyValueStore, string key, float value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) * value, cancellationToken: cancellationToken);
        }

        public static async UniTask Divide(this IKeyValueStore keyValueStore, string key, float value, CancellationToken cancellationToken = default)
        {
            await keyValueStore.Set(key, await keyValueStore.Get(key, 0, cancellationToken: cancellationToken) / value, cancellationToken: cancellationToken);
        }
    }
}