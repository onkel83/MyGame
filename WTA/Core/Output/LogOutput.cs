using System.Collections.Generic;
using Core.Helper;
using Core.Model;

namespace Core.Output
{
    public static class LogOutput
    {
        public static void DisplayLogs(List<LogEntry> logs)
        {
            ConsoleTableHelper<LogEntry>.PrintTable(logs);
        }
    }
}
