using Core.Helper;
using System;
using System.IO;

namespace Core.Log
{
    public class LogConfig
    {
        private static LogConfig _instance = new LogConfig();
        private LogLevel _level;

        public bool LogToFile { get; set; }
        public bool LogToConsole { get; set; }
        public string LogDirectory { get; set; }
        public LogLevel MinimumLogLevel { get => _level; set => _level = value; }

        private LogConfig()
        {
            LogToConsole = false;
            LogToFile = false;
            LogDirectory = string.Empty;
            MinimumLogLevel = LogLevel.Dbug;
            LoadConfig();
        }

        public static LogConfig GetInstance()
        {
            return _instance ??= new LogConfig();
        }

        private void LoadConfig()
        {
            LogToFile = ConfigHelper.GetConfigValue("LogConfig", "LogToFile") == "true";
            LogToConsole = ConfigHelper.GetConfigValue("LogConfig", "LogToConsole") == "false";
            LogDirectory = ConfigHelper.GetConfigValue("LogConfig", "LogDirectory",$"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")}");
            Enum.TryParse(ConfigHelper.GetConfigValue("LogConfig", "MinimumLogLevel", "Info"), out _level);
        }
    }
}
