using Core.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class QueueHelper
    {
        private static readonly ConcurrentQueue<LogEntry> LogQueue = new ConcurrentQueue<LogEntry>();

        public static void Enqueue(LogEntry entry)
        {
            LogQueue.Enqueue(entry);
        }

        public static async Task ProcessQueue(string logDirectory)
        {
            while (LogQueue.TryDequeue(out LogEntry entry))
            {
                FileHelper.WriteLogEntry(entry, logDirectory);
            }
        }
    }
}
