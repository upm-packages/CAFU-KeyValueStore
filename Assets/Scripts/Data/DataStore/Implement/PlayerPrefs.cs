using System;
using CAFU.KeyValueStore.Application.Utility;
using CAFU.KeyValueStore.Data.Repository.Interface.DataStore;
using JetBrains.Annotations;
using UniRx.Async;
using Zenject;

namespace CAFU.KeyValueStore.Data.DataStore.Implement
{
    [UsedImplicitly]
    internal class PlayerPrefs : IAsyncGetter, IAsyncSetter, IAsyncChecker
    {
        [Inject]
        internal PlayerPrefs()
        {
        }

        async UniTask<T> IAsyncGetter.GetAsync<T>(string key, T defaultValue, Func<string, T> deserializeCallback)
        {
            if (!UnityEngine.PlayerPrefs.HasKey(key))
            {
                return await UniTask.FromResult(defaultValue);
            }

            T result;
            if (typeof(T) == typeof(ReferenceWrapper<bool>))
            {
                result = (T) (object) (UnityEngine.PlayerPrefs.GetInt(key, (defaultValue as ReferenceWrapper<bool>).Unwrap() ? 1 : 0) == 1).Wrap();
            }
            else if (typeof(T) == typeof(ReferenceWrapper<int>))
            {
                result = (T) (object) UnityEngine.PlayerPrefs.GetInt(key, (defaultValue as ReferenceWrapper<int>).Unwrap()).Wrap();
            }
            else if (typeof(T) == typeof(ReferenceWrapper<float>))
            {
                result = (T) (object) UnityEngine.PlayerPrefs.GetFloat(key, (defaultValue as ReferenceWrapper<float>).Unwrap()).Wrap();
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T) (object) UnityEngine.PlayerPrefs.GetString(key, (string) (object) defaultValue);
            }
            else if (deserializeCallback != default)
            {
                result = deserializeCallback(UnityEngine.PlayerPrefs.GetString(key));
            }
            else
            {
                // Add `else if` when add Deserializer methods
                if (typeof(T) == typeof(ReferenceWrapper<DateTime>))
                {
                    result = Deserializer.DateTime(UnityEngine.PlayerPrefs.GetString(key)) as T;
                }
                else
                {
                    throw new ArgumentException($"Need third argument to get type of {typeof(T)}");
                }
            }

            return await UniTask.FromResult(result);
        }

        UniTask IAsyncSetter.SetAsync<T>(string key, T value, Func<T, string> serializeCallback)
        {
            switch (value)
            {
                case ReferenceWrapper<bool> v:
                    UnityEngine.PlayerPrefs.SetInt(key, v ? 1 : 0);
                    break;
                case ReferenceWrapper<int> v:
                    UnityEngine.PlayerPrefs.SetInt(key, v);
                    break;
                case ReferenceWrapper<float> v:
                    UnityEngine.PlayerPrefs.SetFloat(key, v);
                    break;
                case string v:
                    UnityEngine.PlayerPrefs.SetString(key, v);
                    break;
                default:
                    if (serializeCallback != default)
                    {
                        UnityEngine.PlayerPrefs.SetString(key, serializeCallback(value));
                        break;
                    }

                    switch (value)
                    {
                        // Add `case` when add Serializer methods
                        case ReferenceWrapper<DateTime> v:
                            UnityEngine.PlayerPrefs.SetString(key, Serializer.DateTime(v));
                            break;
                        default:
                            throw new ArgumentException($"Need third argument to set type of {typeof(T)}");
                    }

                    break;
            }

            UnityEngine.PlayerPrefs.Save();

            return UniTask.CompletedTask;
        }

        async UniTask<bool> IAsyncChecker.Has(string key)
        {
            return await UniTask.FromResult(UnityEngine.PlayerPrefs.HasKey(key));
        }
    }
}