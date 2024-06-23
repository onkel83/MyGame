using System;
using Core.Enums;

namespace Core.Model
{
    public class LogEntry : BaseModel
    {
        public DateTime LogTime { get; set; } = DateTime.Now;
        public string Message { get; set; } = string.Empty;
        public LogLevel Level { get; set; } = LogLevel.Dbug;
        public string Caller { get; set; } = string.Empty;
        public int LineNumber { get; set; } = 0;

        public override string ToString()
        {
            return $"{LogTime:HH:mm dd.MM.yyyy} [{Level}] {Message} (Caller: {Caller}, Line: {LineNumber})";
        }
    }
}