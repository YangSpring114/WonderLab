namespace PluginLoader
{
    public class PluginLoadEvent : Event
    {
        public override string Name { get { return "PluginUnLoadEvent"; } }
        public PluginInfo PluginInfo { get; set; }
        public override void Do()
        {
        }
    }
}
