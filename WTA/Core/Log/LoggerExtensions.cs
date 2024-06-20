using System;
using System.Collections.Generic;
using Core.Helper;
using Core.Model;
using Core.Enum;
using System.IO;

namespace Core.Log
{
    public static class LoggerExtensions
    {
        public static List<LogEntry> GetLogs(DateTime? start = null, DateTime? end = null, LogLevel? level = null)
        {
            var logConfig = LogConfig.GetInstance();
            List<LogEntry> logEntries = FileHelper.ReadLogEntries(logConfig.LogDirectory, start, end, level);

            string archiveDirectory = $"{logConfig.LogDirectory}/archive";
            if (Directory.Exists(archiveDirectory))
            {
                List<LogEntry> archivedEntries = FileHelper.ReadLogEntries(archiveDirectory, start, end, level);
                logEntries.AddRange(archivedEntries);
            }

            return logEntries;
        }

        public static List<LogEntry> GetCurrentMonthLogs(LogLevel? level = null)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);
            return GetLogs(start, end, level);
        }
    }
}