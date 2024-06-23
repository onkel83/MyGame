using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Core.Enums;
using Core.Log;
using Core.Model;

namespace Core.Helper
{
    public static class LoggerHelper
    {
        private static readonly ConcurrentQueue<LogEntry> LogQueue = new ConcurrentQueue<LogEntry>();
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private static readonly Task ProcessingTask = Task.Run(() => ProcessQueue(CancellationTokenSource.Token));

        public static void Log(string message, LogLevel level, string caller = "", int lineNumber = 0)
        {
            var entry = new LogEntry
            {
                LogTime = DateTime.Now,
                Message = message,
                Level = level,
                Caller = caller,
                LineNumber = lineNumber
            };

            LogQueue.Enqueue(entry);
        }

        public static async Task ProcessQueue(CancellationToken cancellationToken)
        {
            var logConfig = LogConfig.GetInstance();
            while (!cancellationToken.IsCancellationRequested)
            {
                while (LogQueue.TryDequeue(out var entry))
                {
                    try
                    {
                        // Log to console if enabled
                        if (logConfig.LogToConsole)
                        {
                            ConsoleHelper.WriteLine($"{entry.LogTime:HH:mm dd.MM.yyyy} [{entry.Level}] {entry.Message}", GetColorForLogLevel(entry.Level));
                        }

                        // Log to file if enabled
                        if (logConfig.LogToFile)
                        {
                            string logFilePath = Path.Combine(logConfig.LogDirectory, $"{DateTime.Now:yyyy-MM}.log");
                            string CL = string.IsNullOrEmpty(entry.Caller) ? (string.Empty) : $"::[{entry.Caller}]"; 
                            CL += (entry.LineNumber <= 0 ? string.Empty : $"::[{entry.LineNumber}]");
                            string logMessage = $"[{entry.LogTime:HH:mm dd.MM.yyyy}]::[{entry.Level}]::[{entry.Message}]{CL}";
                            await FileHelper.AppendTextAsync(logFilePath, logMessage + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.WriteLine($"Error processing log action: {ex.Message}", ConsoleColor.Red);
                    }
                }
                await Task.Delay(1001);
            }
        }

        public static ConsoleColor GetColorForLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Crit => ConsoleColor.Red,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Info => ConsoleColor.Green,
                LogLevel.Dbug => ConsoleColor.Blue,
                _ => ConsoleColor.White,
            };
        }

        public static void Shutdown()
        {
            CancellationTokenSource.Cancel();
            ProcessingTask.Wait();
        }
    }
}
