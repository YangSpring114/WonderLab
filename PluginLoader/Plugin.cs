namespace PluginLoader
{
    /// <summary>
    /// 插件基类
    /// </summary>
    public interface Plugin
    {
        /// <summary>
        /// 当插件加载时
        /// </summary>
        public void OnPluginLoad();
        /// <summary>
        /// 当插件卸载时
        /// </summary>
        public void OnPluginUnLoad();
    }
    public static class PluginExtra
    {
        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <param name="plugin">插件类</param>
        /// <returns>插件信息</returns>
        public static PluginInfo GetPluginInfo(this Plugin plugin)
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
    }
}
