using System;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI
{
    /// <summary>
    /// SpectacularAI Camera object.
    /// </summary>
    public sealed class Camera : IDisposable
    {
        // Native handle to the Camera
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the Camera class.
        /// </summary>
        /// <param name="handle">The native handle to the Camera.</param>
        public Camera(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException(nameof(handle), "Camera handle cannot be IntPtr.Zero");
            }

            _handle = handle;
        }

        /// <summary>
        /// Releases the resources associated with the Camera object.
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

                ExternApi.sai_camera_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the Camera class.
        /// </summary>
        ~Camera()
        {
            Dispose(false);
        }

        /// <summary>
        /// Convert pixel coordinates to camera coordinates
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="ray"></param>
        /// <returns>true if conversion succeeded</returns>
        public bool PixelToRay(PixelCoordinates pixel, out Vector3d ray)
        {
            CheckDisposed();
            return ExternApi.sai_camera_pixel_to_ray(_handle, pixel, out ray);
        }

        /// <summary>
        /// Convert camera coordinates to pixel coordinates
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="pixel"></param>
        /// <returns>true if conversion succeeded</returns>
        public bool RayToPixel(Vector3d ray, out PixelCoordinates pixel)
        {
            CheckDisposed();
            return ExternApi.sai_camera_ray_to_pixel(_handle, ray, out pixel);
        }

        /// <summary>
        /// Defined as the rectified intrinsic matrix if there's distortion.
        /// </summary>
        /// <returns>OpenCV convention
        /// fx 0 ppx
        /// 0 fy ppy
        /// 0  0 1
        /// </returns>
        public Matrix3d GetIntrinsicMatrix()
        {
            CheckDisposed();
            return ExternApi.sai_camera_get_intrinsic_matrix(_handle);
        }

        /// <summary>
        /// Project from camera coordinates to normalized device coordinates (NDC).
        /// </summary>
        /// <param name="nearClip"></param>
        /// <param name="farClip"></param>
        /// <returns></returns>
        public Matrix4d GetProjectionMatrixOpenGL(double nearClip, double farClip)
        {
            CheckDisposed();
            return ExternApi.sai_camera_get_projection_matrix_opengl(_handle, nearClip, farClip);
        }

        /// <summary>
        /// Build pinhole camera with specified intrinsics, width and height.
        /// </summary>
        /// <param name="intrinsicMatrix"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Camera BuildPinhole(Matrix3d intrinsicMatrix, int width, int height)
        {
            IntPtr handle = ExternApi.sai_camera_build_pinhole(intrinsicMatrix, width, height);
            return new Camera(handle);
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
            public static extern bool sai_camera_pixel_to_ray(
                IntPtr cameraHandle,
                PixelCoordinates pixel,
                [Out] out Vector3d ray);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern bool sai_camera_ray_to_pixel(
                IntPtr cameraHandle,
                Vector3d ray,
                [Out] out PixelCoordinates pixel);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix3d sai_camera_get_intrinsic_matrix(IntPtr cameraHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern Matrix4d sai_camera_get_projection_matrix_opengl(
                IntPtr cameraHandle,
                double nearClip,
                double farClip);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_camera_build_pinhole(
                Matrix3d intrinsicMatrix,
                int width,
                int height);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_camera_release(IntPtr cameraHandle);
        }
    }
}