using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// Coordinates of an image pixel, subpixel accuracy
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PixelCoordinates
    {
        float x, y;

        public override string ToString()
        {
            return $"SpectacularAI.PixelCoordinates (x={x}, y={y})";
        }
    };
}