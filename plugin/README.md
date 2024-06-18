# spectacularAI_unity plugin

## Dependencies

The `spectacularAI_depthaiPlugin` package is required. For non-commercial purposes you can find one here: https://github.com/SpectacularAI/sdk/releases

## Building (Windows)
```
mkdir target && cd target
cmake -Ddepthai_DIR=path\to\spectacularAI_depthaiPlugin_cpp_non-commercial_1.34.0\Windows\lib\cmake\depthai -DspectacularAI_depthaiPlugin_DIR=path\to\spectacularAI_depthaiPlugin_cpp_non-commercial_1.34.0\Windows\lib\cmake\spectacularAI ..
cmake --build . --config Release
```

Replace the existing `spectacularAI_unity.dll` [here](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Plugins/Windows).

## Building (Linux)
```
mkdir target && cd target
cmake -Ddepthai_DIR=path\to\spectacularAI_depthaiPlugin_cpp_non-commercial_1.34.0\Linux_Ubuntu_x86-64\lib\cmake\depthai -DspectacularAI_depthaiPlugin_DIR=path\to\spectacularAI_depthaiPlugin_cpp_non-commercial_1.34.0\Linux_Ubuntu_x86-64\lib\cmake\spectacularAI ..
make
```
Replace the existing `spectacularAI_unity.so` [here](https://github.com/SpectacularAI/unity-wrapper/tree/main/unity-examples/Assets/SpectacularAI/Plugins/Linux_Ubuntu_x86-64).

## Run C++ examples (for debugging/testing)
1. Live example with DepthAI devices. Connect DepthAI device and then run
```
.\Release\main_depthai.exe
```
The position of the device should be printed in your terminal.

2. If you have recorded datasets, then you can replay them using
```
.\Release\main_replay.exe path\to\recording
```
The position of the device should be printed in your terminal.
