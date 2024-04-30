using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.DepthAI
{
    /// <summary>
    /// VIO session. Should be created via Pipeline::StartSession.
    /// </summary>
    public sealed class Session : IDisposable
    {
        // Native handle to the Session
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the Session class.
        /// </summary>
        /// <param name="handle">The native handle to the Session.</param>
        public Session(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "Session handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the Session object.
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

                ExternApi.sai_depthai_session_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the Session class.
        /// </summary>
        ~Session()
        {
            Dispose(false);
        }

        /// <summary>
        /// Check if new output is available
        /// </summary>
        /// <returns>True if output available, otherwise false</returns>
        public bool HasOutput() {
            CheckDisposed();
            return ExternApi.sai_depthai_session_has_output(_handle);
        }

        /// <summary>
        /// Get output from the queue.
        /// </summary>
        /// <returns>If available returns vio output. If not, returns null</returns>
        public VioOutput GetOutput()
        {
            CheckDisposed();
            IntPtr vioOutputHandle = ExternApi.sai_depthai_session_get_output(_handle);
            if (vioOutputHandle == IntPtr.Zero) return null;
            return new VioOutput(vioOutputHandle);
        }

        /// <summary>
        /// Wait until new output is available and then return it.
        /// </summary>
        /// <returns>Vio output</returns>
        public VioOutput WaitForOutput()
        {
            CheckDisposed();
            IntPtr vioOutputHandle = ExternApi.sai_depthai_session_wait_for_output(_handle);
            return new VioOutput(vioOutputHandle);
        }

        /// <summary>
        /// Add an external trigger input. Causes additional output corresponding
        /// to a certain timestamp to be generated.
        /// </summary>
        /// <param name="t">timestamp, monotonic float seconds</param>
        /// <param name="tag">
        /// additonal tag to indentify this particular trigger event.
        /// The default outputs corresponding to input camera frames have a tag 0.
        /// </param>
        public void AddTrigger(double t, int tag)
        {
            CheckDisposed();
            ExternApi.sai_depthai_session_add_trigger(_handle, t, tag);
        }

        /// <summary>
        /// Add external pose information. VIO will correct its estimates to match the pose.
        /// </summary>
        /// <param name="pose">pose of the output coordinates in the external world coordinates</param>
        /// <param name="positionCovariance">position uncertainty as a covariance matrix in the external world coordinates</param>
        /// <param name="orientationVariance">optional orientation uncertainty as variance of angle</param>
        public void AddAbsolutePose(
            Pose pose,
            Matrix3d positionCovariance,
            double orientationVariance = -1)
        {
            CheckDisposed();
            ExternApi.sai_depthai_session_add_absolute_pose(
                _handle, 
                pose, 
                positionCovariance, 
                orientationVariance);
        }

        /// <summary>
        /// Compute RGB camera pose at vio output.
        /// </summary>
        /// <param name="vioOutput">Vio output at which to compute the pose</param>
        /// <returns>RGB camera's pose</returns>
        public CameraPose GetRgbCameraPose(VioOutput vioOutput)
        {
            CheckDisposed();
            IntPtr cameraPoseHandle = ExternApi.sai_depthai_session_get_rgb_camera_pose(
                _handle,
                vioOutput.GetNativeHandle()); 
            return new CameraPose(cameraPoseHandle);
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(VioOutput));
            }
        }

        private struct ExternApi
        {   
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern bool sai_depthai_session_has_output(IntPtr sessionHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_depthai_session_get_output(IntPtr sessionHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_depthai_session_wait_for_output(IntPtr sessionHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_depthai_session_add_trigger(
                IntPtr sessionHandle, 
                double t, 
                int tag);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_depthai_session_add_absolute_pose(
                IntPtr sessionHandle, 
                Pose pose, 
                Matrix3d positionCovariance, 
                double orientationVariance);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_depthai_session_get_rgb_camera_pose(
                IntPtr sessionHandle,
                IntPtr vioOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_depthai_session_release(IntPtr sessionHandle);
        }
    }
}
