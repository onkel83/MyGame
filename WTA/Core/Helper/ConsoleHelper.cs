using Core.Enum;
using Core.Model;
using System;

namespace Core.Helper
{
    public static class ConsoleHelper
    {
        public static void WriteLogEntry(LogEntry entry)
        {
            Console.ForegroundColor = entry.Level switch
            {
                LogLevel.Crit => ConsoleColor.Red,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Info => ConsoleColor.Green,
                LogLevel.Dbug => ConsoleColor.Blue,
                _ => Console.ForegroundColor
            };
            Console.WriteLine(entry);
            Console.ResetColor();
        }
    }
}
