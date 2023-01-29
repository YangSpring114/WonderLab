namespace PluginLoader
{
    /// <summary>
    /// 插件头
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginHandler : Attribute
    {
        /// <summary>
        /// 插件Guid
        /// </summary>
        public string Guid { get; }

        public PluginHandler(string Name, string? Description, string Version, string Guid, string? Icon = null)
        {
            this.Icon = Icon;
            this.Name = Name;
            this.Description = Description;
            this.Version = Version;
            if (!util.IsGuidByReg(Guid))
            {
                throw new GuidExpection();
            }
            this.Guid = Guid;
        }
        /// <summary>
        /// 插件名
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 插件描述
        /// </summary>
        public string? Description { get; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public string Version { get; }
        /// <summary>
        /// 插件图标
        /// </summary>
        public string? Icon { get; }
    }
}