using UnityEngine;
using SpectacularAI.Mapping;

namespace SpectacularAI.Examples.MappingVisu
{
    public class DepthAIController : MonoBehaviour
    {
        [Tooltip("Material used to render point clouds")]
        [SerializeField]
        private Material _pointCloudMaterial;
        private MapRenderer _mapRenderer;

        private void OnEnable()
        {
            _mapRenderer = new MapRenderer(_pointCloudMaterial);
        }

        private void OnDisable()
        {
            _mapRenderer = null;
        }

        private void Update()
        {
            if (MappingAPI.HasOutput())
            {
                MapperOutput output = MappingAPI.Dequeue();
                _mapRenderer.OnMapperOutput(output);
                output.Dispose(); // Must dispose mapper outputs
            }
        }
    }
}
