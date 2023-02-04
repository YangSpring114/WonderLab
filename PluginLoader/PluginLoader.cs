using System.Reflection;
using System.Resources;
using static PathLib.PathLib;
namespace PluginLoader
{
    /// <summary>
    /// 插件加载器
    /// </summary>
    public static class PluginLoader
    {
        /// <summary>
        /// 插件默认路径
        /// </summary>
        public static string PluginPath = getSubPath(Environment.CurrentDirectory , "Plugins");
        /// <summary>
        /// 全局插件列表
        /// </summary>
        public static List<PluginInfo> PluginInfos = new List<PluginInfo>();
        /// <summary>
        /// 全局插件类
        /// </summary>
        public static List<Plugin> Plugins = new List<Plugin>();
        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <param name="type">
        /// 需要获取的插件主类
        /// </param>
        /// <returns>
        /// 插件主类对应的插件信息
        /// </returns>
        public static PluginInfo? GetPluginInfo(Type type)
        {
            Attribute? attribute = Attribute.GetCustomAttribute(type, typeof(PluginHandler));
            PluginHandler handler;
            if (attribute != null)
            {
                handler = (PluginHandler)attribute;
            }
            else { return null; }
            if (handler != null)
            {
                PluginInfo info = new PluginInfo();
                info.Name = handler.Name;
                info.Description = handler.Description;
                info.Version = handler.Version;
                info.Guid = handler.Guid;
                info.MainType = GetMainPluginType(type.Assembly.Location);
                info.Path = type.Assembly.Location;
                info.Icon = handler.Icon;
                return info;
            }
            return null;

        }
        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <param name="Plugin">插件实例</param>
        /// <returns>插件信息</returns>
        public static PluginInfo? GetPluginInfo(Plugin Plugin)
        {
            Type type = Plugin.GetType();
            Attribute? attribute = Attribute.GetCustomAttribute(type, typeof(PluginHandler));
            PluginHandler handler;
            if (attribute != null)
            {
                handler = (PluginHandler)attribute;
            }
            else { return null; }
            if (handler != null)
            {
                PluginInfo info = new PluginInfo();
                info.Name = handler.Name;
                info.Description = handler.Description;
                info.Version = handler.Version;
                info.Guid = handler.Guid;
                info.MainType = GetMainPluginType(type.Assembly.Location);
                info.Path = type.Assembly.Location;
                info.Icon = handler.Icon;
                info.Author = handler.Author;
                return info;
            }
            return null;

        }
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="Path">
        /// 插件文件路径
        /// </param>
        public static void Load(string Path)
        {
            Type type = GetMainPluginType(Path);
            Type[] types = GetPluginTypes(Path);
            PluginInfo? plugin = ((Plugin)Activator.CreateInstance(type, Array.Empty<object>())).GetPluginInfo();
            if (plugin == null)
            {
                return;
            }
            foreach (PluginInfo i in PluginInfos)
            {
                if (i.Guid == plugin.Guid || i.Name == plugin.Name)
                {
                    return;
                }
            }
            foreach (Type t in types)
            {
                Attribute[] att = t.GetCustomAttributes().ToArray();
                foreach (Attribute attr in att)
                {
                    if (attr is ListenerHandler)
                    {
                        var tmp = Activator.CreateInstance(t, new object[] { });
                        if (tmp != null)
                        {
                            ((Listener)tmp).Register();
                        }
                    }
                    if (attr is RunnableHandler)
                    {
                        var tmp = Activator.CreateInstance(t, new object[] { });
                        if (tmp != null)
                        {
                            ((Runnable)tmp).Register();
                        }
                    }
                }
            }
            if (type != null)
            {
                try
                {
                    MethodInfo method = type.GetMethod("OnPluginLoad");
                    object obj = Activator.CreateInstance(type);
                    method.Invoke(obj, new object[] { });
                    PluginInfo pluginInfo = GetPluginInfo(type);
                    pluginInfo.Path = Path;
                    pluginInfo.MainType = type;
                    PluginInfos.Add(pluginInfo);
                    Plugins.Add((Plugin)obj);
                    PluginLoadEvent e = new PluginLoadEvent();
                    e.PluginInfo = pluginInfo;
                    Event.CallEvent(e);
                }
                catch { }
            }

        }
        /// <summary>
        /// 通关插件名获取插件实例
        /// </summary>
        /// <param name="pluginName">插件名</param>
        /// <returns>插件实例</returns>
        public static Plugin? GetPlugin(string pluginName)
        {
            foreach (Plugin plugin in Plugins)
            {
                if (plugin.GetPluginInfo().Name == pluginName)
                {
                    return plugin;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取所有加载的插件
        /// </summary>
        /// <returns>加载的插件</returns>
        public static Plugin[] GetPlugins()
        {
            return Plugins.ToArray();
        }
        /// <summary>
        /// 获取插件主类
        /// </summary>
        /// <param name="Path">插件路径</param>
        /// <returns>主类</returns>
        public static Type GetMainPluginType(string Path)
        {
            Assembly dllFromPlugin = Assembly.LoadFile(Path);
            bool IsPlugin = false;
            string MainClassLocation = null;
            try
            {
                string name = System.IO.Path.GetDirectoryName(Path);
                ResourceManager resourceManager = new ResourceManager(name + ".Properties.Resources", dllFromPlugin);
                MainClassLocation = resourceManager.GetString("MainClass")!;
            }
            catch { }
            if (MainClassLocation != null)
            {
                IsPlugin = true;
            }
            else
            {
                foreach (Type t in dllFromPlugin.GetTypes())
                {
                    Attribute[] att = t.GetCustomAttributes().ToArray();
                    foreach (Attribute attr in att)
                    {
                        if (attr is PluginHandler)
                        {
                            IsPlugin = true;
                            MainClassLocation = t.FullName;
                            break;
                        }
                    }
                }
            }
            if (!IsPlugin)
            {
                return null;
            }
            Type type = dllFromPlugin.GetType(MainClassLocation);
            return type;
        }
        public static Type[] GetPluginTypes(string Path)
        {
            Assembly dllFromPlugin = Assembly.LoadFile(Path);
            return dllFromPlugin.GetTypes();
        }
        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="Path">
        /// 已加载插件路径
        /// </param>
        public static void UnLoad(string Path)
        {
            for (int i = 0; i < PluginInfos.Count; i++)
            {
                if (PluginInfos[i].Path == Path)
                {
                    Type type = PluginInfos[i].MainType;
                    if (type != null)
                    {
                        try
                        {
                            MethodInfo method = type.GetMethod("OnPluginUnLoad");
                            object obj = GetPlugin(PluginInfos[i].Name);
                            method.Invoke(obj, new object[] { });
                        }
                        catch (Exception) { }
                    }
                    PluginUnLoadEvent e = new PluginUnLoadEvent();
                    e.PluginInfo = PluginInfos[i];
                    Event.CallEvent(e);
                    for (int j = 0; j < Event.Listeners.Count; j++)
                    {
                        if (Event.Listeners[j].PluginInfo.Guid == PluginInfos[i].Guid)
                        {

                            Event.Listeners.RemoveAt(j);
                            j--;
                        }
                    }
                    for (int j = 0; j < Runnable.Runnables.Count; j++)
                    {
                        if (Runnable.Runnables[j].PluginInfo.Guid == PluginInfos[i].Guid)
                        {
                            Runnable.Runnables[j].Stop();
                            Runnable.Runnables.RemoveAt(j);
                            j--;
                        }
                    }
                    Plugins.Remove(GetPlugin(PluginInfos[i].Name));
                    PluginInfos.RemoveAt(i); return;

                }
            }
        }
        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="plugin">插件实例</param>
        public static void UnLoad(Plugin plugin)
        {
            for (int i = 0; i < PluginInfos.Count; i++)
            {
                if (PluginInfos[i].Guid == plugin.GetPluginInfo().Guid)
                {
                    Type type = PluginInfos[i].MainType;
                    if (type != null)
                    {
                        try
                        {
                            MethodInfo method = type.GetMethod("OnPluginUnLoad");
                            method.Invoke(plugin, new object[] { });
                        }
                        catch (Exception) { }
                    }
                    PluginUnLoadEvent e = new PluginUnLoadEvent();
                    e.PluginInfo = PluginInfos[i];
                    Event.CallEvent(e);
                    for (int j = 0; j < Event.Listeners.Count; j++)
                    {
                        if (Event.Listeners[j].PluginInfo.Guid == PluginInfos[i].Guid)
                        {
                            Event.Listeners.RemoveAt(j);
                            j--;
                        }
                    }
                    for (int j = 0; j < Runnable.Runnables.Count; j++)
                    {
                        if (Runnable.Runnables[j].PluginInfo.Guid == PluginInfos[i].Guid)
                        {
                            Runnable.Runnables[j].Stop();
                            Runnable.Runnables.RemoveAt(j);
                            j--;
                        }
                    }
                    Plugins.Remove(plugin);
                    PluginInfos.RemoveAt(i);
                    return;
                }
            }
        }
        public static void LoadAllFromPluginDir()
        {
            DirectoryInfo dir = new DirectoryInfo(PluginPath);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                Load(file.FullName);
            }
        }
        public static void LoadAllFromPlugin()
        {
            ConfigManager configManager = new ConfigManager(getSubPath(PluginPath, "Plugins.json"));
            DirectoryInfo dir = new DirectoryInfo(PluginPath);
            foreach (DirectoryInfo PluginDir in dir.GetDirectories())
            {
                if (File.Exists(getSubPath(PluginDir.FullName, "Plugin.dll")))
                {
                    bool isEnable = true;
                    try
                    {
                        isEnable = configManager.GetBool(((Plugin)Activator.CreateInstance(GetMainPluginType(getSubPath(PluginDir.FullName, "Plugin.dll")), Array.Empty<object>())).GetPluginInfo().Guid);
                    }
                    catch { }
                    if (isEnable)
                    {
                        Load(getSubPath(PluginDir.FullName, "Plugin.dll"));
                    }
                }
            }
        }
        /// <summary>
        /// 禁用插件
        /// </summary>
        /// <param name="PluginGuid">插件Guid</param>
        public static void SetDisable(string PluginGuid)
        {
            ConfigManager configManager = new ConfigManager(getSubPath(PluginPath, "Plugins.json"));
            configManager.SetBool(PluginGuid, false);
            configManager.SaveConfig();
        }
        /// <summary>
        /// 启用插件
        /// </summary>
        /// <param name="PluginGuid">插件Guid</param>
        public static void SetEnable(string PluginGuid)
        {
            ConfigManager configManager = new ConfigManager(getSubPath(PluginPath, "Plugins.json"));
            configManager.SetBool(PluginGuid, false);
            configManager.SaveConfig();
        }
        /// <summary>
        /// 禁用插件
        /// </summary>
        /// <param name="plugin">插件类</param>
        public static void SetDisable(Plugin plugin)
        {
            ConfigManager configManager = new ConfigManager(getSubPath(PluginPath, "Plugins.json"));
            configManager.SetBool(plugin.GetPluginInfo().Guid, false);
            configManager.SaveConfig();
        }
        /// <summary>
        /// 启用插件
        /// </summary>
        /// <param name="plugin">插件类</param>
        public static void SetEnable(Plugin plugin)
        {
            ConfigManager configManager = new ConfigManager(getSubPath(PluginPath, "Plugins.json"));
            configManager.SetBool(plugin.GetPluginInfo().Guid, false);
            configManager.SaveConfig();
        }
        /// <summary>
        /// 卸载所有插件
        /// </summary>
        public static void UnloadAll()
        {
            foreach (Plugin plugin in Plugins.ToArray())
            {
                UnLoad(plugin);
            }
        }
    }
}
