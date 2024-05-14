using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.Mapping
{
    /// <summary>
    /// A set of camera frames from multiple cameras at a moment of time.
    /// </summary>
    public sealed class FrameSet : IDisposable
    {
        // Native handle to the FrameSet
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        Frame _primaryFrame;
        Frame _secondaryFrame;
        Frame _rgbFrame;
        Frame _depthFrame;

        /// <summary>
        /// Initializes a new instance of the FrameSet class.
        /// </summary>
        /// <param name="handle">The native handle to the FrameSet.</param>
        public FrameSet(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "FrameSet handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the FrameSet object.
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
                    _primaryFrame?.Dispose();
                    _secondaryFrame?.Dispose();
                    _rgbFrame?.Dispose();
                    _depthFrame?.Dispose();
                }

                ExternApi.sai_frame_set_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the FrameSet class.
        /// </summary>
        ~FrameSet()
        {
            Dispose(false);
        }

        /// <summary>
        /// Primary camera frame used for tracking
        /// </summary>
        public Frame PrimaryFrame
        {
            get
            {
                CheckDisposed();
                
                if (_primaryFrame == null)
                {
                    IntPtr frameHandle = ExternApi.sai_frame_set_get_primary_frame(_handle);
                    _primaryFrame = new Frame(frameHandle);
                }

                return _primaryFrame;
            }
        }

        /// <summary>
        /// Secondary camera frame used for tracking when using stereo cameras.
        /// </summary>
        public Frame SecondaryFrame
        {
            get
            {
                CheckDisposed();

                if (_secondaryFrame == null)
                {
                    IntPtr frameHandle = ExternApi.sai_frame_set_get_secondary_frame(_handle);
                    _secondaryFrame = new Frame(frameHandle);
                }

                return _secondaryFrame;
            }
        }

        /// <summary>
        /// RGB color frame if such exists. When using RGB-D this is the same as primaryFrame
        /// </summary>
        public Frame RGBFrame
        {
            get
            {
                CheckDisposed();

                if (_rgbFrame == null)
                {
                    IntPtr frameHandle = ExternApi.sai_frame_set_get_rgb_frame(_handle);
                    _rgbFrame = new Frame(frameHandle);
                }

                return _rgbFrame;
            }
        }

        /// <summary>
        /// Depth frame if such exists.
        /// </summary>
        public Frame DepthFrame
        {
            get
            {
                CheckDisposed();

                if (_depthFrame == null)
                {
                    IntPtr frameHandle = ExternApi.sai_frame_set_get_depth_frame(_handle);
                    _depthFrame = new Frame(frameHandle);
                }

                return _depthFrame;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(FrameSet));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_frame_set_get_primary_frame(IntPtr frameSetHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_frame_set_get_secondary_frame(IntPtr frameSetHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_frame_set_get_rgb_frame(IntPtr frameSetHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_frame_set_get_depth_frame(IntPtr frameSetHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_frame_set_release(IntPtr frameSetHandle);
        }
    }
}
