using PluginLoader;
using WonderLab.PluginAPI;

namespace ExamplePlugin
{
    [PluginHandle("ExamplePlugin", null, "1.0", "{FDE24F4B-3E3F-4580-A087-CBFDF1781B16}")]
    public class Example : Plugin
    {
        public override void onPluginLoad()
        {
            
        }

        public override void onPluginUnLoad()
        {
            
        }
    }
    [ListenerHandle]
    public class Listener1 : Listener
    {
        public Listener1()
        {
            PluginInfo = PluginLoader.PluginLoader.GetPluginInfo(typeof(Example));
        }
        public override PluginInfo PluginInfo { get; set; }
        [EventHandle]
        public void OnGameLaunchEvent(GameLaunchAsyncEvent e)
        {
            ((ICancellable)e).Cancel();
        }
    }
}