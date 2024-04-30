#include "../include/spectacularAI/unity/output.hpp"
#include "../include/spectacularAI/unity/util.hpp"

#include <cassert>

spectacularAI::TrackingStatus sai_vio_output_get_tracking_status(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return vioOutputHandle->getHandle()->status;
}

spectacularAI::Pose sai_vio_output_get_pose(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return vioOutputHandle->getHandle()->pose;
}

spectacularAI::Vector3d sai_vio_output_get_velocity(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return vioOutputHandle->getHandle()->velocity;
}

spectacularAI::Vector3d sai_vio_output_get_angular_velocity(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return vioOutputHandle->getHandle()->angularVelocity;
}

spectacularAI::Vector3d sai_vio_output_get_acceleration(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return vioOutputHandle->getHandle()->acceleration;
}

Matrix3dWrapper sai_vio_output_get_position_covariance(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return reinterpret_cast<const Matrix3dWrapper&>(vioOutputHandle->getHandle()->positionCovariance);
}

Matrix3dWrapper sai_vio_output_get_velocity_covariance(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return reinterpret_cast<const Matrix3dWrapper&>(vioOutputHandle->getHandle()->velocityCovariance);
}

spectacularAI::CameraPose* sai_vio_output_get_camera_pose(const VioOutputWrapper* vioOutputHandle, int cameraId) {
    assert(vioOutputHandle);
    spectacularAI::CameraPose* cameraPose = new spectacularAI::CameraPose();
    *cameraPose = vioOutputHandle->getHandle()->getCameraPose(cameraId);
    return cameraPose;
}

int32_t sai_vio_output_get_tag(const VioOutputWrapper* vioOutputHandle) {
    assert(vioOutputHandle);
    return vioOutputHandle->getHandle()->tag;
}

void sai_vio_output_release(const VioOutputWrapper* vioOutputHandle) {
    if (vioOutputHandle) delete vioOutputHandle;
}

spectacularAI::Pose sai_camera_pose_get_pose(spectacularAI::CameraPose* cameraPoseHandle) {
    assert(cameraPoseHandle);
    return cameraPoseHandle->pose;
}

spectacularAI::Vector3d sai_camera_pose_get_velocity(spectacularAI::CameraPose* cameraPoseHandle) {
    assert(cameraPoseHandle);
    return cameraPoseHandle->velocity;
}

const CameraWrapper* sai_camera_pose_get_camera(spectacularAI::CameraPose* cameraPoseHandle) {
    assert(cameraPoseHandle);
    return new CameraWrapper(cameraPoseHandle->camera);
}

Matrix4dWrapper sai_camera_pose_get_world_to_camera_matrix(const spectacularAI::CameraPose* cameraPoseHandle) {
    assert(cameraPoseHandle);
    return matrix_to_wrapper(cameraPoseHandle->getWorldToCameraMatrix());
}

Matrix4dWrapper sai_camera_pose_get_camera_to_world_matrix(const spectacularAI::CameraPose* cameraPoseHandle) {
    assert(cameraPoseHandle);
    return matrix_to_wrapper(cameraPoseHandle->getCameraToWorldMatrix());
}

spectacularAI::Vector3d sai_camera_pose_get_position(const spectacularAI::CameraPose* cameraPoseHandle) {
    assert(cameraPoseHandle);
    return cameraPoseHandle->getPosition();
}

void sai_camera_pose_release(spectacularAI::CameraPose* cameraPoseHandle) {
    if (cameraPoseHandle) delete cameraPoseHandle;
}

bool sai_camera_pixel_to_ray(
        const CameraWrapper* cameraHandle,
        const spectacularAI::PixelCoordinates* pixel,
        spectacularAI::Vector3d* ray) {
    assert(cameraHandle);
    return cameraHandle->getHandle()->pixelToRay(*pixel, *ray);
}

bool sai_camera_ray_to_pixel(
        const CameraWrapper* cameraHandle,
        const spectacularAI::Vector3d* ray,
        spectacularAI::PixelCoordinates *pixel) {
    assert(cameraHandle);
    return cameraHandle->getHandle()->rayToPixel(*ray, *pixel);
}

Matrix3dWrapper sai_camera_get_intrinsic_matrix(const CameraWrapper* cameraHandle) {
    assert(cameraHandle);
    return matrix_to_wrapper(cameraHandle->getHandle()->getIntrinsicMatrix());
}

Matrix4dWrapper sai_camera_get_projection_matrix_opengl(
        const CameraWrapper* cameraHandle,
        double nearClip, 
        double farClip) {
    assert(cameraHandle);
    return matrix_to_wrapper(cameraHandle->getHandle()->getProjectionMatrixOpenGL(nearClip, farClip));
}

CameraWrapper* sai_camera_build_pinhole(
        Matrix3dWrapper intrinsicMatrix, 
        int width,
        int height) {
    const spectacularAI::Matrix3d &intrinsics = reinterpret_cast<const spectacularAI::Matrix3d&>(intrinsicMatrix);
    return new CameraWrapper(spectacularAI::Camera::buildPinhole(intrinsics, width, height));
}

void sai_camera_release(const CameraWrapper* cameraHandle) {
    if (cameraHandle) delete cameraHandle;
}