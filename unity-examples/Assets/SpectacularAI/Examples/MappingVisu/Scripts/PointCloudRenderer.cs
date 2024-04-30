namespace SpectacularAI.Examples.MappingVisu
{
    using SpectacularAI.Mapping;
    using UnityEngine;
    using UnityEngine.Rendering;

    public sealed class PointCloudRenderer : MonoBehaviour
    {
        public void Initialize(PointCloud pointCloud, Material material)
        {
            Mesh mesh = new Mesh
            {
                indexFormat = pointCloud.Size > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16
            };

            int[] indices = new int[pointCloud.Size];
            for (int i = 0; i < indices.Length; ++i)
            {
                indices[i] = i;
            }

            mesh.SetVertices(pointCloud.Positions, 0, pointCloud.Size);
            mesh.SetIndices(indices, MeshTopology.Points, 0);
            if (pointCloud.HasColors) mesh.SetColors(pointCloud.Colors, 0, pointCloud.Size);
            mesh.UploadMeshData(true);

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;
        }
    }
}
