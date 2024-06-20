using Core.Model;
using System;
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

        public static Task? ProcessQueue(string logDirectory)
        {

            while (LogQueue.TryDequeue(out LogEntry entry))
            {
                Task.Run(() =>
                {
                    FileHelper.WriteLogEntry(entry, logDirectory);

                }).Wait();
            }
            return null;
        }
    }
}
