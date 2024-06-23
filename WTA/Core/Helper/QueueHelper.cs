using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Core.Model;

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
            while (LogQueue.TryDequeue(out var entry))
            {
                string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM}.log");
                string logMessage = $"{entry.LogTime:HH:mm dd.MM.yyyy} [{entry.Level}] {entry.Message}";
                await FileHelper.WriteFileAsync(logFilePath, logMessage + Environment.NewLine);
            }
        }
    }
}
