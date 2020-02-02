using System;
using System.Collections;
using CAFU.KeyValueStore.Application.Installer;
using CAFU.KeyValueStore.Application.Master;
using CAFU.KeyValueStore.Application.Utility;
using CAFU.KeyValueStore.Domain.UseCase.Interface.Repository;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace CAFU.KeyValueStore
{
    public class 設定 : ZenjectIntegrationTestFixture
    {
        [Serializable]
        internal class CustomClass
        {
            public CustomClass(int intValue, string stringValue)
            {
                this.intValue = intValue;
                this.stringValue = stringValue;
            }

            [SerializeField] private int intValue;
            [SerializeField] private string stringValue;

            public int IntValue => intValue;
            public string StringValue => stringValue;
        }

        [Inject] private IKeyValueStore KeyValueStore { get; }

        private static string Key { get; } = "SetTest";

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
        public IEnumerator A_値を設定できる()
        {
            yield return KeyValueStore
                .Set(Key, "値を設定")
                .ToCoroutine();

            Assert.That(PlayerPrefs.GetString(Key), Is.EqualTo("値を設定"));
        }

        [UnityTest]
        public IEnumerator B_プリミティブ型の値を設定できる()
        {
            yield return KeyValueStore
                .Set(Key + "Bool", true)
                .ToCoroutine();
            yield return KeyValueStore
                .Set(Key + "Int", 2)
                .ToCoroutine();
            yield return KeyValueStore
                .Set(Key + "Float", 100.0f)
                .ToCoroutine();
            yield return KeyValueStore
                .Set(Key + "String", "string value")
                .ToCoroutine();

            Assert.That(PlayerPrefs.GetInt(Key + "Bool") == 1, Is.True);
            Assert.That(PlayerPrefs.GetInt(Key + "Int"), Is.EqualTo(2));
            Assert.That(PlayerPrefs.GetFloat(Key + "Float"), Is.EqualTo(100.0f));
            Assert.That(PlayerPrefs.GetString(Key + "String"), Is.EqualTo("string value"));
        }

        [UnityTest]
        public IEnumerator C_非プリミティブ型の値を設定できる()
        {
            var customClass = new CustomClass(10, "custom class");
            var dateTimeNow = DateTime.Now;

            yield return KeyValueStore
                .Set(Key + "Custom", customClass, Serializer.Default)
                .ToCoroutine(
                    x =>
                    {
                    }
                );
            yield return KeyValueStore
                .Set(Key + "DateTime", dateTimeNow, Serializer.DateTime)
                .ToCoroutine();

            {
                var x = Deserializer.Default<CustomClass>(PlayerPrefs.GetString(Key + "Custom"));
                Assert.That(x.IntValue, Is.EqualTo(customClass.IntValue));
                Assert.That(x.StringValue, Is.EqualTo(customClass.StringValue));
            }
            {
                var x = Deserializer.DateTime(PlayerPrefs.GetString(Key + "DateTime"));
                Assert.That(x.Year, Is.EqualTo(dateTimeNow.Year));
                Assert.That(x.Month, Is.EqualTo(dateTimeNow.Month));
                Assert.That(x.Day, Is.EqualTo(dateTimeNow.Day));
                Assert.That(x.Hour, Is.EqualTo(dateTimeNow.Hour));
                Assert.That(x.Minute, Is.EqualTo(dateTimeNow.Minute));
                Assert.That(x.Second, Is.EqualTo(dateTimeNow.Second));
            }
        }
    }
}