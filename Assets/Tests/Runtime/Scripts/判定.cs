using System.Collections;
using CAFU.KeyValueStore.Application.Installer;
using CAFU.KeyValueStore.Application.Master;
using CAFU.KeyValueStore.Domain.UseCase.Interface.Repository;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace CAFU.KeyValueStore
{
    public class 判定 : ZenjectIntegrationTestFixture
    {
        [Inject] private IKeyValueStore KeyValueStore { get; }

        private static string Key { get; } = "HasTest";

        [SetUp]
        public void Install()
        {
            PreInstall();
            KeyValueStoreInstaller.Install(Container, DataStoreType.PlayerPrefs);
            PostInstall();

            Container.Inject(this);
        }

        [TearDown]
        public void AfterTest()
        {
            PlayerPrefs.DeleteAll();
        }

        [UnityTest]
        public IEnumerator A_キーが存在する場合はTrue()
        {
            PlayerPrefs.SetString(Key, "some value");

            yield return KeyValueStore
                .Has(Key)
                .ToCoroutine(x => Assert.That(x, Is.True));
        }

        [UnityTest]
        public IEnumerator B_キーが存在しない場合はFalse()
        {
            PlayerPrefs.SetString(Key + "_", "some value");

            yield return KeyValueStore
                .Has(Key)
                .ToCoroutine(x => Assert.That(x, Is.False));
        }
    }
}