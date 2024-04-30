using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// A 4x4 matrix
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4d
    {
        public double m00, m01, m02, m03;
        public double m10, m11, m12, m13;
        public double m20, m21, m22, m23;
        public double m30, m31, m32, m33;

        public UnityEngine.Matrix4x4 ToUnity()
        {
            return new UnityEngine.Matrix4x4( // Vectors given by column, so it looks transposed here
                new UnityEngine.Vector4((float)m00, (float)m10, (float)m20, (float)m30),
                new UnityEngine.Vector4((float)m01, (float)m11, (float)m21, (float)m31),
                new UnityEngine.Vector4((float)m02, (float)m12, (float)m22, (float)m32),
                new UnityEngine.Vector4((float)m03, (float)m13, (float)m23, (float)m33)
            );
        }

        public static Matrix4d FromUnity(UnityEngine.Matrix4x4 m)
        {
            Matrix4d s;
            s.m00 = m.m00;
            s.m01 = m.m01;
            s.m02 = m.m02;
            s.m03 = m.m03;

            s.m10 = m.m10;
            s.m11 = m.m11;
            s.m12 = m.m12;
            s.m13 = m.m13;

            s.m20 = m.m20;
            s.m21 = m.m21;
            s.m22 = m.m22;
            s.m23 = m.m23;

            s.m30 = m.m30;
            s.m31 = m.m31;
            s.m32 = m.m32;
            s.m33 = m.m33;
            return s;
        }

        public override string ToString()
        {
            return $"SpectacularAI.Matrix4d (" +
                $"({m00} {m01} {m02} {m03}), " +
                $"({m10} {m11} {m12} {m13}), " +
                $"({m20} {m21} {m22} {m23})) " +
                $"({m30} {m31} {m32} {m33}))";
        }
    }
}