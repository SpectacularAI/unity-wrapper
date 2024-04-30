using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.Mapping
{
    /// <summary>
    /// SLAM map
    /// Note that when the map goes out of scope it also disposes its keyframes.
    /// </summary>
    public sealed class Map : IDisposable
    {
        // Native handle to the Map
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        private Dictionary<long, KeyFrame> _keyFrames;

        /// <summary>
        /// Initializes a new instance of the Map class.
        /// </summary>
        /// <param name="handle">The native handle to the Map.</param>
        public Map(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "Map handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the Map object.
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
                    if (_keyFrames != null)
                    {
                        // Dispose keyframes to avoid 
                        // SpectacularAI ERROR: Allocator `image frame buffer` reached max capacity: 100.
                        foreach (var kf in _keyFrames)
                        {
                            kf.Value.Dispose();
                        }
                        _keyFrames.Clear();
                    }
                }

                ExternApi.sai_map_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the Map class.
        /// </summary>
        ~Map()
        {
            Dispose(false);
        }

        /// <summary>
        /// SLAM keyframes. Key = ID, Value = KeyFrame.
        /// </summary>
        public Dictionary<long, KeyFrame> KeyFrames
        {
            get
            {
                CheckDisposed();
                
                if (_keyFrames == null)
                {
                    int n = ExternApi.sai_map_get_key_frame_count(_handle);
                    IntPtr[] keyFrameHandles = new IntPtr[n];
                    ExternApi.sai_map_get_key_frames(_handle, keyFrameHandles);

                    _keyFrames = new Dictionary<long, KeyFrame>(n);
                    foreach (IntPtr keyFrameHandle in keyFrameHandles)
                    {
                        KeyFrame kf = new KeyFrame(keyFrameHandle);
                        _keyFrames[kf.Id] = kf;
                    }
                }

                return _keyFrames;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Map));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern int sai_map_get_key_frame_count(IntPtr mapHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_map_get_key_frames(
                IntPtr mapHandle, 
                [Out] IntPtr[] keyFrameHandles);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_map_release(IntPtr mapHandle);
        }
    }
}
