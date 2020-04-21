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

![image](https://user-images.githubusercontent.com/838945/78094369-61a8b800-740f-11ea-842c-431a6144c017.png)

1. Select `Overlay` to `Render Type` field of `Camera` component.
1. Add `AddOverlayCameraToCameraStack` component.

#### Note

* `AddOverlayCameraToCameraStack` component will find `Overlay Camera` automatically from same GameObject on runtime. You can also set `Overlay Camera` manually.
    * Set field automatically when component attached (and invoked Reset).
* Overwrite `Render Type` to `Overlay` on runtime if `Overwrite Render Type` field is set to `true`.

### Run
 
![image](https://user-images.githubusercontent.com/838945/78006249-b4369580-7377-11ea-9502-3a93efba442d.png)

Add Overlay Camera into Camera Stack of Base Camera automatically. 

The base camera to be added is selected with the highest Priority value.

## Notes

### Camera Stack may not be configured well in some processing order.

This is directly due to the process of adding a Camera managed by the AddOverlayCameraToCameraStack Component to the Camera Stack of Base Camera.

When looking for a Base Camera to add, you can use `Camera.depth` (the value that appears as **Priority** in the Inspector.) values in descending order, the first one is used.

Therefore, it is recommended to set a large value in `Camera.depth` to fix the camera to be added correctly.
