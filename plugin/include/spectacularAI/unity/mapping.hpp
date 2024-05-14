#pragma once

#include <spectacularAI/mapping.hpp>
#include "types.hpp"

using MapperOutputWrapper = Wrapper<const spectacularAI::mapping::MapperOutput>;
using MapWrapper = Wrapper<const spectacularAI::mapping::Map>;
using KeyFrameWrapper = Wrapper<const spectacularAI::mapping::KeyFrame>;
using FrameSetWrapper = Wrapper<spectacularAI::mapping::FrameSet>;
using FrameWrapper = Wrapper<spectacularAI::mapping::Frame>;
using PointCloudWrapper = Wrapper<spectacularAI::mapping::PointCloud>;

typedef void (*callback_t_mapper_output)(const MapperOutputWrapper*);

extern "C" {
    /** MapperOutput API */
    EXPORT_API MapWrapper* sai_mapper_output_get_map(const MapperOutputWrapper* mapperOutputHandle);
    EXPORT_API int32_t sai_mapper_output_get_updated_key_frames(
        const MapperOutputWrapper* mapperOutputHandle,
        const int64_t** updatedKeyFramesHandle);
    EXPORT_API bool sai_mapper_output_get_final_map(const MapperOutputWrapper* mapperOutputHandle);
    EXPORT_API void sai_mapper_output_release(const MapperOutputWrapper* mapperOutputHandle);

    /** Map API */
    EXPORT_API int32_t sai_map_get_key_frame_count(const MapWrapper* mapHandle);
    EXPORT_API void sai_map_get_key_frames(
        const MapWrapper* mapHandle,
        const KeyFrameWrapper** keyFramesHandles);
    EXPORT_API void sai_map_release(const MapWrapper* mapHandle);

    /** KeyFrame API */
    EXPORT_API int64_t sai_key_frame_get_id(const KeyFrameWrapper* keyFrameHandle);
    EXPORT_API FrameSetWrapper* sai_key_frame_get_frame_set(const KeyFrameWrapper* keyFrameHandle);
    EXPORT_API PointCloudWrapper* sai_key_frame_get_point_cloud(const KeyFrameWrapper* keyFrameHandle);
    EXPORT_API spectacularAI::Vector3d sai_key_frame_get_angular_velocity(const KeyFrameWrapper* keyFrameHandle);
    EXPORT_API void sai_key_frame_release(const KeyFrameWrapper* keyFrameHandle);

    /** FrameSet API */
    EXPORT_API FrameWrapper* sai_frame_set_get_primary_frame(FrameSetWrapper* frameSetHandle);
    EXPORT_API FrameWrapper* sai_frame_set_get_secondary_frame(FrameSetWrapper* frameSetHandle);
    EXPORT_API FrameWrapper* sai_frame_set_get_rgb_frame(FrameSetWrapper* frameSetHandle);
    EXPORT_API FrameWrapper* sai_frame_set_get_depth_frame(FrameSetWrapper* frameSetHandle);
    EXPORT_API void sai_frame_set_release(FrameSetWrapper* frameSetHandle);

    /** Frame API */
    EXPORT_API spectacularAI::CameraPose* sai_frame_get_camera_pose(FrameWrapper* frameHandle);
    EXPORT_API double sai_frame_get_depth_scale(FrameWrapper* frameHandle);
    EXPORT_API void sai_frame_release(FrameWrapper* frameHandle);

    /** PointCloud API */
    EXPORT_API int sai_point_cloud_get_size(const PointCloudWrapper* pointCloudHandle);
    EXPORT_API bool sai_point_cloud_empty(const PointCloudWrapper* pointCloudHandle);
    EXPORT_API bool sai_point_cloud_has_normals(const PointCloudWrapper* pointCloudHandle);
    EXPORT_API bool sai_point_cloud_has_colors(const PointCloudWrapper* pointCloudHandle);
    EXPORT_API const spectacularAI::Vector3f* sai_point_cloud_get_position_data(PointCloudWrapper* pointCloudHandle);
    EXPORT_API const spectacularAI::Vector3f* sai_point_cloud_get_normal_data(PointCloudWrapper* pointCloudHandle);
    EXPORT_API const std::uint8_t* sai_point_cloud_get_rgb24_data(PointCloudWrapper* pointCloudHandle);
    EXPORT_API void sai_point_cloud_release(PointCloudWrapper* pointCloudHandle);
}