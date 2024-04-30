using System.Runtime.InteropServices;

namespace SpectacularAI
{
    /// <summary>
    /// Represents vio parameter (key, value) pair.
    /// </summary>
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct VioParameter
    {
        public string Key;
        public string Value;

        public VioParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}