using System.Threading;
using UniRx.Async;

namespace CAFU.KeyValueStore.Data.Interface
{
    internal interface IAsyncChecker
    {
        UniTask<bool> HasAsync(string key, CancellationToken cancellationToken = default);
    }
}