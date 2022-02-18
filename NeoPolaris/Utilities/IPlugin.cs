using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoPolaris.Utilities
{
    public interface IPlugin
    {
        public string Name { get; }
        public string Author { get; }
        public string Version { get; }
    }
}
