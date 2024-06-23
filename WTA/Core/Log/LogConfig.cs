using Core.Enums;
using Core.Helper;
using System;

namespace Core.Log
{
    public class LogConfig
    {
        private static LogConfig _instance = new LogConfig();
        private LogLevel _level = LogLevel.Dbug;

        public bool LogToFile { get; set; }
        public bool LogToConsole { get; set; }
        public string LogDirectory { get; set; }
        public LogLevel MinimumLogLevel { get => _level; set => _level = value; }

        private LogConfig()
        {
            LogDirectory = string.Empty;
            LoadConfig();
        }

        public static LogConfig GetInstance()
        {
            return _instance ??= new LogConfig();
        }

        private void LoadConfig()
        {
            LogToFile = ConfigHelper.GetConfigValue("LogConfig", "LogToFile") == "true";
            LogToConsole = ConfigHelper.GetConfigValue("LogConfig", "LogToConsole") == "true";
            LogDirectory = ConfigHelper.GetConfigValue("LogConfig", "LogDirectory") ?? "./logs";
            Enum.TryParse(ConfigHelper.GetConfigValue("LogConfig", "MinimumLogLevel"), out _level);
        }
    }
}
