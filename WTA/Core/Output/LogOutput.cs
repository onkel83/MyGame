using Core.Helper;
using Core.Model;
using System.Collections.Generic;

namespace Core.Output
{
    public static class LogOutput
    {
        public static void DisplayLogs(List<LogEntry> entries)
        {
            foreach (var entry in entries)
            {
                ConsoleHelper.WriteLogEntry(entry);
            }
        }
    }
}
