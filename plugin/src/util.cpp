#include "../include/spectacularAI/unity/util.hpp"

Matrix3dWrapper matrix_to_wrapper(const spectacularAI::Matrix3d &m) {
    Matrix3dWrapper w;
    w.m00 = m[0][0]; w.m01 = m[0][1]; w.m02 = m[0][2];
    w.m10 = m[1][0]; w.m11 = m[1][1]; w.m12 = m[1][2];
    w.m20 = m[2][0]; w.m21 = m[2][1]; w.m22 = m[2][2];
    return w;
}

Matrix4dWrapper matrix_to_wrapper(const spectacularAI::Matrix4d &m) {
    Matrix4dWrapper w;
    w.m00 = m[0][0]; w.m01 = m[0][1]; w.m02 = m[0][2]; w.m03 = m[0][3];
    w.m10 = m[1][0]; w.m11 = m[1][1]; w.m12 = m[1][2]; w.m13 = m[1][3];
    w.m20 = m[2][0]; w.m21 = m[2][1]; w.m22 = m[2][2]; w.m23 = m[2][3];
    w.m30 = m[3][0]; w.m31 = m[3][1]; w.m32 = m[3][2]; w.m33 = m[3][3];
    return w;
}

Matrix4dWrapper sai_pose_as_matrix(spectacularAI::Vector3d position, spectacularAI::Quaternion orientation) {
    spectacularAI::Pose pose;
    pose.position = position;
    pose.orientation = orientation;
    return matrix_to_wrapper(pose.asMatrix());
}

spectacularAI::Pose sai_pose_from_matrix(double t, Matrix4dWrapper localToWorld) {
    return spectacularAI::Pose::fromMatrix(
        t, 
        reinterpret_cast<spectacularAI::Matrix4d&>(localToWorld));
}