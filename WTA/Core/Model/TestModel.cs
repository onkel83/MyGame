using System;

namespace Core.Model
{
    public class TestModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime Start { get; set; }
        public DateTime Ende { get; set; }

        public decimal Pause { get; set; } = 0;

        public double WorkTime => (Ende - Start).TotalHours - (double)Pause;
    }
}
