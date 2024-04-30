using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// 24-bit RGB color
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RGB24
    {
        public byte r, g, b;

        public override string ToString()
        {
            return $"SpectacularAI.RGB24 (r={r}, g={g}, b={b})";
        }

        public UnityEngine.Color ToUnity()
        {
            return new UnityEngine.Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }
    }
}