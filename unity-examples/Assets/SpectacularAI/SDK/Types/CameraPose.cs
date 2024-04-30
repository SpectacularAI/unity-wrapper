using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI
{
    /// <summary>
    /// Pose of camera and conversions between world, camera, and pixel coordinates.
    /// </summary>
    public sealed class CameraPose : IDisposable
    {
        // Native handle to the CameraPose
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the CameraPose class.
        /// </summary>
        /// <param name="handle">The native handle to the CameraPose.</param>
        public CameraPose(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "CameraPose handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the CameraPose object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // No managed resources to release in this case
                }

                ExternApi.sai_camera_pose_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the CameraPose class.
        /// </summary>
        ~CameraPose()
        {
            Dispose(false);
        }

        /// <summary>
        /// Camera pose
        /// </summary>
        public Pose Pose
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_camera_pose_get_pose(_handle);
            }
        }

        /// <summary>
        /// Velocity vector (xyz) of the camera center in m/s
        /// </summary>
        public UnityEngine.Vector3 Velocity
        {
            get
            {
                CheckDisposed();
                return Utility.TransformWorldDirectionToUnity(ExternApi.sai_camera_pose_get_velocity(_handle));
            }
        }

        /// <summary>
        /// Camera intrinsic transformations
        /// </summary>
        public Camera Camera
        {
            get
            {
                CheckDisposed();
                return new Camera(ExternApi.sai_camera_pose_get_camera(_handle));
            }
        }

        /// <summary>
        /// Matrix that converts homogeneous world coordinates to homogeneous camera coordinates
        /// </summary>
        /// <returns>4x4 world to camera transformation matrix</returns>
        public UnityEngine.Matrix4x4 GetWorldToCameraMatrix()
        {
            CheckDisposed();
            return Utility.TransformWorldToCameraMatrixToUnity(ExternApi.sai_camera_pose_get_world_to_camera_matrix(_handle));
        }

        /// <summary>
        /// Matrix that converts homogeneous camera coordinates to homogeneous world coordinates
        /// </summary>
        /// <returns>4x4 camera to world transformation matrix</returns>
        public UnityEngine.Matrix4x4 GetCameraToWorldMatrix()
        {
            CheckDisposed();
            return Utility.TransformCameraToWorldMatrixToUnity(ExternApi.sai_camera_pose_get_camera_to_world_matrix(_handle));
        }

        /// <summary>
        /// Position in Unity world coordinates
        /// </summary>
        /// <returns>camera world position</returns>
        public UnityEngine.Vector3 GetPosition()
        {
            CheckDisposed();
            return Utility.TransformWorldPointToUnity(ExternApi.sai_camera_pose_get_position(_handle));
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Camera));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Pose sai_camera_pose_get_pose(IntPtr cameraPoseHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Vector3d sai_camera_pose_get_velocity(IntPtr cameraPoseHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_camera_pose_get_camera(IntPtr cameraPoseHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix4d sai_camera_pose_get_world_to_camera_matrix(IntPtr cameraPoseHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix4d sai_camera_pose_get_camera_to_world_matrix(IntPtr cameraPoseHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Vector3d sai_camera_pose_get_position(IntPtr cameraPoseHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_camera_pose_release(IntPtr cameraPoseHandle);
        }
    }
}
