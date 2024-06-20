using Core.Model;

namespace WTA_Test
{
    public class TestModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime Start { get; set; }
        public DateTime Ende { get; set; }

        public decimal Pause { get; set; } = 0;

        public double WorkTime { get => (Ende - Start).TotalHours - ((double)Pause); }
    }
}
