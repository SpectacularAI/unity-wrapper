using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// Quaternion representation of a rotation. Hamilton convention.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public double x, y, z, w;

        public UnityEngine.Quaternion ToUnity()
        {
            return new UnityEngine.Quaternion((float)x, (float)y, (float)z, (float)w);
        }

        public override string ToString()
        {
            return $"SpectacularAI.Quaternion (x={x}, y={y}, z={z}, w={w})";
        }
    }
}