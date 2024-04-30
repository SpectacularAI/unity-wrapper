using UnityEngine;

namespace SpectacularAI.Examples.Common
{
    public class ThirdPersonFollow : MonoBehaviour
    {
        public Transform Target;

        private void Update()
        {
            transform.LookAt(Target, Vector3.up);    
        }
    }
}