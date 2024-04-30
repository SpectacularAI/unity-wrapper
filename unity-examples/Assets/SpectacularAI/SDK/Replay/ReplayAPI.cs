using System.Collections.Generic;

namespace SpectacularAI
{
    public static class ReplayAPI
    {
        private static readonly int _maxQueueSize = 10;
        private static Queue<VioOutput> _outputQueue = new Queue<VioOutput>();

        public static bool HasOutput()
        {
            lock (_outputQueue)
            {
                return _outputQueue.Count > 0;
            }
        }

        public static VioOutput Dequeue()
        {
            lock (_outputQueue)
            {
                if (_outputQueue.Count == 0) return null;
                return _outputQueue.Dequeue();
            }
        }

        public static void Reset()
        {
            lock (_outputQueue)
            {
                _outputQueue.Clear();
            }
        }

        internal static void OnVioOutput(VioOutput output)
        {
            lock (_outputQueue)
            {
                _outputQueue.Enqueue(output);
                if (_outputQueue.Count > _maxQueueSize)
                {
                    UnityEngine.Debug.Log("Dropping VioOutput, you are not reading them fast enough!");
                    _outputQueue.Dequeue();
                }
            }
        }
    }
}
