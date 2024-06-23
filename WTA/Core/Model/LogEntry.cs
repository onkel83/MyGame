using Core.Helper;
using System;

namespace Core.Model
{
    public class LogEntry : BaseModel
    {
        public DateTime LogTime { get; set; } = DateTime.Now;
        public string Message { get; set; } = string.Empty;
        public LogLevel Level { get; set; } = LogLevel.Dbug;
        public string Caller { get; set; } = string.Empty;
        public int LineNumber { get; set; } = 0;
    }
}
