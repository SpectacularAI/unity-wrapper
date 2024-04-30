using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// A 3x3 matrix
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3d
    {
        public double m00, m01, m02;
        public double m10, m11, m12;
        public double m20, m21, m22;

        public override string ToString()
        {
            return $"SpectacularAI.Matrix3d (" +
                $"({m00} {m01} {m02}), " +
                $"({m10} {m11} {m12}), " +
                $"({m20} {m21} {m22}))";
        }
    }
}