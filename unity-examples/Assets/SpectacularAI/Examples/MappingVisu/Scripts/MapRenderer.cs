using SpectacularAI.Mapping;
using System.Collections.Generic;

namespace SpectacularAI.Examples.MappingVisu
{
    public class MapRenderer
    {
        private UnityEngine.Material _pointCloudMaterial;
        private UnityEngine.GameObject _map;
        private Dictionary<long, UnityEngine.GameObject> _keyFrames = new Dictionary<long, UnityEngine.GameObject>();

        public MapRenderer(UnityEngine.Material pointCloudMaterial)
        {
            _pointCloudMaterial = pointCloudMaterial;
            _map = new UnityEngine.GameObject("SLAM Map");
        }

        ~MapRenderer()
        {
            UnityEngine.GameObject.Destroy(_map);
        }

        private void AddKeyFrame(KeyFrame kf)
        {
            UnityEngine.GameObject keyFrame = new UnityEngine.GameObject("KeyFrame" + kf.Id);
            keyFrame.AddComponent<Common.CameraPoseRenderer>();
            keyFrame.transform.parent = _map.transform;
            if (!kf.PointCloud.Empty)
            {
                UnityEngine.GameObject pointCloudRenderer = new UnityEngine.GameObject("PointCloudRenderer");
                var rendr = pointCloudRenderer.AddComponent<PointCloudRenderer>();
                rendr.Initialize(kf.PointCloud, _pointCloudMaterial);
                pointCloudRenderer.transform.parent = keyFrame.transform;
            }
            _keyFrames.Add(kf.Id, keyFrame);
        }

        private void UpdateKeyFrame(KeyFrame kf)
        {
            if (!_keyFrames.ContainsKey(kf.Id))
            {
                if (kf.PointCloud == null) return;
                AddKeyFrame(kf);
            }

            UnityEngine.GameObject keyFrame = _keyFrames[kf.Id];
            Pose cameraToWorld = kf.FrameSet.PrimaryFrame.CameraPose.Pose;
            keyFrame.transform.position = cameraToWorld.Position;
            keyFrame.transform.rotation = cameraToWorld.Orientation;
        }

        private void RemoveKeyFrame(long kfId)
        {
            if (_keyFrames.ContainsKey(kfId)) 
            {
                UnityEngine.GameObject keyFrame = _keyFrames[kfId];
                UnityEngine.Object.Destroy(keyFrame);
                _keyFrames.Remove(kfId);
            }
        }

        public void OnMapperOutput(MapperOutput output)
        {
            Map map = output.Map;

            foreach (long kfId in output.UpdatedKeyFrames)
            {
                if (map.KeyFrames.ContainsKey(kfId))
                {
                    UpdateKeyFrame(map.KeyFrames[kfId]);
                }
                else
                {
                    RemoveKeyFrame(kfId);
                }
            }
        }
    }
}