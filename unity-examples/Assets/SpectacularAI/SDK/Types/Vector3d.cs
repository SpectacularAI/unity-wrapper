using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// Vector in R^3, can represent, e.g., velocity, position, or angular velocity
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3d
    {
        public double x, y, z;

        public UnityEngine.Vector3 ToUnity()
        {
            return new UnityEngine.Vector3((float)x, (float)y, (float)z);
        }

        public override string ToString()
        {
            return $"SpectacularAI.Vector3d (x={x}, y={y}, z={z})";
        }
    }
}