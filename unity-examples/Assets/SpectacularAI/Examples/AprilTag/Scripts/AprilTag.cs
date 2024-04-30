using UnityEngine;

namespace SpectacularAI.Examples.AprilTag
{

    public class AprilTag : MonoBehaviour
    {
        [Tooltip("April tag id (integer)")]
        [field: SerializeField] public int Id { get; private set; }

        [Tooltip("April tag size in meters")]
        [field: SerializeField] public float Size { get; private set; }

        [Tooltip("April tag family: options tag36h11, tag25h9, tag16h5, tagCircle21h7, tagCircle49h12, tagStandard41h12, tagStandard52h13, tagCustom48h12")]
        [field: SerializeField] public string Family { get; private set; }

        public Matrix4x4 TagToWorld
        {
            get
            {
                Vector3 t = transform.position;
                UnityEngine.Quaternion q = transform.rotation;
                Matrix4x4 unityTagToUnityWorld = Matrix4x4.TRS(t, q, Vector3.one);
                return Utility.ConvertAprilTagToSpectacularAI(unityTagToUnityWorld);
            }
        }

        private static string SerializeMatrix4x4(Matrix4x4 m)
        {
            return
                "[" +
                $"[{m.m00}, {m.m01}, {m.m02}, {m.m03}]," +
                $"[{m.m10}, {m.m11}, {m.m12}, {m.m13}]," +
                $"[{m.m20}, {m.m21}, {m.m22}, {m.m23}]," +
                $"[{m.m30}, {m.m31}, {m.m32}, {m.m33}]" +
                "]";
        }

        public string ToJson()
        {
            return
                "   {\n" +
                $"      \"id\": {Id},\n" +
                $"      \"size\": {Size},\n" +
                $"      \"family\": \"{Family}\",\n" +
                $"      \"tagToWorld\": {SerializeMatrix4x4(TagToWorld)}\n" +
                "   }";
        }
    }
}