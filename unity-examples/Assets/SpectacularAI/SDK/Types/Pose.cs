using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI
{
    /// <summary>
    /// Represents the pose (position & orientation) of a device at a given time */
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Pose
    {
        /// <summary>
        /// Timestamp in seconds. Monotonically increasing
        /// </summary>
        public double Time;

        /// <summary>
        /// Position in Unity world coordinates
        /// </summary>
        private Vector3d _position;
        public UnityEngine.Vector3 Position
        {
            get
            {
                return Utility.TransformWorldPointToUnity(_position);
            }
        }

        /// <summary>
        /// Orientation quaternion in Unity world coordinates
        /// </summary>
        private Quaternion _orientation;
        public UnityEngine.Quaternion Orientation
        {
            get
            {
                return Utility.TransformCameraToWorldQuaternionToUnity(_orientation);
            }
        }

        /// <summary>
        /// Matrix that converts homogeneous local coordinates to homogeneous world coordinates
        /// </summary>
        /// <returns></returns>
        public UnityEngine.Matrix4x4 AsMatrix()
        {
            return Utility.TransformCameraToWorldMatrixToUnity(ExternApi.sai_pose_as_matrix(_position, _orientation));
        }

        /// <summary>
        /// Create a pose from a timestamp and local-to-world matrix
        /// </summary>
        /// <returns>Pose object</returns>
        public static Pose FromMatrix(double t, UnityEngine.Matrix4x4 localToWorld)
        {
            return ExternApi.sai_pose_from_matrix(t, Utility.TransformCameraToWorldMatrixToSpectacularAI(localToWorld));
        }

        public override string ToString()
        {
            return $"SpectacularAI.Pose (time={Time}, position={Position}, orientation={Orientation})";
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix4d sai_pose_as_matrix(Vector3d position, Quaternion orientation);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Pose sai_pose_from_matrix(double t, Matrix4d localToWorld);
        }
    }
}