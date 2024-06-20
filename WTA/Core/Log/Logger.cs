using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Enum;
using Core.Helper;
using Core.Model;

namespace Core.Log
{
    public class Logger
    {
        private static readonly Logger _instance = new Logger();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _processingTask;

        private Logger()
        {
            _processingTask = Task.Run(() => ProcessQueue(_cancellationTokenSource.Token));
        }

        public static Logger Instance => _instance;

        public void Log(string message, LogLevel level, string caller = "", int lineNumber = 0)
        {
            var logConfig = LogConfig.GetInstance();
            if (level > logConfig.MinimumLogLevel) return;

            LogEntry entry = new LogEntry
            {
                Message = message,
                Level = level,
                Caller = caller,
                LineNumber = lineNumber
            };

            if (logConfig.LogToConsole)
            {
                ConsoleHelper.WriteLogEntry(entry);
            }

            if (logConfig.LogToFile)
            {
                QueueHelper.Enqueue(entry);
            }
        }

        public async Task ProcessQueue(CancellationToken cancellationToken)
        {
            var logConfig = LogConfig.GetInstance();
            while (!cancellationToken.IsCancellationRequested)
            {
                QueueHelper.ProcessQueue(logConfig.LogDirectory)?.Wait();
                await Task.Delay(1000);
            }
        }

        public void Shutdown()
        {
            _cancellationTokenSource.Cancel();
            _processingTask.Wait();
        }
    }
}