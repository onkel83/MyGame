using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Enums;
using Core.Helper;
using Core.Model;

namespace Core.Log
{
    public static class LoggerExtensions
    {
        public static List<LogEntry> GetCurrentMonthLogs()
        {
            var logConfig = LogConfig.GetInstance();
            string logFilePath = Path.Combine(logConfig.LogDirectory, $"{DateTime.Now:yyyy-MM}.log");

            if (!FileHelper.FileExists(logFilePath))
                return new List<LogEntry>();

            var logEntries = FileHelper.ReadFile(logFilePath)
                .Split(Environment.NewLine)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => ParseLogEntry(line))
                .ToList();

            return logEntries;
        }

        private static LogEntry ParseLogEntry(string logLine)
        {
            var parts = logLine.Split(new[] { " [" }, StringSplitOptions.None);
            var dateTimePart = parts[0].Trim();
            var logLevelPart = parts[1].Split(']')[0].Trim();
            var messagePart = parts[1].Split(']')[1].Trim();

            return new LogEntry
            {
                LogTime = DateTime.ParseExact(dateTimePart, "HH:mm dd.MM.yyyy", null),
                Level = Enum.Parse<LogLevel>(logLevelPart),
                Message = messagePart
            };
        }
    }
}
