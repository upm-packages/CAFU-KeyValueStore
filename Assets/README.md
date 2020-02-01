# CAFU KeyValueStore

## Installation

### Use Command Line

```bash
upm add package dev.upm-packages.cafu.keyvaluestore
```

Note: `upm` command is provided by [this repository](https://github.com/upm-packages/upm-cli).

### Edit `Packages/manifest.json`

```jsonc
{
  "dependencies": {
    // (snip)
    "dev.upm-packages.cafu.keyvaluestore": "[latest version]",
    // (snip)
  },
  "scopedRegistries": [
    {
      "name": "Unofficial Unity Package Manager Registry",
      "url": "https://upm-packages.dev",
      "scopes": [
        "com.stevevermeulen",
        "jp.cysharp",
        "dev.upm-packages"
      ]
    }
  ]
}
```

## Usages

### Zenject Installer

```csharp
KeyValueStoreInstaller.Install(Container, DataStoreType.PlayerPrefs);
```

Note: Currently the only DataStore type implemented is `PlayerPrefs`

### Handle values

```csharp
using CAFU.KeyValueStore.Application.Interface;
using System.Threading.Tasks;

public class Foo
{
    [Inject] private IKeyValueStore KeyValueStore { get; }

    public async Task Bar()
    {
        var value = "";

        if (await KeyValueStore.Has("key"))
        {
            value = await KeyValueStore.Get<string>("key", "default value");
        }

        await KeyValueStore.Set<string>("key", "value is " + value);
    }
}
```

It is strongly recommended that you use [UniTask](https://github.com/cysharp/UniTask) instead of `Task` if you can.
