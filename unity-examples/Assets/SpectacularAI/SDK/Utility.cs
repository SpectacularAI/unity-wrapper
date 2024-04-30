using UnityEngine;

namespace SpectacularAI
{
    public static class Utility
    {
        private static readonly Matrix4x4 SPECTACULAR_AI_WORLD_TO_UNITY_WORLD = CreateMatrix(
            1, 0, 0, 0,
            0, 0, 1, 0,
            0, 1, 0, 0,
            0, 0, 0, 1);
        private static readonly Matrix4x4 SPECTACULAR_AI_CAMERA_TO_UNITY_CAMERA = CreateMatrix(
            1,  0, 0, 0,
            0, -1, 0, 0,
            0,  0, 1, 0,
            0,  0, 0, 1);
        private static readonly Matrix4x4 UNITY_APRIL_TAG_TO_SPECTACULAR_AI_APRIL_TAG = CreateMatrix(
            -1, 0, 0, 0,
             0, 0, 1, 0,
             0, -1, 0, 0,
             0, 0, 0, 1);
        private static readonly Matrix4x4 UNITY_WORLD_TO_SPECTACULAR_AI_WORLD = SPECTACULAR_AI_WORLD_TO_UNITY_WORLD.inverse;
        private static readonly Matrix4x4 UNITY_CAMERA_TO_SPECTACULAR_AI_CAMERA = SPECTACULAR_AI_CAMERA_TO_UNITY_CAMERA.inverse;
        private static readonly Matrix4x4 SPECTACULAR_AI_APRIL_TAG_TO_UNITY_APRIL_TAG = UNITY_APRIL_TAG_TO_SPECTACULAR_AI_APRIL_TAG.inverse;

        private static Matrix4x4 CreateMatrix(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            return new Matrix4x4( // Vectors given by column, so it looks transposed here
                new Vector4(m00, m10, m20, m30),
                new Vector4(m01, m11, m21, m31),
                new Vector4(m02, m12, m22, m32),
                new Vector4(m03, m13, m23, m33)
            );
        }

        public static Matrix4x4 TransformCameraToWorldMatrixToUnity(Matrix4d cameraToWorld)
        {
            // unity_camera->unity_world = unity_camera->sai_camera->sai_world->unity_world
            //                           = sai_world->unity_world * sai_camera->sai_world * unity_camera->sai_camera
            return SPECTACULAR_AI_WORLD_TO_UNITY_WORLD * 
                cameraToWorld.ToUnity() * 
                UNITY_CAMERA_TO_SPECTACULAR_AI_CAMERA;
        }

        public static Matrix4x4 TransformWorldToCameraMatrixToUnity(Matrix4d worldToCamera)
        {
            // unity_world->unity_camera = unity_world->sai_world->sai_camera->unity_camera
            //                           = sai_camera->unity_camera * sai_world->sai_camera * unity_world->sai_world
            return SPECTACULAR_AI_CAMERA_TO_UNITY_CAMERA * 
                worldToCamera.ToUnity() * 
                UNITY_WORLD_TO_SPECTACULAR_AI_WORLD;
        }

        public static UnityEngine.Quaternion TransformCameraToWorldQuaternionToUnity(Quaternion cameraToWorld)
        {
            // Must use rotation matrices because of left->right handed flip (quaternions do not support it).
            return (SPECTACULAR_AI_WORLD_TO_UNITY_WORLD *
                Matrix4x4.Rotate(cameraToWorld.ToUnity()) * 
                UNITY_CAMERA_TO_SPECTACULAR_AI_CAMERA).rotation;
        }

        public static Vector3 TransformCameraPointToUnity(Vector3f point)
        {
            return new Vector3(point.x, -point.y, point.z);
        }

        public static Vector3 TransformCameraDirectionToUnity(Vector3f direction)
        {
            return new Vector3(direction.x, -direction.y, direction.z);
        }

        public static Vector3 TransformWorldPointToUnity(Vector3d point)
        {
            return new Vector3((float)point.x, (float)point.z, (float)point.y);
        }

        public static Vector3 TransformWorldDirectionToUnity(Vector3d direction)
        {
            return new Vector3((float)direction.x, (float)direction.z, (float)direction.y);
        }

        public static Vector3 TransformWorldAngularVelocityToUnity(Vector3d angularVelocity)
        {
            // left->right handed, need to invert rotation
            return -TransformWorldDirectionToUnity(angularVelocity);
        }

        public static Matrix4d TransformCameraToWorldMatrixToSpectacularAI(Matrix4x4 cameraToWorld)
        {
            // sai_camera->sai_world = sai_camera->unity_camera->unity_world->sai_world
            //                           = unity_world->sai_world * unity_camera->unity_world * sai_camera->unity_camera
            return Matrix4d.FromUnity(
                UNITY_WORLD_TO_SPECTACULAR_AI_WORLD * 
                cameraToWorld * 
                SPECTACULAR_AI_CAMERA_TO_UNITY_CAMERA);
        }

        public static Matrix4x4 ConvertAprilTagToSpectacularAI(Matrix4x4 tagToWorld)
        {
            // sai_tag->sai_world = sai_tag->unity_tag->unity_world->sai_world =
            // unity_world->sai_world * unity_tag->unity_world * sai_tag->unity_tag
            return UNITY_WORLD_TO_SPECTACULAR_AI_WORLD *
                tagToWorld *
                SPECTACULAR_AI_APRIL_TAG_TO_UNITY_APRIL_TAG;
        }

        /// <summary>
        /// Predicts position given current position, velocity (m/s) and delta time (seconds).
        /// </summary>
        /// <param name="position">Position in Unity world coordinates</param>
        /// <param name="velocity">Velocity (m/s) in Unity world coordinates</param>
        /// <param name="dt">Delta time (seconds)</param>
        /// <returns></returns>
        public static Vector3 PredictPosition(Vector3 position, Vector3 velocity, float dt)
        {
            if (dt <= 0) return position;
            return position + velocity * dt;
        }

        /// <summary>
        /// Predicts orientation given current orientation qt0, constant angular velocity w (rad/s) and delta time (seconds).
        /// Source: https://users.aalto.fi/~ssarkka/pub/quat.pdf
        /// </summary>
        /// <param name="qt0">Orientatation quaternion (local->world)</param>
        /// <param name="w">Angular velocity (rad/s) in Unity world coordinates.)</param>
        /// <param name="dt">Delta time (seconds)</param>
        /// <returns>Predicted local->world orientation at time=t0 + dt</returns>
        public static UnityEngine.Quaternion PredictOrientation(UnityEngine.Quaternion qt0, Vector3 w, float dt)
        {
            if (dt <= 0) return qt0;
            float wNorm = w.magnitude;
            if (wNorm < 1e-4) return qt0;
            
            // The formulas assume world->local
            qt0 = UnityEngine.Quaternion.Inverse(qt0);

            // q(t) = q(t0) * p(t0, t; w)
            // where
            // d(t0, t; w) = (cos(|w| * dt/2)           )
            //               (w / |w| * sin(|w| * dt/2) )
            float dt2 = dt / 2;
            Vector4 p = new Vector4(
                Mathf.Cos(wNorm * dt2),
                -w.x / wNorm * Mathf.Sin(wNorm * dt2),
                -w.y / wNorm * Mathf.Sin(wNorm * dt2),
                -w.z / wNorm * Mathf.Sin(wNorm * dt2));
            Vector4 q = new Vector4(qt0.w, qt0.x, qt0.y, qt0.z);

            // q(t) = q(t0) * p(t0, t; w)
            Vector4 qt = new Vector4(
                q[0] * p[0] - q[1] * p[1] - q[2] * p[2] - q[3] * p[3],
                q[1] * p[0] + q[0] * p[1] - q[3] * p[2] + q[2] * p[3],
                q[2] * p[0] + q[3] * p[1] + q[0] * p[2] - q[1] * p[3],
                q[3] * p[0] - q[2] * p[1] + q[1] * p[2] + q[0] * p[3]);

            return UnityEngine.Quaternion.Inverse(new UnityEngine.Quaternion(qt[1], qt[2], qt[3], qt[0]));
        }
    }
}