#include "../include/spectacularAI/unity/depthai.hpp"

#include <string>
#include <depthai/depthai.hpp>
#include <cassert>

namespace {

void create_configuration(
        const ConfigurationWrapper &w,
        const char** internalParameters,
        int internalParametersCount,
        spectacularAI::daiPlugin::Configuration &config) {
    config.useStereo = w.useStereo;
    config.useSlam = w.useSlam;
    config.useFeatureTracker = w.useFeatureTracker;
    config.fastVio = w.fastVio;
    config.useColorStereoCameras = w.useColorStereoCameras;
    config.mapSavePath = w.mapSavePath;
    config.mapLoadPath = w.mapLoadPath;
    config.aprilTagPath = w.aprilTagPath;
    config.accFrequencyHz = w.accFrequencyHz;
    config.gyroFrequencyHz = w.gyroFrequencyHz;
    config.keyframeCandidateEveryNthFrame = w.keyframeCandidateEveryNthFrame;
    config.inputResolution = w.inputResolution;
    config.recordingFolder = w.recordingFolder;
    config.recordingOnly = w.recordingOnly;
    config.fastImu = w.fastImu;
    config.lowLatency = w.lowLatency;

    for (int i = 0; i < internalParametersCount; ++i) {
        std::string k = std::string(internalParameters[2 * i]);
        std::string v = std::string(internalParameters[2 * i + 1]);
        config.internalParameters.insert(std::make_pair(k, v));
    }
}

} // anonymous namespace

PipelineWrapper* sai_depthai_pipeline_build(
        ConfigurationWrapper* configuration,
        const char** internalParameters,
        int internalParametersCount,
        callback_t_mapper_output onMapperOutput) {
    std::shared_ptr<dai::Pipeline> pipeline = std::make_shared<dai::Pipeline>();

    spectacularAI::daiPlugin::Configuration config;
    create_configuration(*configuration, internalParameters, internalParametersCount, config);

    std::shared_ptr<spectacularAI::daiPlugin::Pipeline> handle = onMapperOutput ?
        std::make_shared<spectacularAI::daiPlugin::Pipeline>(*pipeline, config,
            [onMapperOutput](spectacularAI::mapping::MapperOutputPtr mapperOutput) {
                onMapperOutput(new MapperOutputWrapper(mapperOutput));
            })
        : std::make_shared<spectacularAI::daiPlugin::Pipeline>(*pipeline, config);
    std::shared_ptr<dai::Device> device = std::make_shared<dai::Device>(*pipeline);
    return new PipelineWrapper(handle, pipeline, device);
}

spectacularAI::daiPlugin::Session* sai_depthai_pipeline_start_session(PipelineWrapper* pipelineHandle, char* errorMsg) {
    assert(pipelineHandle);
    try {
        return pipelineHandle->getHandle()->startSession(*pipelineHandle->getDevice()).release();
    } catch(const std::runtime_error &e) {
        if (errorMsg != nullptr) {
            strncpy(errorMsg, e.what(), 1000 - 1);
            errorMsg[1000 - 1] = '\0'; // Ensure null-termination
        } else {
            throw e;
        }
    }

    return nullptr;
}

void sai_depthai_pipeline_release(PipelineWrapper* pipelineHandle) {
    if (pipelineHandle) delete pipelineHandle;
}

bool sai_depthai_session_has_output(const spectacularAI::daiPlugin::Session* sessionHandle) {
    assert(sessionHandle);
    return sessionHandle->hasOutput();
}

VioOutputWrapper* sai_depthai_session_get_output(spectacularAI::daiPlugin::Session* sessionHandle) {
    assert(sessionHandle);
    return new VioOutputWrapper(sessionHandle->getOutput());
}

VioOutputWrapper* sai_depthai_session_wait_for_output(spectacularAI::daiPlugin::Session* sessionHandle) {
    assert(sessionHandle);
    return new VioOutputWrapper(sessionHandle->waitForOutput());
}

void sai_depthai_session_add_trigger(
        spectacularAI::daiPlugin::Session* sessionHandle,
        double t,
        int tag) {
    assert(sessionHandle);
    sessionHandle->addTrigger(t, tag);
}

void sai_depthai_session_add_absolute_pose(
        spectacularAI::daiPlugin::Session* sessionHandle,
        spectacularAI::Pose pose,
        Matrix3dWrapper positionCovariance,
        double orientationVariance) {
    assert(sessionHandle);
    sessionHandle->addAbsolutePose(
        pose,
        reinterpret_cast<spectacularAI::Matrix3d&>(positionCovariance),
        orientationVariance);
}

spectacularAI::CameraPose* sai_depthai_session_get_rgb_camera_pose(
        spectacularAI::daiPlugin::Session* sessionHandle,
        const VioOutputWrapper* vioOutputHandle) {
    assert(sessionHandle);
    spectacularAI::CameraPose* cameraPose = new spectacularAI::CameraPose();
    *cameraPose = sessionHandle->getRgbCameraPose(*vioOutputHandle->getHandle());
    return cameraPose;
}

void sai_depthai_session_release(spectacularAI::daiPlugin::Session* sessionHandle) {
    if (sessionHandle) delete sessionHandle;
}
