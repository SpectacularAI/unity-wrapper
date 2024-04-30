using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.Mapping
{
    /// <summary>
    /// SLAM map keyframe
    /// </summary>
    public sealed class KeyFrame : IDisposable
    {
        // Native handle to the KeyFrame
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        private FrameSet _frameSet;
        private PointCloud _pointCloud;

        /// <summary>
        /// Initializes a new instance of the KeyFrame class.
        /// </summary>
        /// <param name="handle">The native handle to the Map.</param>
        public KeyFrame(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "KeyFrame handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the KeyFrame object.
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
                    // Dispose images and pointcloud
                    if (_frameSet != null)
                    {
                        _frameSet.Dispose();
                    }

                    if (_pointCloud != null)
                    {
                        _pointCloud.Dispose();
                    }
                }

                ExternApi.sai_key_frame_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the KeyFrame class.
        /// </summary>
        ~KeyFrame()
        {
            Dispose(false);
        }

        /// <summary>
        /// Unique ID for this keyframe. Monotonically increasing.
        /// </summary>
        public long Id
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_key_frame_get_id(_handle);
            }
        }

        /// <summary>
        /// Cameras for this key frame
        /// </summary>
        public FrameSet FrameSet
        {
            get
            {
                CheckDisposed();

                if (_frameSet == null)
                {
                    IntPtr frameSetHandle = ExternApi.sai_key_frame_get_frame_set(_handle);
                    _frameSet = new FrameSet(frameSetHandle);
                }

                return _frameSet;
            }
        }

        /// <summary>
        /// Optional. Point cloud for this key frame in primary frame coordinates.
        /// </summary>
        public PointCloud PointCloud
        {
            get
            {
                CheckDisposed();

                if (_pointCloud == null)
                {
                    IntPtr pointCloudHandle = ExternApi.sai_key_frame_get_point_cloud(_handle);
                    if (pointCloudHandle != IntPtr.Zero)
                    {
                        _pointCloud = new PointCloud(pointCloudHandle);
                    }
                }

                return _pointCloud;
            }
        }

        /// <summary>
        /// Angular velocity vector in SI units (rad/s)
        /// </summary>
        public UnityEngine.Vector3 AngularVelocity
        {
            get
            {
                CheckDisposed();
                return Utility.TransformWorldAngularVelocityToUnity(ExternApi.sai_key_frame_get_angular_velocity(_handle));
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(KeyFrame));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern long sai_key_frame_get_id(IntPtr keyFrameHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_key_frame_get_frame_set(IntPtr keyFrameHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_key_frame_get_point_cloud(IntPtr keyFrameHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Vector3d sai_key_frame_get_angular_velocity(IntPtr keyFrameHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_key_frame_release(IntPtr keyFrameHandle);
        }
    }
}
