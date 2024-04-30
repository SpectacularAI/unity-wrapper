using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.Mapping
{
    /// <summary>
    /// PointCloud object.
    /// </summary>
    public sealed class PointCloud : IDisposable
    {
        // Native handle to the PointCloud
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        private UnityEngine.Vector3[] _positionData;
        private UnityEngine.Vector3[] _normalData;
        private UnityEngine.Color[] _colorData;

        /// <summary>
        /// Initializes a new instance of the PointCloud class.
        /// </summary>
        /// <param name="handle">The native handle to the PointCloud.</param>
        public PointCloud(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "PointCloud handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the PointCloud object.
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
                    _positionData = null;
                    _normalData = null;
                    _colorData = null;
                }

                ExternApi.sai_point_cloud_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the PointCloud class.
        /// </summary>
        ~PointCloud()
        {
            Dispose(false);
        }

        /// <summary>
        /// Returns the number of points in the point cloud.
        /// </summary>
        public int Size
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_point_cloud_get_size(_handle);
            }
        }

        /// <summary>
        /// Returns true if the point cloud has 0 points.
        /// </summary>
        public bool Empty
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_point_cloud_empty(_handle);
            }
        }

        /// <summary>
        /// Returns true if the point cloud has optional normal data.
        /// </summary>
        public bool HasNormals
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_point_cloud_has_normals(_handle);
            }
        }

        /// <summary>
        /// Returns true if the the point cloud has optional color data.
        /// </summary>
        public bool HasColors
        {
            get
            {
                CheckDisposed();
                return ExternApi.sai_point_cloud_has_colors(_handle);
            }
        }

        /// <summary>
        /// Get position data, null if point cloud is empty
        /// </summary>
        public UnityEngine.Vector3[] Positions
        {
            get
            {
                CheckDisposed();

                if (!Empty && _positionData == null)
                {
                    IntPtr ptr = ExternApi.sai_point_cloud_get_position_data(_handle);
                    int vector3fSize = Marshal.SizeOf<Vector3f>();
                    int n = Size;
                    
                    _positionData = new UnityEngine.Vector3[n];
                    for (int i = 0; i < n; i++)
                    {
                        IntPtr positionPtr = IntPtr.Add(ptr, i * vector3fSize);
                        _positionData[i] = Utility.TransformCameraPointToUnity(Marshal.PtrToStructure<Vector3f>(positionPtr));
                    }
                }

                return _positionData;
            }
        }

        /// <summary>
        /// Get normal data, null if point cloud is empty or HasNormals is false
        /// </summary>
        public UnityEngine.Vector3[] Normals
        {
            get
            {
                CheckDisposed();

                if (!Empty && HasNormals && _normalData == null)
                {
                    IntPtr ptr = ExternApi.sai_point_cloud_get_normal_data(_handle);
                    int vector3fSize = Marshal.SizeOf<Vector3f>();
                    int n = Size;

                    _normalData = new UnityEngine.Vector3[n];
                    for (int i = 0; i < n; i++)
                    {
                        IntPtr normalPtr = IntPtr.Add(ptr, i * vector3fSize);
                        _normalData[i] = Utility.TransformCameraDirectionToUnity(Marshal.PtrToStructure<Vector3f>(normalPtr));
                    }
                }

                return _normalData;
            }
        }

        /// <summary>
        /// Get color data, null if point cloud is empty or HasColors is false
        /// </summary>
        public UnityEngine.Color[] Colors
    {
        get
            {
                CheckDisposed();

                if (!Empty && HasColors && _colorData == null)
                {
                    IntPtr ptr = ExternApi.sai_point_cloud_get_rgb24_data(_handle);
                    int n = Size;
                    int rgb24Size = Marshal.SizeOf<RGB24>();

                    _colorData = new UnityEngine.Color[n];
                    for (int i = 0; i < n; i++)
                    {
                        IntPtr colorPtr = IntPtr.Add(ptr, i * rgb24Size);
                        _colorData[i] = Marshal.PtrToStructure<RGB24>(colorPtr).ToUnity();
                    }

                }

                return _colorData;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PointCloud));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern int sai_point_cloud_get_size(IntPtr pointCloudHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern bool sai_point_cloud_empty(IntPtr pointCloudHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern bool sai_point_cloud_has_normals(IntPtr pointCloudHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern bool sai_point_cloud_has_colors(IntPtr pointCloudHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_point_cloud_get_position_data(IntPtr pointCloudHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_point_cloud_get_normal_data(IntPtr pointCloudHandle);
            
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_point_cloud_get_rgb24_data(IntPtr pointCloudHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_point_cloud_release(IntPtr pointCloudHandle);
        }
    }
}
