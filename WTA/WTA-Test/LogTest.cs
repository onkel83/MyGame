using Core.Enums;
using Core.Helper;
using Core.Log;
using Core.Model;
using Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTA_Test
{
    public class LogTest
    {
        public LogTest() { Run(); }

        private static void Run()
        {
            ConfigHelper.SetConfigValue("LogConfig", "LogToFile", "true");
            ConfigHelper.SetConfigValue("LogConfig", "LogToConsole", "true");
            ConfigHelper.SetConfigValue("LogConfig", "LogDirectory", "./logs");
            ConfigHelper.SetConfigValue("LogConfig", "MinimumLogLevel", "Info");

            // Initialize Logger
            Logger.Instance.Log("Logger initialized.", LogLevel.Info);

            // Log messages at different levels
            LogMessages();

            // Wait to ensure all logs are processed
            Task.Delay(10000);

            // Display logs from the current month
            DisplayCurrentMonthLogs();

            // Shutdown Logger
            Logger.Instance.Shutdown();

            Console.WriteLine("Please press [Enter] to exit.");
            Console.ReadLine();
        }

        static void LogMessages()
        {
            Logger.Instance.Log("This is an info message", LogLevel.Info);
            Logger.Instance.Log("This is a debug message", LogLevel.Dbug);
            Logger.Instance.Log("This is a warning message", LogLevel.Warn);
            Logger.Instance.Log("This is a critical message", LogLevel.Crit);
        }

        static void DisplayCurrentMonthLogs()
        {
            List<LogEntry> logs = LoggerExtensions.GetCurrentMonthLogs();
            LogOutput.DisplayLogs(logs);
        }
    }
}
