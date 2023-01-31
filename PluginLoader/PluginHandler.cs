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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">插件名</param>
        /// <param name="Description">插件描述</param>
        /// <param name="Version">版本</param>
        /// <param name="Guid">guid</param>
        /// <param name="Icon">图标</param>
        /// <exception cref="GuidExpection">guid不正常</exception>
        public PluginHandler(string Name, string? Description, string Version, string Guid,string Author = "未著名", string? Icon = null)
        {
            this.Icon = Icon;
            this.Name = Name;
            this.Description = Description;
            this.Version = Version;
            this.Author = Author;
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
        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; }
    }
}