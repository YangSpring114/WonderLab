using PluginLoader;
using WonderLab.PluginAPI;

namespace CustomValue
{
    [PluginHandler("CustomValue", null, "1.0", "{EDE94F4B-3E3F-4380-A0A7-CBFEF1781B16}")]
    public class CustomValuePlugin : Plugin
    {
        public ConfigManager Config;
        public static CustomValuePlugin Plugin;
        public void OnPluginLoad()
        {
            Config = new ConfigManager(this);
            try
            {
                if (Config.GetBool("Value"))
                {

                }
            }
            catch
            {
                Config.SetBool("Value", false);
            }
            Plugin = this;
        }
        public void OnPluginUnLoad()
        {
            Config.SaveConfig();
        }
    }
    public class GetValueEvent : Event
    {
        public override string Name
        {
            get
            {
                return "GetValueEvent";
            }
        }

        public override bool Do()
        {
            return CustomValuePlugin.Plugin.Config.GetBool("Value");
        }
    }
}