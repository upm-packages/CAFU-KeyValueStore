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
        var referenceTypeValue = "";
        var valueTypeValue = 0;

        if (await KeyValueStore.Has("key"))
        {
            referenceTypeValue = await KeyValueStore.GetReferenceType<string>("reference type key", "default value");
            valueTypeValue = await KeyValueStore.GetReferenceType<int>("value type key", -1);
        }

        await KeyValueStore.SetReferenceType<string>("some key", $"reference type value is {referenceTypeValue} and value type value is {valueTypeValue}");
        await KeyValueStore.SetValueType<bool>("some flag", true);
    }
}
```

It is strongly recommended that you use [UniTask](https://github.com/cysharp/UniTask) instead of `Task` if you can.
