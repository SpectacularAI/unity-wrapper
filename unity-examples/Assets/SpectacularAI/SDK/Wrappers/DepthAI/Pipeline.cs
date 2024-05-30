using System;
using System.Text;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI.DepthAI
{
    /// <summary>
    /// Spectacular AI pipeline for Depth AI API.
    /// </summary>
    public sealed class Pipeline : IDisposable
    {
        // Native handle to the Pipeline
        private readonly IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        // Delegate mapper output
        private delegate void CallbackDelegate(IntPtr callback);
        private CallbackDelegate _mapperOutputCallback = null;

        /// <summary>
        /// Setup a Pipeline
        /// </summary
        /// <param name="configuration">Optional. Define Pipeline configuration.</param>
        /// <param name="internalParameters">Optional. Define internal VIO parameters.</param>
        /// <param name="enableMappingAPI">Optional. Set true to enable mapping API.</param>
        public Pipeline(
            Configuration configuration = null,
            VioParameter[] internalParameters = null,
            bool enableMappingAPI = false)
        {
            if (configuration == null) configuration = new Configuration();
            if (internalParameters == null) internalParameters = new VioParameter[0];
            if (enableMappingAPI)
            {
                Mapping.MappingAPI.Reset();
                _mapperOutputCallback = (mapperOutputHandle) =>
                {
                    CheckDisposed();
                    Mapping.MappingAPI.OnMapperOutput(new Mapping.MapperOutput(mapperOutputHandle));
                };
            }

            _handle = ExternApi.sai_depthai_pipeline_build(
                configuration,
                internalParameters,
                internalParameters.Length,
                _mapperOutputCallback);
        }

        /// <summary>
        /// Releases the resources associated with the Pipeline object.
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

                ExternApi.sai_depthai_pipeline_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the Pipeline class.
        /// </summary>
        ~Pipeline()
        {
            Dispose(false);
        }

        /// <summary>
        /// Start a new VIO session
        /// </summary>
        public Session StartSession()
        {
            CheckDisposed();

            var buffer = new StringBuilder(1000);
            IntPtr sessionHandle = ExternApi.sai_depthai_pipeline_start_session(_handle, buffer);
            if (sessionHandle == IntPtr.Zero)
            {
                throw new Exception(buffer.ToString());
            }

            return new Session(sessionHandle);
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Pipeline));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_depthai_pipeline_build(
                [In] Configuration configuration,
                VioParameter[] vioParameters,
                int internalParametersCount,
                CallbackDelegate onMapperOutput);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_depthai_pipeline_start_session(IntPtr pipelineHandle, StringBuilder errorMsg);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_depthai_pipeline_release(IntPtr pipelineHandle);
        }
    }
}
