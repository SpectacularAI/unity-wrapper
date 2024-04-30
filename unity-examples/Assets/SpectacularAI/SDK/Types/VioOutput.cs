using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI
{
    /// <summary>
    /// Main output object.
    /// </summary>
    public sealed class VioOutput : IDisposable
    {
        // Native handle to the VioOutput
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the VioOutput class.
        /// </summary>
        /// <param name="handle">The native handle to the VioOutput.</param>
        public VioOutput(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "VioOutput handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the VioOutput object.
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

                ExternApi.sai_vio_output_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the VioOutput class.
        /// </summary>
        ~VioOutput()
        {
            Dispose(false);
        }

        /// <summary>
        /// Current tracking status
        /// </summary>
        public TrackingStatus Status
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_vio_output_get_tracking_status(_handle);
            }
        }

        /// <summary>
        /// The current pose in Unity world coordinates, with the timestamp in the clock used for input
        /// sensor data and camera frames.
        /// </summary>
        public Pose Pose
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_vio_output_get_pose(_handle);
            }
        }

        /// <summary>
        /// Velocity vector (xyz) in m/s in Unity world coordinates.
        /// </summary>
        public UnityEngine.Vector3 Velocity
        {
            get
            {
                CheckDisposed();
                return Utility.TransformWorldDirectionToUnity(ExternApi.sai_vio_output_get_velocity(_handle));
            }
        }

        /// <summary>
        /// Angular velocity vector in SI units (rad/s) in Unity world coordinates.
        /// </summary>
        public UnityEngine.Vector3 AngularVelocity
        {
            get
            {
                CheckDisposed();
                return Utility.TransformWorldAngularVelocityToUnity(ExternApi.sai_vio_output_get_angular_velocity(_handle));
            }
        }

        /// <summary>
        /// Linear acceleration in SI units (m/s^2).
        /// </summary>
        public UnityEngine.Vector3 Acceleration
        {
            get
            {
                CheckDisposed();
                return Utility.TransformWorldDirectionToUnity(ExternApi.sai_vio_output_get_acceleration(_handle));
            }
        }

        /// <summary>
        /// Uncertainty of the current position as a 3x3 covariance matrix.
        /// </summary>
        public Matrix3d PositionCovariance
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_vio_output_get_position_covariance(_handle);
            }
        }

        /// <summary>
        /// Uncertainty of velocity as a 3x3 covariance matrix.
        /// </summary>
        public Matrix3d VelocityCovariance
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_vio_output_get_velocity_covariance(_handle);
            }
        }

        /// <summary>
        /// The input frame tag. This is the value given in addFrame... methods.
        /// </summary>
        public int Tag
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_vio_output_get_tag(_handle);
            }
        }

        /// <summary>
        /// Current pose in camera coordinates
        /// </summary>
        /// <param name="cameraId">0 for primary, 1 for secondary camera </param>
        /// <returns></returns>
        public CameraPose GetCameraPose(int cameraId)
        {
            CheckDisposed();
            return new CameraPose(ExternApi.sai_vio_output_get_camera_pose(_handle, cameraId));
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(VioOutput));
            }
        }

        internal IntPtr GetNativeHandle()
        {
            return _handle;
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern TrackingStatus sai_vio_output_get_tracking_status(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Pose sai_vio_output_get_pose(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Vector3d sai_vio_output_get_velocity(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Vector3d sai_vio_output_get_angular_velocity(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Vector3d sai_vio_output_get_acceleration(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix3d sai_vio_output_get_position_covariance(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix3d sai_vio_output_get_velocity_covariance(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern int sai_vio_output_get_tag(IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_vio_output_get_camera_pose(IntPtr vioOutputHandle, int cameraId);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_vio_output_release(IntPtr vioOutputHandle);
        }
    }
}
