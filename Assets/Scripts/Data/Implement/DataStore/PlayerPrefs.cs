using System;
using System.Threading;
using CAFU.KeyValueStore.Data.Interface;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Zenject;

namespace CAFU.KeyValueStore.Data.Implement.DataStore
{
    [UsedImplicitly]
    internal class PlayerPrefs : IAsyncGetter, IAsyncSetter, IAsyncChecker
    {
        [Inject]
        internal PlayerPrefs()
        {
        }

        async UniTask<T> IAsyncGetter.GetAsync<T>(string key, T defaultValue, Func<string, T> deserializeCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!UnityEngine.PlayerPrefs.HasKey(key))
            {
                return await UniTask.FromResult(defaultValue);
            }

            T result;
            if (typeof(T) == typeof(bool))
            {
                result = (T) (object) (UnityEngine.PlayerPrefs.GetInt(key, (bool)(object) defaultValue ? 1 : 0) == 1);
            }
            else if (typeof(T) == typeof(int))
            {
                result = (T) (object) UnityEngine.PlayerPrefs.GetInt(key, (int) (object) defaultValue);
            }
            else if (typeof(T) == typeof(float))
            {
                result = (T) (object) UnityEngine.PlayerPrefs.GetFloat(key, (float) (object) defaultValue);
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T) (object) UnityEngine.PlayerPrefs.GetString(key, (string) (object) defaultValue);
            }
            else
            {
                if (deserializeCallback == default)
                {
                    throw new ArgumentException($"Need third argument to get type of {typeof(T)}");
                }

                result = deserializeCallback(UnityEngine.PlayerPrefs.GetString(key));
            }

            return await UniTask.FromResult(result);
        }

        UniTask IAsyncSetter.SetAsync<T>(string key, T value, Func<T, string> serializeCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (value)
            {
                case bool v:
                    UnityEngine.PlayerPrefs.SetInt(key, v ? 1 : 0);
                    break;
                case int v:
                    UnityEngine.PlayerPrefs.SetInt(key, v);
                    break;
                case float v:
                    UnityEngine.PlayerPrefs.SetFloat(key, v);
                    break;
                case string v:
                    UnityEngine.PlayerPrefs.SetString(key, v);
                    break;
                default:
                    if (serializeCallback == default)
                    {
                        throw new ArgumentException($"Need third argument to set type of {typeof(T)}");
                    }

                    UnityEngine.PlayerPrefs.SetString(key, serializeCallback(value));
                    break;
            }

            UnityEngine.PlayerPrefs.Save();

            return UniTask.CompletedTask;
        }

        async UniTask<bool> IAsyncChecker.HasAsync(string key, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await UniTask.FromResult(UnityEngine.PlayerPrefs.HasKey(key));
        }
    }
}