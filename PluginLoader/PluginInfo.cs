namespace PluginLoader
{
    /// <summary>
    /// 表示一个插件的信息
    /// </summary>
    public class PluginInfo
    {
        public string Guid;
        public string Name;
        public string? Description;
        public static implicit operator PluginInfo(Plugin plugin)
        {
            PluginInfo pluginInfo = new PluginInfo();
            if (plugin is EmptyPlugin)
            {
                pluginInfo.Guid = ((EmptyPlugin)plugin).Flag;
            }
            else
            {
                pluginInfo = PluginLoader.GetPluginInfo(plugin);
            }
            return pluginInfo;
        }
        public string? Version;
        public string Path;
        public Type? MainType;
    }
}
