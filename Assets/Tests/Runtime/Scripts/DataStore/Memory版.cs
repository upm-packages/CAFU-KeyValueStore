using System.Collections;
using CAFU.KeyValueStore.Application.Installer;
using CAFU.KeyValueStore.Application.Interface;
using CAFU.KeyValueStore.Application.Master;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace CAFU.KeyValueStore.DataStore
{
    public class Memory版 : ZenjectIntegrationTestFixture
    {
        [Inject] private IKeyValueStore KeyValueStore { get; }

        private static string Key { get; } = "MemoryTest";

        [SetUp]
        public void Install()
        {
            PreInstall();
            KeyValueStoreInstaller.Install(Container, DataStoreType.Memory);
            PostInstall();

            Container.Inject(this);
        }

        [UnityTest]
        public IEnumerator 基本メソッド()
        {
            yield return KeyValueStore
                .Has(Key)
                .ToCoroutine(x => Assert.That(x, Is.False));
            yield return KeyValueStore
                .Get(Key, "DefaultValue")
                .ToCoroutine(x => Assert.That(x, Is.EqualTo("DefaultValue")));
            yield return KeyValueStore
                .Set(Key, "NewValue")
                .ToCoroutine();
            yield return KeyValueStore
                .Has(Key)
                .ToCoroutine(x => Assert.That(x, Is.True));
            yield return KeyValueStore
                .Get<string>(Key)
                .ToCoroutine(x => Assert.That(x, Is.EqualTo("NewValue")));
        }
    }
}