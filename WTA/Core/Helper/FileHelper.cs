using System;
using System.Collections.Generic;
using System.IO;
using Core.Model;
using Newtonsoft.Json;
using Core.Log;
using Core.Enum;

namespace Core.Helper
{
    public static class FileHelper
    {
        public static void WriteLogEntry(LogEntry entry, string logDirectory)
        {
            EnsureDirectoryExists(logDirectory);
            string fileName = $"{logDirectory}/{DateTime.Now:yyyy-MM}.log";
            string jsonEntry = JsonConvert.SerializeObject(entry) + Environment.NewLine;
            File.AppendAllText(fileName, jsonEntry); // Anhängen an die vorhandene Datei
        }

        public static List<LogEntry> ReadLogEntries(string logDirectory, DateTime? start = null, DateTime? end = null, LogLevel? level = null)
        {
            List<LogEntry> logEntries = new List<LogEntry>();
            string[] files = Directory.GetFiles(logDirectory, "*.log");
            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    LogEntry entry = JsonConvert.DeserializeObject<LogEntry>(line)??new LogEntry();
                    if ((!start.HasValue || entry.LogTime >= start.Value) &&
                        (!end.HasValue || entry.LogTime <= end.Value) &&
                        (!level.HasValue || entry.Level == level.Value) && entry != null)
                    {
                        logEntries.Add(entry);
                    }
                }
            }
            return logEntries;
        }

        public static void RotateLogs(string logDirectory)
        {
            string oldFileName = $"{logDirectory}/{DateTime.Now.AddMonths(-1):yyyy-MM}.log";
            string newFileName = $"{logDirectory}/archive/{DateTime.Now.AddMonths(-1):yyyy-MM}.log";
            if (File.Exists(oldFileName))
            {
                EnsureDirectoryExists(Path.GetDirectoryName(newFileName));
                File.Move(oldFileName, newFileName);
            }
        }

        public static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Logger.Instance.Log($"Directory created: {directoryPath}", Core.Enum.LogLevel.Info);
            }
        }

        public static void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
                Logger.Instance.Log($"File created: {filePath}", Core.Enum.LogLevel.Info);
            }
        }
    }
}
