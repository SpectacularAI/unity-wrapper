using System.Collections.Generic;

namespace SpectacularAI.Mapping
{
    public static class MappingAPI
    {
        private static Queue<MapperOutput> _outputQueue = new Queue<MapperOutput>();
        private static readonly int _maxQueueSize = 10;

        public static bool HasOutput()
        {
            lock (_outputQueue)
            {
                return _outputQueue.Count > 0;
            }
        }

        public static MapperOutput Dequeue()
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

        internal static void OnMapperOutput(MapperOutput output)
        {
            lock (_outputQueue)
            {
                _outputQueue.Enqueue(output);
                if (_outputQueue.Count > _maxQueueSize)
                {
                    UnityEngine.Debug.Log("Dropping MapperOutput, you are not reading them fast enough!");
                    _outputQueue.Dequeue();
                }
            }
        }
    }
}
