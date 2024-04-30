using UnityEngine;
using SpectacularAI;

namespace HelloReplay
{
    /// <summary>
    /// Minimal example that shows how to use Spectacular AI Replay API in Unity.
    /// </summary>
    public class ReplayController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Path to recording")]
        private string _ReplayFolder;

        private Replay _replay;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(_ReplayFolder))
            {
                Debug.LogError("Please set ReplayFolder in the Editor!");
                return;
            }

            _replay = new Replay(_ReplayFolder);
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
            if (_replay != null)
            {
                bool moreData = _replay.ReplayOneLine();
                if (!moreData)
                {
                    // Close replay
                    _replay.Dispose();
                    _replay = null;
                }
            }

            if (ReplayAPI.HasOutput())
            {
                VioOutput output = ReplayAPI.Dequeue();
                Debug.Log(output.Status);
                output.Dispose(); // Must dispose vio outputs
            }
        }
    }
}
