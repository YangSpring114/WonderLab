namespace PluginLoader
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandle : Attribute
    {
        public bool IgnoreCancelled = false;
        public EventHandle(bool IgnoreCancelled = false)
        {
            this.IgnoreCancelled = IgnoreCancelled;
        }
    }
}
