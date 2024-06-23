using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class ConfigModel : BaseModel
    {
        private string _ConfigFile = string.Empty;
        private string _Key = string.Empty;
        private string _Value = string.Empty;

        public string Config { get => _ConfigFile; set => _ConfigFile = value; }
        public string Key { get => _Key; set => _Key = value; }
        public string Value { get => _Value; set => _Value = value; }
    }
}
