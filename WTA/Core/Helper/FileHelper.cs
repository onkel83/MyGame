using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class FileHelper
    {
        public static void EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    LoggerHelper.Log($"Directory created: {path}", LogLevel.Dbug);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error ensuring directory exists: {ex.Message}", LogLevel.Crit);
            }
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error reading file: {ex.Message}", LogLevel.Crit);
                throw;
            }
        }

        public static void WriteFile(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
                LoggerHelper.Log($"File written: {path}", LogLevel.Dbug);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error writing file: {ex.Message}", LogLevel.Crit);
                throw;
            }
        }

        public static async Task WriteFileAsync(string path, string content)
        {
            try
            {
                await File.WriteAllTextAsync(path, content);
                LoggerHelper.Log($"File written: {path}", LogLevel.Dbug);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error writing file: {ex.Message}", LogLevel.Crit);
                throw;
            }
        }

        public static async Task AppendTextAsync(string path, string content)
        {
            try
            {
                await File.AppendAllTextAsync(path, content);
                LoggerHelper.Log($"Appended text to file: {path}", LogLevel.Dbug);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error appending text to file: {ex.Message}", LogLevel.Crit);
                throw;
            }
        }
    }
}
