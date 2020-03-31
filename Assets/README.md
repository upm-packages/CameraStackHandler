![](https://github.com/upm-packages/CameraStackHandler/workflows/Publish%20UPM%20Package/badge.svg)

# CameraStackHandler

## Installation

```bash
upm add package dev.upm-packages.camerastackhandler
```

Note: `upm` command is provided by [this repository](https://github.com/upm-packages/upm-cli).

You can also edit `Packages/manifest.json` directly.

```jsonc
{
  "dependencies": {
    // (snip)
    "dev.upm-packages.camerastackhandler": "[latest version]", 
    // (snip)
  },
  "scopedRegistries": [
    {
      "name": "Unofficial Unity Package Manager Registry",
      "url": "https://upm-packages.dev",
      "scopes": [
        "dev.upm-packages"
      ]
    }
  ]
}
```

## Usages

### Upgrade the version of URP to v7.2.0 or higher

![image](https://user-images.githubusercontent.com/838945/77998989-eabae300-736c-11ea-9693-bae5263ee82e.png)

As of Unity 2019.3.7f1, the Verified version is v7.1.8, so you'll need to expand the ▶ to the left of the Universal RP to find the latest version.

### Add component into Overlay Camera

![image](https://user-images.githubusercontent.com/838945/78002961-ec87a500-7372-11ea-999f-d5c62ef81a71.png)

1. Select `Overlay` to `Render Type` field of `Camera` component.
1. Add `AddOverlayCameraToCameraStack` component.

Note: `AddOverlayCameraToCameraStack` component will find `Overlay Camera` automatically from same GameObject on runtime. You can also set `Overlay Camera` manually 
 
### Run
 
![image](https://user-images.githubusercontent.com/838945/78006249-b4369580-7377-11ea-9502-3a93efba442d.png)

Add Overlay Camera into Camera Stack of Base Camera automatically. 

The base camera to be added is selected with the highest Priority value.
