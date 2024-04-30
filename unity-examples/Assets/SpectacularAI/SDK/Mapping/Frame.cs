using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.Mapping
{
    /// <summary>
    /// A set of camera frames from multiple cameras at a moment of time.
    /// </summary>
    public sealed class Frame : IDisposable
    {
        // Native handle to the Frame
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        CameraPose _cameraPose;

        /// <summary>
        /// Initializes a new instance of the Frame class.
        /// </summary>
        /// <param name="handle">The native handle to the Frame.</param>
        public Frame(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "Frame handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the Frame object.
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
                }

                ExternApi.sai_frame_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the Frame class.
        /// </summary>
        ~Frame()
        {
            Dispose(false);
        }

        /// <summary>
        /// Camera pose information
        /// </summary>
        public CameraPose CameraPose
        {
            get
            {
                if (_cameraPose == null)
                {
                    IntPtr cameraPoseHandle = ExternApi.sai_frame_get_camera_pose(_handle);
                    _cameraPose = new CameraPose(cameraPoseHandle);
                }

                return _cameraPose;
            }
        }

        /// <summary>
        /// Image data from the camera, might not always exist. 
        /// TODO: implement, now always null
        /// </summary>
        public IntPtr Image
        {
            get
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Set when image type is depth.Depth image values are multiplied by this
        /// number to to make their unit meters(e.g., if depth is integer mm,
        /// this will be set to 1.0 / 1000)
        /// </summary>
        public double DepthScale
        {
            get
            {
                return ExternApi.sai_frame_get_depth_scale(_handle);
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Frame));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_frame_get_camera_pose(IntPtr frameHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern double sai_frame_get_depth_scale(IntPtr frameHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_frame_release(IntPtr frameHandle);
        }
    }
}
