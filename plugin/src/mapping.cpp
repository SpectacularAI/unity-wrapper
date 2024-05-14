#include "../include/spectacularAI/unity/mapping.hpp"

#include <cassert>

MapWrapper* sai_mapper_output_get_map(const MapperOutputWrapper* mapperOutputHandle) {
    assert(mapperOutputHandle);
    return new MapWrapper(mapperOutputHandle->getHandle()->map);
}

int32_t sai_mapper_output_get_updated_key_frames(
        const MapperOutputWrapper* mapperOutputHandle,
        const int64_t** updatedKeyFramesHandle) {
    assert(mapperOutputHandle);
    const std::vector<int64_t> &updatedKeyFrames = mapperOutputHandle->getHandle()->updatedKeyFrames;
    *updatedKeyFramesHandle = updatedKeyFrames.data();
    return (int32_t)updatedKeyFrames.size();
}

bool sai_mapper_output_get_final_map(const MapperOutputWrapper* mapperOutputHandle) {
    assert(mapperOutputHandle);
    return mapperOutputHandle->getHandle()->finalMap;
}

void sai_mapper_output_release(const MapperOutputWrapper* mapperOutputHandle) {
    if (mapperOutputHandle) delete mapperOutputHandle;
}

int32_t sai_map_get_key_frame_count(const MapWrapper* mapHandle) {
    assert(mapHandle);
    return (int32_t)mapHandle->getHandle()->keyFrames.size();
}

void sai_map_get_key_frames(
        const MapWrapper* mapHandle,
        const KeyFrameWrapper** keyFramesHandles) {
    assert(mapHandle);

    int i = 0;
    for (const auto &it : mapHandle->getHandle()->keyFrames) {
        keyFramesHandles[i] = new KeyFrameWrapper(it.second);
        ++i;
    }
}

void sai_map_release(const MapWrapper* mapHandle) {
    if (mapHandle) delete mapHandle;
}

int64_t sai_key_frame_get_id(const KeyFrameWrapper* keyFrameHandle) {
    assert(keyFrameHandle);
    return keyFrameHandle->getHandle()->id;
}

FrameSetWrapper* sai_key_frame_get_frame_set(const KeyFrameWrapper* keyFrameHandle) {
    assert(keyFrameHandle);
    return new FrameSetWrapper(keyFrameHandle->getHandle()->frameSet);
}

PointCloudWrapper* sai_key_frame_get_point_cloud(const KeyFrameWrapper* keyFrameHandle) {
    assert(keyFrameHandle);
    if (keyFrameHandle->getHandle()->pointCloud) {
        return new PointCloudWrapper(keyFrameHandle->getHandle()->pointCloud);
    }
    return nullptr;
}

spectacularAI::Vector3d sai_key_frame_get_angular_velocity(const KeyFrameWrapper* keyFrameHandle) {
    assert(keyFrameHandle);
    return keyFrameHandle->getHandle()->angularVelocity;
}

void sai_key_frame_release(const KeyFrameWrapper* keyFrameHandle) {
    if (keyFrameHandle) delete keyFrameHandle;
}

FrameWrapper* sai_frame_set_get_primary_frame(FrameSetWrapper* frameSetHandle) {
    assert(frameSetHandle);
    if (frameSetHandle->getHandle()->primaryFrame) {
        return new FrameWrapper(frameSetHandle->getHandle()->primaryFrame);
    }
    return nullptr;
}

FrameWrapper* sai_frame_set_get_secondary_frame(FrameSetWrapper* frameSetHandle) {
    assert(frameSetHandle);
    if (frameSetHandle->getHandle()->secondaryFrame) {
        return new FrameWrapper(frameSetHandle->getHandle()->secondaryFrame);
    }
    return nullptr;
}

FrameWrapper* sai_frame_set_get_rgb_frame(FrameSetWrapper* frameSetHandle) {
    assert(frameSetHandle);
    if (frameSetHandle->getHandle()->rgbFrame) {
        return new FrameWrapper(frameSetHandle->getHandle()->rgbFrame);
    }
    return nullptr;
}

FrameWrapper* sai_frame_set_get_depth_frame(FrameSetWrapper* frameSetHandle) {
    assert(frameSetHandle);
    if (frameSetHandle->getHandle()->depthFrame) {
        return new FrameWrapper(frameSetHandle->getHandle()->depthFrame);
    }
    return nullptr;
}

void sai_frame_set_release(FrameSetWrapper* frameSetHandle) {
    if (frameSetHandle) delete frameSetHandle;
}

spectacularAI::CameraPose* sai_frame_get_camera_pose(FrameWrapper* frameHandle) {
    assert(frameHandle);
    spectacularAI::CameraPose* cameraPose = new spectacularAI::CameraPose();
    *cameraPose = frameHandle->getHandle()->cameraPose;
    return cameraPose;
}

double sai_frame_get_depth_scale(FrameWrapper* frameHandle) {
    assert(frameHandle);
    return frameHandle->getHandle()->depthScale;
}

void sai_frame_release(FrameWrapper* frameHandle) {
    if (frameHandle) delete frameHandle;
}

int sai_point_cloud_get_size(const PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return (int)pointCloudHandle->getHandle()->size();
}

bool sai_point_cloud_empty(const PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return pointCloudHandle->getHandle()->empty();
}

bool sai_point_cloud_has_normals(const PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return pointCloudHandle->getHandle()->hasNormals();
}

bool sai_point_cloud_has_colors(const PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return pointCloudHandle->getHandle()->hasColors();
}

const spectacularAI::Vector3f* sai_point_cloud_get_position_data(PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return pointCloudHandle->getHandle()->getPositionData();
}

const spectacularAI::Vector3f* sai_point_cloud_get_normal_data(PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return pointCloudHandle->getHandle()->getNormalData();
}

const std::uint8_t* sai_point_cloud_get_rgb24_data(PointCloudWrapper* pointCloudHandle) {
    assert(pointCloudHandle);
    return pointCloudHandle->getHandle()->getRGB24Data();
}

void sai_point_cloud_release(PointCloudWrapper* pointCloudHandle) {
    if (pointCloudHandle) delete pointCloudHandle;
}

