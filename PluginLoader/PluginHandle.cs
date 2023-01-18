namespace PluginLoader
{
    /// <summary>
    /// 插件头
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginHandle : Attribute
    {
        public readonly string Guid;

        public PluginHandle(string Name, string? Description, string Version, string Guid)
        {
            this.Name = Name;
            this.Description = Description;
            this.Version = Version;
            if (!util.IsGuidByReg(Guid))
            {
                throw new GuidExpection();
            }
            this.Guid = Guid;
        }

        public string Name { get; }
        public string? Description { get; }
        public string Version { get; }
    }
}
