namespace PluginLoader
{
    /// <summary>
    /// 表示一个插件的信息
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// 插件图标
        /// </summary>
        public string? Icon;
        /// <summary>
        /// 插件Guid
        /// </summary>
        public string Guid;
        /// <summary>
        /// 插件名
        /// </summary>
        public string Name;
        /// <summary>
        /// 插件描述
        /// </summary>
        public string? Description;
        /// <summary>
        /// 插件版本
        /// </summary>
        public string? Version;
        /// <summary>
        /// 插件路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 插件主类
        /// </summary>
        public Type MainType;
        public PluginInfo()
        {

        }
    }
}
