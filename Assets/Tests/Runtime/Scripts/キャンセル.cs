using System.Collections;
using System.Threading;
using CAFU.KeyValueStore.Application.Interface;
using CAFU.KeyValueStore.Data.Repository.Implement;
using CAFU.KeyValueStore.Data.Repository.Interface.DataStore;
using NSubstitute;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine.TestTools;
using Zenject;

namespace CAFU.KeyValueStore
{
    public class キャンセル : ZenjectIntegrationTestFixture
    {
        [Inject] private IKeyValueStore KeyValueStore { get; }

        private static string Key { get; } = "CancelTest";

        private IAsyncGetter AsyncGetter { get; } = Substitute.For<IAsyncGetter>();
        private IAsyncSetter AsyncSetter { get; } = Substitute.For<IAsyncSetter>();
        private IAsyncChecker AsyncChecker { get; } = Substitute.For<IAsyncChecker>();

        [SetUp]
        public void Install()
        {
            PreInstall();
            Container.BindInterfacesTo<KeyValueStoreHandler>().AsCached();
            Container.BindInstance(AsyncGetter).AsCached();
            Container.BindInstance(AsyncSetter).AsCached();
            Container.BindInstance(AsyncChecker).AsCached();
            PostInstall();

            Container.Inject(this);
        }

        [UnityTest]
        public IEnumerator Getterをキャンセル()
        {
            var source = new CancellationTokenSource();
            AsyncGetter.When(x => x.GetAsync<int>(Key, cancellationToken: source.Token)).Do(_ => source.Cancel());

            yield return KeyValueStore.Get<int>(Key, cancellationToken: source.Token).ToCoroutine();

            Assert.That(source.IsCancellationRequested, Is.True);
        }

        [UnityTest]
        public IEnumerator Setterをキャンセル()
        {
            var source = new CancellationTokenSource();
            AsyncSetter.When(x => x.SetAsync(Key, 1, cancellationToken: source.Token)).Do(_ => source.Cancel());

            yield return KeyValueStore.Set(Key, 1, cancellationToken: source.Token).ToCoroutine();

            Assert.That(source.IsCancellationRequested, Is.True);
        }

        [UnityTest]
        public IEnumerator Checkerをキャンセル()
        {
            var source = new CancellationTokenSource();
            AsyncChecker.When(x => x.HasAsync(Key, source.Token)).Do(_ => source.Cancel());

            yield return KeyValueStore.Has(Key, source.Token).ToCoroutine();

            Assert.That(source.IsCancellationRequested, Is.True);
        }
    }
}