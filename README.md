# Spectacular AI Unity integration example

Spectacular AI SDK fuses data from cameras and IMU sensors (accelerometer and gyroscope) and outputs an accurate 6-degree-of-freedom pose of a device. The SDK also includes a Mapping API that can be used to access the full SLAM map for both real-time and offline 3D reconstruction use cases.

This repository contains example Unity integration for the Spectacular AI DepthAI C++ Plugin, enabling real-time tracking and reconstruction with OAK-D devices straight in Unity.

### Getting started
You can either,
1. Clone this repository and open [unity-examples](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples) using Unity Hub.
```
git clone https://github.com/SpectacularAI/unity-wrapper.git
```

2. Or, download `spectacularAI.unitypackage` from the [releases page](https://github.com/SpectacularAI/unity-wrapper/releases), and import it directly to your Unity project inside Unity Editor using `Assets->Import Package->Custom Package...`

### Project structure

 * **[plugin:](https://github.com/SpectacularAI/unity-wrapper/tree/main/plugin)** contains source code for the `spectacularAI_unity` plugin. A prebuilt version is shipped with the unity-examples for Windows and Linux Ubuntu x84-64 so building the plugin is not necessary to get started.
 * **[unity-examples:](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples)** contains source code for the Unity wrapper and a couple of examples that demonstrate the usage:
    * **[HelloDepthAI](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/HelloDepthAI)** Minimal OAK-D example. Shows how to use the SDK to track the pose of OAK-D device in real-time.
    * **[MappingVisu](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/MappingVisu)** SLAM example. Build and visualize 3D point cloud of the environment in real-time using OAK-D device.
    * **[Record](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/Record)** Recording tool. Enables recording datasets with OAK-D device for Replay API. Note: requires FFmpeg https://spectacularai.github.io/docs/sdk/index.html#ffmpeg.
    * **[AprilTag](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/AprilTag)** April Tag integration example. Place April Tags using Unity Editor. The April Tags are then input to the SDK enabling absolute positioning.
    * **[HelloReplay](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/HelloReplay)** Minimal Replay API example. Shows how to use the Replay API in Unity to replay your recordings.

### Running Unity applications on Linux

Most of the dependecies are included with the build, but there are a few libraries that must be present on the system for the plugin to run correctly.

System library dependencies:
```
libstdc++.so.6
libm.so.6
libgcc_s.so.1
libc.so.6
librt.so.1
libz.so.1
libdl.so.2
libpthread.so.0
/lib64/ld-linux-x86-64.so.2
libusb-1.0.so
```
Most of these libraries will be installed by default on a Linux system, but if you encounter errors in your application `player.log` like "Failed to open plugin: libspectacularAI_unity.so", make sure all of these libraries exist on your machine and install them if not (see [Issue 3](https://github.com/SpectacularAI/unity-wrapper/issues/3)).

We encountered a missing `libusb-1.0.so` library on a fresh install of Ubuntu Jammy 22.04 and fixed the problem by running:
```
sudo apt update
sudo apt install libusb-1.0-0-dev
```

### Quick links

#### [SDK documentation](https://spectacularai.github.io/docs/sdk/)
#### [C++ release packages](https://github.com/SpectacularAI/sdk/releases)

## Copyright

The examples in this repository are licensed under Apache 2.0 (see LICENSE).

A list of 3rd party copyright notices that should be included in
redistributions is provided within `unity-examples/Assets/SpectacularAI/Plugins/<operating system>/LICENSE`.

The SDK itself (not included in this repository) is proprietary to Spectacular AI.
For commerical licensing options and more SDK variants (ARM binaries & C++ API),
contact us at https://www.spectacularai.com/#contact.
