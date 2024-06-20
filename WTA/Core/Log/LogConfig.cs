using Core.Enum;

namespace Core.Log
{
    public class LogConfig
    {
        private static LogConfig _instance = new LogConfig();

        public string LogDirectory { get; set; } = "./Logs";
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info;
        public bool LogToFile { get; set; } = true;
        public bool LogToConsole { get; set; } = true;

        private LogConfig() { }

        public static LogConfig GetInstance()
        {
            _instance ??= new LogConfig();
            return _instance;
        }

        public static void SetInstance(LogConfig config)
        {
            _instance = config;
        }
    }
}