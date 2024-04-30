using UnityEngine;

namespace SpectacularAI.Examples.Common
{
    public class CameraPoseRenderer : MonoBehaviour
    {
        public float Scale = 0.1f;

        private static readonly int[] _indices =
        {
            0, 1,
            0, 2,
            0, 3,
            0, 4,
            1, 2,
            2, 3,
            3, 4,
            4, 1,
        };

        private void Start()
        {
            Vector3[] vertices =
            {
                Scale * new Vector3(0.0f, 0.0f, 0.0f), // origin
                Scale * new Vector3(-1.0f, 0.7f, 1.0f), // top-left
                Scale * new Vector3(1.0f, 0.7f, 1.0f), // top-right
                Scale * new Vector3(1.0f, -0.7f, 1.0f), // bottom-right
                Scale * new Vector3(-1.0f, -0.7f, 1.0f) // bottom-left
            };

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices, 0, vertices.Length);
            mesh.SetIndices(_indices, MeshTopology.Lines, 0);

            gameObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
    }
}