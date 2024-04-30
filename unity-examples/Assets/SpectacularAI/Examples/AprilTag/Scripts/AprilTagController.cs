using System.IO;
using System.Text;
using UnityEngine;
using SpectacularAI.DepthAI;

namespace SpectacularAI.Examples.AprilTag
{
    public class AprilTagController : MonoBehaviour
    {
        [SerializeField]
        private Vio _vio;

        string SerializeAprilTags(AprilTag[] aprilTags)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 0; i < aprilTags.Length; ++i)
            {
                AprilTag tag = aprilTags[i];
                sb.Append(tag.ToJson());
                if (i < aprilTags.Length - 1)
                {
                    sb.Append(",");
                }
                sb.Append("\n");
            }
            sb.AppendLine("]");

            string json = sb.ToString();
            string filePath = Path.Combine(Application.persistentDataPath, "tags.json");
            File.WriteAllText(filePath, json);
            Debug.Log("April Tag file written to: " + filePath);

            return filePath;
        }

        void Start()
        {
            AprilTag[] aprilTags = FindObjectsOfType<AprilTag>();
            _vio.AprilTagPath = SerializeAprilTags(aprilTags);
            _vio.enabled = true;
        }
    }
}
