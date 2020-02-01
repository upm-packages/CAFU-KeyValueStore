using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Repository.Interface.DataStore
{
    public interface IAsyncChecker
    {
        UniTask<bool> Has(string key);
    }
}