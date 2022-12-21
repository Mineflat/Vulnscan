using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot
{
    internal class ConfigurationItem
    {
        public string? Name;
        public string? Content;

        public ConfigurationItem(string? name, string? content)
        {
            Name = name;
            Content = content;
        }
    }
}
