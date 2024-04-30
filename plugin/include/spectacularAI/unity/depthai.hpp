#pragma once

#include <spectacularAI/depthai/plugin.hpp>
#include "types.hpp"
#include "mapping.hpp"
#include "output.hpp"

struct ConfigurationWrapper {
    bool useStereo=true;
    bool useSlam=false;
    bool useFeatureTracker=true;
    bool fastVio=false;
    bool useColorStereoCameras=false;
    const char* mapSavePath="";
    const char* mapLoadPath="";
    const char* aprilTagPath="";
    uint32_t accFrequencyHz=500;
    uint32_t gyroFrequencyHz=400;
    int keyframeCandidateEveryNthFrame=6;
    const char* inputResolution="400p";
    const char* recordingFolder="";
    bool recordingOnly=false;
    bool fastImu=false;
    bool lowLatency=false;
};

struct PipelineWrapper {
    PipelineWrapper(
        std::shared_ptr<spectacularAI::daiPlugin::Pipeline> handle,
        std::shared_ptr<dai::Pipeline> pipeline,
        std::shared_ptr<dai::Device> device) : _handle(handle), _pipeline(pipeline), _device(device) {};
    const std::shared_ptr<spectacularAI::daiPlugin::Pipeline> getHandle() const { return _handle; }
    const std::shared_ptr<dai::Device> getDevice() const { return _device; }

private:
    const std::shared_ptr<spectacularAI::daiPlugin::Pipeline> _handle;
    const std::shared_ptr<dai::Pipeline> _pipeline;
    const std::shared_ptr<dai::Device> _device;
};

extern "C"
{
    /** Pipeline API */
    EXPORT_API PipelineWrapper* sai_depthai_pipeline_build(
        ConfigurationWrapper* configuration,
        const char** internalParameters,
        int internalParametersCount,
        callback_t_mapper_output onMapperOutput);
    EXPORT_API spectacularAI::daiPlugin::Session* sai_depthai_pipeline_start_session(PipelineWrapper* pipelineHandle);
    EXPORT_API void sai_depthai_pipeline_release(PipelineWrapper* pipelineHandle);

    /** Session API */
    EXPORT_API bool sai_depthai_session_has_output(const spectacularAI::daiPlugin::Session* sessionHandle);
    EXPORT_API VioOutputWrapper* sai_depthai_session_get_output(spectacularAI::daiPlugin::Session* sessionHandle);
    EXPORT_API VioOutputWrapper* sai_depthai_session_wait_for_output(spectacularAI::daiPlugin::Session* sessionHandle);
    EXPORT_API void sai_depthai_session_add_trigger(
        spectacularAI::daiPlugin::Session* sessionHandle,
        double t,
        int tag);
    EXPORT_API void sai_depthai_session_add_absolute_pose(
        spectacularAI::daiPlugin::Session* sessionHandle,
        spectacularAI::Pose pose,
        Matrix3dWrapper positionCovariance,
        double orientationVariance);
    EXPORT_API spectacularAI::CameraPose* sai_depthai_session_get_rgb_camera_pose(
        spectacularAI::daiPlugin::Session* sessionHandle,
        const VioOutputWrapper* vioOutputHandle);
    EXPORT_API void sai_depthai_session_release(spectacularAI::daiPlugin::Session* sessionHandle);
}
