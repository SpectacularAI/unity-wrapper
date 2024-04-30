#pragma once

#include <memory>
#include <spectacularAI/output.hpp>
#include <spectacularAI/types.hpp>
#include "types.hpp"

// Specializations for VioOutput and Camera
using VioOutputWrapper = Wrapper<const spectacularAI::VioOutput>;
using CameraWrapper = Wrapper<const spectacularAI::Camera>;

typedef void (*callback_t_vio_output)(const VioOutputWrapper*);

extern "C" {
    /** VioOutput API */
    EXPORT_API spectacularAI::TrackingStatus sai_vio_output_get_tracking_status(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API spectacularAI::Pose sai_vio_output_get_pose(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API spectacularAI::Vector3d sai_vio_output_get_velocity(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API spectacularAI::Vector3d sai_vio_output_get_angular_velocity(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API spectacularAI::Vector3d sai_vio_output_get_acceleration(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API Matrix3dWrapper sai_vio_output_get_position_covariance(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API Matrix3dWrapper sai_vio_output_get_velocity_covariance(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API spectacularAI::CameraPose* sai_vio_output_get_camera_pose(const VioOutputWrapper* vioOutputHandle, int cameraId);
    EXPORT_API int32_t sai_vio_output_get_tag(const VioOutputWrapper* vioOutputHandle);
    EXPORT_API void sai_vio_output_release(const VioOutputWrapper* vioOutputHandle);

    /** CameraPose API */
    EXPORT_API spectacularAI::Pose sai_camera_pose_get_pose(spectacularAI::CameraPose* cameraPoseHandle);
    EXPORT_API spectacularAI::Vector3d sai_camera_pose_get_velocity(spectacularAI::CameraPose* cameraPoseHandle);
    EXPORT_API const CameraWrapper* sai_camera_pose_get_camera(spectacularAI::CameraPose* cameraPoseHandle);
    EXPORT_API Matrix4dWrapper sai_camera_pose_get_world_to_camera_matrix(const spectacularAI::CameraPose* cameraPoseHandle);
    EXPORT_API Matrix4dWrapper sai_camera_pose_get_camera_to_world_matrix(const spectacularAI::CameraPose* cameraPoseHandle);
    EXPORT_API spectacularAI::Vector3d sai_camera_pose_get_position(const spectacularAI::CameraPose* cameraPoseHandle);
    EXPORT_API void sai_camera_pose_release(spectacularAI::CameraPose* cameraPoseHandle);

    /** Camera API */
    EXPORT_API bool sai_camera_pixel_to_ray(
        const CameraWrapper* cameraHandle,
        const spectacularAI::PixelCoordinates* pixel,
        spectacularAI::Vector3d* ray);
    EXPORT_API bool sai_camera_ray_to_pixel(
        const CameraWrapper* cameraHandle,
        const spectacularAI::Vector3d* ray,
        spectacularAI::PixelCoordinates *pixel);
    EXPORT_API Matrix3dWrapper sai_camera_get_intrinsic_matrix(
        const CameraWrapper* cameraHandle);
    EXPORT_API Matrix4dWrapper sai_camera_get_projection_matrix_opengl(
        const CameraWrapper* cameraHandle,
        double nearClip, 
        double farClip);
    EXPORT_API CameraWrapper* sai_camera_build_pinhole(
        Matrix3dWrapper intrinsicMatrix, 
        int width,
        int height);
    EXPORT_API void sai_camera_release(const CameraWrapper* cameraHandle);
}