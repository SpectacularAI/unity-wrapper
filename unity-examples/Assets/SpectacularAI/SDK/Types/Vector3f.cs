using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// Vector in R^3 (single precision)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3f
    {
        public float x, y, z;

        public override string ToString()
        {
            return $"SpectacularAI.Vector3f (x={x}, y={y}, z={z})";
        }
    }
}