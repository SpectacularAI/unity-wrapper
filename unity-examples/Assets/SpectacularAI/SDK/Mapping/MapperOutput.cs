using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.Mapping
{
    /// <summary>
    /// SLAM output object.
    /// </summary>
    public sealed class MapperOutput : IDisposable
    {
        // Native handle to the MapperOutput
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        private Map _map;
        private List<long> _updatedKeyFrames = null;

        /// <summary>
        /// Initializes a new instance of the MapperOutput class.
        /// </summary>
        /// <param name="handle">The native handle to the MapperOutput.</param>
        public MapperOutput(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "MapperOutput handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the MapperOutput object.
        /// Note that the Map is disposed!
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
                    if (_map != null) _map.Dispose();
                }

                ExternApi.sai_mapper_output_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the MapperOutput class.
        /// </summary>
        ~MapperOutput()
        {
            Dispose(false);
        }

        /// <summary>
        /// Current SLAM map.
        /// </summary>
        public Map Map
        {
            get
            {
                CheckDisposed();
                
                if (_map == null)
                {
                    IntPtr mapHandle = ExternApi.sai_mapper_output_get_map(_handle);
                    _map = new Map(mapHandle);
                }

                return _map;
            }
        }

        /// <summary>
        /// List of keyframes that were modified or deleted. Ordered from oldest to newest.
        /// </summary>
        public List<long> UpdatedKeyFrames
        {
            get
            {
                CheckDisposed();
                if (_updatedKeyFrames == null)
                {
                    int n = ExternApi.sai_mapper_output_get_updated_key_frames(_handle, out IntPtr updatedKeyFramesHandle);
                    long[] arr = new long[n];
                    Marshal.Copy(updatedKeyFramesHandle, arr, 0, n);
                    _updatedKeyFrames = new List<long>(arr);
                }

                return _updatedKeyFrames;
            }
        }

        /// <summary>
        /// True for the last update before program exit
        /// </summary>
        public bool FinalMap
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_mapper_output_get_final_map(_handle);
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(MapperOutput));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_mapper_output_get_map(IntPtr mapperOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern int sai_mapper_output_get_updated_key_frames(
                IntPtr mapperOutputHandle,
                [Out] out IntPtr updatedKeyFramesHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern bool sai_mapper_output_get_final_map(IntPtr mapperOutputHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_mapper_output_release(IntPtr mapperOutputHandle);
        }
    }
}
