using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.KeyValueStore.Data.Interface
{
    internal interface IAsyncChecker
    {
        UniTask<bool> HasAsync(string key, CancellationToken cancellationToken = default);
    }
}