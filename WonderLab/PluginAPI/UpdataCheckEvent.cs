using PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.PluginAPI
{
    public class UpdataCheckEvent : Event
    {
        public override string Name { get { return "UpdataCheckEvent"; } }

        public override bool Do()
        {
            return true;
        }
    }
}
