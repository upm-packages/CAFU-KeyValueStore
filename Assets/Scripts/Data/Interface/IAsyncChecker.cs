using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Repository.Interface.DataStore
{
    internal interface IAsyncChecker
    {
        UniTask<bool> Has(string key);
    }
}