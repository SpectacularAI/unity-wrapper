# Spectacular AI Unity integration example

Spectacular AI SDK fuses data from cameras and IMU sensors (accelerometer and gyroscope) and outputs an accurate 6-degree-of-freedom pose of a device. The SDK also includes a Mapping API that can be used to access the full SLAM map for both real-time and offline 3D reconstruction use cases.

This repository contains example Unity integration for the Spectacular AI DepthAI C++ Plugin, enabling real-time tracking and reconstruction with OAK-D devices straight in Unity.

### Getting started

**Installation** You can either,
1. Clone this repository and open [unity-examples](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples) using Unity Hub.
```
git clone https://github.com/SpectacularAI/unity-wrapper.git
```

2. Or, download `spectacularAI.unitypackage` from the [releases page](https://github.com/SpectacularAI/unity-wrapper/releases), and import it directly to your Unity project inside Unity Editor using `Assets->Import Package->Custom Package...`

**OAK-D setup** To use an OAK-D device, you also need to
[set up the USB udev rules](https://docs.luxonis.com/hardware/platform/deploy/usb-deployment-guide/#USB%20deployment%20guide-Initial%20Connection-Debugging-Linux%20udev%20rules)
and, on Linux, install `libusb` development libraries:

    sudo apt install libusb-1.0-0-dev

See [Luxonis documentation](https://docs.luxonis.com/hardware/platform/deploy/usb-deployment-guide) for more information.

### Project structure

 * **[plugin:](https://github.com/SpectacularAI/unity-wrapper/tree/main/plugin)** contains source code for the `spectacularAI_unity` plugin. A prebuilt version is shipped with the unity-examples for Windows and Linux Ubuntu x84-64 so building the plugin is not necessary to get started.
 * **[unity-examples:](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples)** contains source code for the Unity wrapper and a couple of examples that demonstrate the usage:
    * **[HelloDepthAI](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/HelloDepthAI)** Minimal OAK-D example. Shows how to use the SDK to track the pose of OAK-D device in real-time.
    * **[MappingVisu](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/MappingVisu)** SLAM example. Build and visualize 3D point cloud of the environment in real-time using OAK-D device.
    * **[Record](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/Record)** Recording tool. Enables recording datasets with OAK-D device for Replay API. Note: requires FFmpeg https://spectacularai.github.io/docs/sdk/index.html#ffmpeg.
    * **[AprilTag](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/AprilTag)** April Tag integration example. Place April Tags using Unity Editor. The April Tags are then input to the SDK enabling absolute positioning.
    * **[HelloReplay](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Examples/HelloReplay)** Minimal Replay API example. Shows how to use the Replay API in Unity to replay your recordings.

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
