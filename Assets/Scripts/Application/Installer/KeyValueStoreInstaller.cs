using System;
using System.Collections.Generic;
using CAFU.KeyValueStore.Application.Master;
using CAFU.KeyValueStore.Data.DataStore.Implement;
using CAFU.KeyValueStore.Data.Repository.Implement;
using CAFU.KeyValueStore.Domain.UseCase.Interface.Repository;
using JetBrains.Annotations;
using Zenject;

namespace CAFU.KeyValueStore.Application.Installer
{
    [UsedImplicitly]
    public class KeyValueStoreInstaller : Installer<DataStoreType, KeyValueStoreInstaller>
    {
        public KeyValueStoreInstaller(DataStoreType dataStoreType)
        {
            DataStoreType = dataStoreType;
        }

        private DataStoreType DataStoreType { get; }

        private static IDictionary<DataStoreType, Action<DiContainer>> InstallerMethods { get; } = new Dictionary<DataStoreType, Action<DiContainer>>
        {
            { DataStoreType.PlayerPrefs, InstallWithPlayerPrefs },
        };

        public override void InstallBindings()
        {
            Container
                .Bind<IKeyValueStore>()
                .FromSubContainerResolve()
                .ByMethod(InstallerMethods[DataStoreType])
                .AsCached();
        }

        private static void InstallWithPlayerPrefs(DiContainer container)
        {
            container
                .BindInterfacesTo<KeyValueStoreHandler>()
                .AsCached();
            container
                .BindInterfacesTo<PlayerPrefs>()
                .AsCached();
        }
    }
}