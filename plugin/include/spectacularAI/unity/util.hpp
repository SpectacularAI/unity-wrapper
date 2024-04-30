#pragma once

#include "types.hpp"

Matrix3dWrapper matrix_to_wrapper(const spectacularAI::Matrix3d &m);
Matrix4dWrapper matrix_to_wrapper(const spectacularAI::Matrix4d &m);

extern "C" {
    EXPORT_API Matrix4dWrapper sai_pose_as_matrix(spectacularAI::Vector3d position, spectacularAI::Quaternion orientation);
    EXPORT_API spectacularAI::Pose sai_pose_from_matrix(double t, Matrix4dWrapper localToWorld);
}