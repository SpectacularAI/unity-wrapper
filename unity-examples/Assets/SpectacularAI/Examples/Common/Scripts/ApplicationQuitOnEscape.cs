using UnityEngine;

namespace SpectacularAI.Examples.Common
{
    /// <summary>
    /// Quits the application when escape is pressed.
    /// </summary>
    public class ApplicationQuitOnEscape : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
