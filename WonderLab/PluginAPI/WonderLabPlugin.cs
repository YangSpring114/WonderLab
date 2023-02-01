using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.PluginAPI
{
    public abstract class WonderLabPlugin : PluginLoader.Plugin
    {
        public abstract void OnPluginLoad();
        public abstract void OnPluginUnLoad();
        public abstract void OnSettingButtonClick();
    }
}
