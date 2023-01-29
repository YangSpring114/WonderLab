namespace PluginLoader
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandler : Attribute
    {
        public bool IgnoreCancelled = false;
        public EventHandler(bool IgnoreCancelled = false)
        {
            this.IgnoreCancelled = IgnoreCancelled;
        }
    }
}

