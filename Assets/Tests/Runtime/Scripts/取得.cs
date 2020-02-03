using System;
using System.Collections;
using CAFU.KeyValueStore.Application.Installer;
using CAFU.KeyValueStore.Application.Interface;
using CAFU.KeyValueStore.Application.Master;
using CAFU.KeyValueStore.Application.Utility;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace CAFU.KeyValueStore
{
    public class 取得 : ZenjectIntegrationTestFixture
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

        private static string Key { get; } = "GetTest";

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
        public IEnumerator A_値を取得できる()
        {
            PlayerPrefs.SetString(Key, "値を取得");

            yield return KeyValueStore
                .Get<string>(Key)
                .ToCoroutine(
                    x => Assert.That(x, Is.EqualTo("値を取得"))
                );
        }

        [UnityTest]
        public IEnumerator B_プリミティブ型の値を取得できる()
        {
            PlayerPrefs.SetInt(Key + "Bool", 1);
            PlayerPrefs.SetInt(Key + "Int", 2);
            PlayerPrefs.SetFloat(Key + "Float", 100.0f);
            PlayerPrefs.SetString(Key + "String", "string value");

            yield return KeyValueStore
                .Get<bool>(Key + "Bool")
                .ToCoroutine(x => Assert.That(x, Is.True));
            yield return KeyValueStore
                .Get<int>(Key + "Int")
                .ToCoroutine(x => Assert.That(x, Is.EqualTo(2)));
            yield return KeyValueStore
                .Get<float>(Key + "Float")
                .ToCoroutine(x => Assert.That(x, Is.EqualTo(100.0f)));
            yield return KeyValueStore
                .Get<string>(Key + "String")
                .ToCoroutine(x => Assert.That(x, Is.EqualTo("string value")));
        }

        [UnityTest]
        public IEnumerator C_非プリミティブ型の値を取得できる()
        {
            var customClass = new CustomClass(10, "custom class");
            var dateTimeNow = DateTime.Now;
            PlayerPrefs.SetString(Key + "Custom", Serializer.Default(customClass));
            PlayerPrefs.SetString(Key + "DateTime", Serializer.DateTime(dateTimeNow));

            yield return KeyValueStore
                .Get(Key + "Custom", deserializeCallback: Deserializer.Default<CustomClass>)
                .ToCoroutine(
                    x =>
                    {
                        Assert.That(x.IntValue, Is.EqualTo(customClass.IntValue));
                        Assert.That(x.StringValue, Is.EqualTo(customClass.StringValue));
                    }
                );
            yield return KeyValueStore
                .Get(Key + "DateTime", deserializeCallback: Deserializer.DateTime)
                .ToCoroutine(
                    x =>
                    {
                        Assert.That(x.Year, Is.EqualTo(dateTimeNow.Year));
                        Assert.That(x.Month, Is.EqualTo(dateTimeNow.Month));
                        Assert.That(x.Day, Is.EqualTo(dateTimeNow.Day));
                        Assert.That(x.Hour, Is.EqualTo(dateTimeNow.Hour));
                        Assert.That(x.Minute, Is.EqualTo(dateTimeNow.Minute));
                        Assert.That(x.Second, Is.EqualTo(dateTimeNow.Second));
                    }
                );
        }

        [UnityTest]
        public IEnumerator D_デフォルト値を取得できる()
        {
            yield return KeyValueStore
                .Get(Key, "デフォルト値を取得")
                .ToCoroutine(
                    x => Assert.That(x, Is.EqualTo("デフォルト値を取得"))
                );
        }
    }
}