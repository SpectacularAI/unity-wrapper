using System;
using System.IO;
using UnityEngine;
using SpectacularAI.DepthAI;
using UnityEngine.UI;

namespace SpectacularAI.Examples.Record
{
    public class DepthAIController : MonoBehaviour
    {
        [SerializeField]
        private Vio _vio;

        [SerializeField]
        private GameObject _recordingOptionsPanel;

        [SerializeField]
        private GameObject _recordingEscText;

        [SerializeField]
        private InputField _outputInputField;

        [SerializeField]
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private Common.CameraPoseRenderer _cameraPoseRenderer;

        public void Record()
        {
            if (_vio.enabled)
            {
                Debug.LogWarning("Already recording!");
                return;
            }

            string recordingFolder = _outputInputField.text;
            if (recordingFolder.Length == 0) recordingFolder = Path.Combine(".", "data");
            if (_autoSubfolders)
            {
                string autoFolderName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                recordingFolder = Path.Combine(recordingFolder, autoFolderName);
            }

            try
            {
                DirectoryInfo info = Directory.CreateDirectory(recordingFolder);
                Debug.Log($"Recording to {info.FullName}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to create folder: {ex.Message}");
                return;
            }

            _trailRenderer.Clear();
            _cameraPoseRenderer.Show(!_vio.RecordingOnly);
            _recordingOptionsPanel.SetActive(false);
            _recordingEscText.SetActive(true);
            _vio.RecordingFolder = recordingFolder;
            _vio.enabled = true;
        }

        private bool _autoSubfolders = true;

        public void SetSlamEnabled(Toggle toggle)
        {
            _vio.UseSlam = toggle.isOn;
        }

        public void SetFeatureTrackerEnabled(Toggle toggle)
        {
            _vio.UseFeatureTracker = toggle.isOn;
        }

        public void SetStereoEnabled(Toggle toggle)
        {
            _vio.UseStereo = toggle.isOn;
        }

        public void SetAutoSubfoldersEnabled(Toggle toggle)
        {
            _autoSubfolders = toggle.isOn;
        }

        public void SetRecordingOnly(Toggle toggle)
        {
            _vio.RecordingOnly = toggle.isOn;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_vio.enabled) return;
                Record();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool recording = _vio.enabled;
                if (recording)
                {
                    StopRecording();
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
        }

        private void StopRecording()
        {
            _vio.enabled = false;
            _recordingEscText.SetActive(false);
            _recordingOptionsPanel.SetActive(true);
        }
    }
}
