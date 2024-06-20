using System;

namespace Core.Model
{
    public class LogEntry
    {
        public DateTime LogTime { get; set; } = DateTime.Now;
        public string Message { get; set; } = string.Empty;
        public Core.Enum.LogLevel Level { get; set; }
        public string Caller { get; set; } = string.Empty;
        public int LineNumber { get; set; } = 0;

        public override string ToString()
        {
            return $"{LogTime:HH:mm dd.MM.yyyy} [{Level}] {Message} (Caller: {Caller}, Line: {LineNumber})";
        }
    }
}
