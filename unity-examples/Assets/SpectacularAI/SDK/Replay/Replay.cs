using System;
using System.Text;
using System.Runtime.InteropServices;
using SpectacularAI.Native;

namespace SpectacularAI
{
    /// <summary>
    /// Visual-Inertial Odometry API Replay
    /// <summary>
    public sealed class Replay : IDisposable
    {
        // Native handle to the replay
        private IntPtr _handle;

        // To detect redundant calls to Dispose
        private bool _disposed = false;

        // Delegate for vio and mapper output callbacks.
        private delegate void CallbackDelegate(IntPtr callback);
        private CallbackDelegate _vioOutputCallback = null;
        private CallbackDelegate _mapperOutputCallback = null;

        /// <summary>
        /// Setup a Replay
        /// </summary>
        /// <param name="folder">Path to folder containing sensor data, calibration and optionally VIO configuration.</param>
        /// <param name="configuration">Optional. Define internal VIO parameters.</param>
        /// <param name="enableMappingAPI">Optional. Set true to enable mapping API.</param>
        public Replay(
            string folder,
            VioParameter[] configuration = null,
            bool enableMappingAPI = false)
        {
            ReplayAPI.Reset();
            Mapping.MappingAPI.Reset();

            string configurationYAML = "";
            if (configuration != null)
            {
                foreach (var parameter in configuration)
                {
                    configurationYAML += string.Format("{0}: {1}", parameter.Key, parameter.Value);
                }
            }

            if (enableMappingAPI)
            {
                _mapperOutputCallback = (mapperOutputHandle) =>
                {
                    CheckDisposed();
                    Mapping.MappingAPI.OnMapperOutput(new Mapping.MapperOutput(mapperOutputHandle));
                };
            }

            var buffer = new StringBuilder(1000);
            _handle = ExternApi.sai_replay_build(folder, configurationYAML, _mapperOutputCallback, buffer);
            if (_handle == IntPtr.Zero)
            {
                throw new Exception(buffer.ToString());
            }

            _vioOutputCallback = (vioOutputHandle) =>
            {
                CheckDisposed();
                ReplayAPI.OnVioOutput(new VioOutput(vioOutputHandle));
            };

            ExternApi.sai_replay_set_output_callback(_handle, _vioOutputCallback);
        }

        /// <summary>
        /// Releases the resources associated with the Replay object.
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

                ExternApi.sai_replay_release(_handle);

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the Replay class.
        /// </summary>
        ~Replay()
        {
            Dispose(false);
        }

        /// <summary>
        /// Starts replaying data in the background until replay is closed or entire session has been played.
        /// </summary>
        public void StartReplay()
        {
            CheckDisposed();
            ExternApi.sai_replay_start(_handle);
        }

        /// <summary>
        /// Starts replaying data and blocks until replay is closed or entire session has been played.
        /// </summary>
        public void RunReplay()
        {
            CheckDisposed();
            ExternApi.sai_replay_run(_handle);
        }

        /// <summary>
        /// Plays a single line of data.
        /// </summary>
        /// <returns>Returns false when there is no more data, otherwise true. </returns>
        public bool ReplayOneLine()
        {
            CheckDisposed();
            return ExternApi.sai_replay_one_line(_handle);
        }

        /// <summary>
        /// Sets playbacks speed, 1.0 == real time, 2.0 == fast forward 2x, 0.5 == at half speed, -1.0 == unlimited. Defaults to 1.0
        /// </summary>
        /// <param name="speed">Replay speed</param>
        public void SetPlaybackSpeed(double speed)
        {
            CheckDisposed();
            ExternApi.sai_replay_set_playback_speed(_handle, speed);
        }

        /// <summary>
        /// If enabled, read and parse recorded data, but do not feed it to the algorithm. This is
        /// useful for performance measurements: a dry run can be used to estimate the data parsing
        /// time which does not happen in the real-time use case.
        /// </summary>
        /// <param name="isDryRun">Set true for dry run</param>
        public void SetDryRun(bool isDryRun)
        {
            CheckDisposed();
            ExternApi.sai_replay_set_dry_run(_handle, isDryRun);
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Replay));
            }
        }

        private struct ExternApi
        {
            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern IntPtr sai_replay_build(
                [MarshalAs(UnmanagedType.LPStr)] string folder,
                [MarshalAs(UnmanagedType.LPStr)] string configurationYAML,
                CallbackDelegate onMapperOutput,
                StringBuilder errorMsg);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_replay_start(IntPtr replayHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_replay_run(IntPtr replayHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool sai_replay_one_line(IntPtr replayHandle);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_replay_set_playback_speed(IntPtr replayHandle, double speed);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_replay_set_dry_run(
                IntPtr replayHandle,
                [MarshalAs(UnmanagedType.I1)] bool isDryRun);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_replay_set_output_callback(IntPtr replayHandle, CallbackDelegate onVioOutput);

            [DllImport(ApiConstants.saiNativeApi, CallingConvention = ApiConstants.saiCallingConvention)]
            public static extern void sai_replay_release(IntPtr replayHandle);
        }
    }
}
