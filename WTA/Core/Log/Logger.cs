using System;
using System.Threading;
using System.Threading.Tasks;
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
                var ca = (!string.IsNullOrEmpty(entry.Caller)) ? $"[{entry.Caller}]" : string.Empty;
                var li = (entry.LineNumber > 0) ? $"[{entry.LineNumber:D6}]" : string.Empty;
                ConsoleHelper.WriteLine($"[{entry.LogTime:HH:mm dd.MM.yyyy}]:[{entry.Level}]:[{entry.Message}]:[{ca}]:[{li}]");
            }

            if (logConfig.LogToFile)
            {
                LoggerHelper.Log(entry.Message, level, entry.Caller, entry.LineNumber);
            }
        }

        private async Task ProcessQueue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoggerHelper.ProcessQueue(cancellationToken);
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
