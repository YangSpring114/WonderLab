using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLoader
{
    [PluginHandler("EmptyPlugin","一个用于标识的插件类","0", "{3406A8A8-D4DB-4E32-9DEC-2ED5349A75C4}")]
    public class EmptyPlugin : Plugin
    {
        public string? Flag { get; set; }
        public EmptyPlugin(string? Flag)
        {
            this.Flag = Flag;
        }

        public void OnPluginLoad()
        {

        }

        public void OnPluginUnLoad()
        {

        }
    }
}
