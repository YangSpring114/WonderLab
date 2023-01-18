using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLoader
{
    public abstract class Runnable
    {
        public static List<Runnable> Runnables = new List<Runnable>();
        Thread? thread;
        public PluginInfo PluginInfo { get; set; }
        public void Register()
        {
            if(PluginInfo != null)
            {
                Start();
                Runnables.Add(this);
            }
        }
        public virtual void Run() { }
        public void Start()
        {
            thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }
        public void Stop()
        {
            if(thread != null) { thread.Abort(); }
        }
    }
}
