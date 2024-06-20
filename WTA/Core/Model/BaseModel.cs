using System;
namespace Core.Model
{
    public abstract class BaseModel
    {
        private int _Id = 0;
        public string ID
        {
            get => _Id.ToString("D6");
            set => _Id = Convert.ToInt32(value);
        }
    }
}
