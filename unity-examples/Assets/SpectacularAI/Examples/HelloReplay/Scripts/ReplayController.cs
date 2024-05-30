using UnityEngine;
using SpectacularAI;
using System.Collections.Generic;

namespace HelloReplay
{
    /// <summary>
    /// Minimal example that shows how to use Spectacular AI Replay API in Unity.
    /// </summary>
    public class ReplayController : MonoBehaviour
    {
        [Tooltip("Path to recording")]
        public string ReplayFolder;

        [Tooltip("Internal algorithm parameters")]
        public List<VioParameter> InternalParameters;

        private Replay _replay;

        private UnityEngine.Camera _camera;

        private void OnEnable()
        {
            _camera = UnityEngine.Camera.main;

            if (string.IsNullOrEmpty(ReplayFolder))
            {
                Debug.LogError("Please set ReplayFolder in the Editor!");
                return;
            }

            _replay = new Replay(ReplayFolder, InternalParameters.ToArray());
        }

        private void OnDisable()
        {
            if (_replay != null)
            {
                _replay.Dispose();
                _replay = null;
            }
        }

        private void Update()
        {
            // Loops until one vio output, or end of recording
            while (_replay != null)
            {
                bool moreData = _replay.ReplayOneLine();
                if (!moreData)
                {
                    // Close replay
                    _replay.Dispose();
                    _replay = null;
                }

                if (ReplayAPI.HasOutput())
                {
                    VioOutput output = ReplayAPI.Dequeue();
                    SpectacularAI.Pose pose = output.GetCameraPose(0).Pose;
                    _camera.transform.position = pose.Position;
                    _camera.transform.rotation = pose.Orientation;
                    output.Dispose(); // Must dispose vio outputs
                    break;
                }
            }
        }
    }
}
