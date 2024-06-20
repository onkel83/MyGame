using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Core.Log;
using Core.Enum;
using Core.Model;

namespace Core.Helper
{
    public static class FileHelper
    {
        public static void WriteLogEntry(LogEntry entry, string logDirectory)
        {
            try
            {
                EnsureDirectoryExists(logDirectory);
                string fileName = $"{logDirectory}/{DateTime.Now:yyyy-MM}.log";
                string jsonEntry = JsonConvert.SerializeObject(entry) + Environment.NewLine;
                File.AppendAllText(fileName, jsonEntry);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error writing log entry: {ex.Message}", LogLevel.Crit);
            }
        }

        public static List<LogEntry> ReadLogEntries(string logDirectory, DateTime? start = null, DateTime? end = null, LogLevel? level = null)
        {
            try
            {
                List<LogEntry> logEntries = new List<LogEntry>();
                string[] files = Directory.GetFiles(logDirectory, "*.log");
                foreach (string file in files)
                {
                    string[] lines = File.ReadAllLines(file);
                    foreach (string line in lines)
                    {
                        LogEntry? entry = JsonConvert.DeserializeObject<LogEntry>(line);
                        if (entry !=null && (!start.HasValue || entry.LogTime >= start.Value) &&
                            (!end.HasValue || entry.LogTime <= end.Value) &&
                            (!level.HasValue || entry.Level == level.Value))
                        {
                            logEntries.Add(entry);
                        }
                    }
                }
                return logEntries;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error reading log entries: {ex.Message}", LogLevel.Crit);
                return new List<LogEntry>();
            }
        }

        public static void RotateLogs(string logDirectory)
        {
            try
            {
                string oldFileName = $"{logDirectory}/{DateTime.Now.AddMonths(-1):yyyy-MM}.log";
                string newFileName = $"{logDirectory}/archive/{DateTime.Now.AddMonths(-1):yyyy-MM}.log";
                if (File.Exists(oldFileName))
                {
                    EnsureDirectoryExists(Path.GetDirectoryName(newFileName));
                    File.Move(oldFileName, newFileName);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error rotating logs: {ex.Message}", LogLevel.Crit);
            }
        }

        public static void EnsureDirectoryExists(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    Logger.Instance.Log($"Directory created: {directoryPath}", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error ensuring directory exists: {ex.Message}", LogLevel.Crit);
            }
        }

        public static void EnsureFileExists(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    Logger.Instance.Log($"File created: {filePath}", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error ensuring file exists: {ex.Message}", LogLevel.Crit);
            }
        }
    }
}
