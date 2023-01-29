using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static PathLib.PathLib;

namespace PluginLoader
{
    public class ConfigManager
    {
        private static Dictionary<string,Dictionary<string, object>> Config = new Dictionary<string, Dictionary<string, object>>();
        public string? configData;
        public string FilePath;
        public object this[string Key]
        {
            get
            {
                return Config[Plugin.GetPluginInfo().Guid][Key];
            }
            set 
            { 
                Config[Plugin.GetPluginInfo().Guid][Key] = value;
            } 
        }
        public Plugin Plugin { get; set; }
        public ConfigManager(Plugin plugin)
        {
            Plugin = plugin;
            string tmp = getSubPath(PluginLoader.PluginPath, Plugin.GetPluginInfo().Name);
            FilePath = getSubPath(tmp, "Config.json");
            if (!Config.ContainsKey(Plugin.GetPluginInfo().Guid))
            {
                LoadConfig();
            }
        }
        public ConfigManager(string ConfigFilePath)
        {
            Plugin = new EmptyPlugin(ConfigFilePath);
            FilePath = ConfigFilePath;
            if (!Config.ContainsKey(Plugin.GetPluginInfo().Guid))
            {
                LoadConfig();
            }
        }
        ~ConfigManager()
        {
            SaveConfig();
        }
        public void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(Config[Plugin.GetPluginInfo().Guid]);
            File.WriteAllText(FilePath, json);
        }
        public void LoadConfig()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(getSubPath(PluginLoader.PluginPath, Plugin.GetPluginInfo().Name));
                File.Create(FilePath).Close();
            }
            configData = File.ReadAllText(FilePath);
            Dictionary<string,object>? tmp = JsonConvert.DeserializeObject<Dictionary<string, object>>(configData);

            if (tmp == null)
            {
                Config[Plugin.GetPluginInfo().Guid] = new Dictionary<string, object>();
            }
            else { Config[Plugin.GetPluginInfo().Guid] = tmp;}
        }
        #region Get
        public string GetString(string Key)
        {
            return (string)Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public string[] GetStringArray(string Key)
        {
            return (string[])Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public int GetInt32(string Key)
        {
            return (int)Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public int[] GetInt32Array(string Key)
        {
            return (int[])Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public long GetLong(string Key)
        {
            return (long)Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public long[] GetLongArray(string Key)
        {
            return (long[])Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public object GetObject(string Key)
        {
            return Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public object[] GetObjectArray(string Key)
        {
            return (object[])Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public bool GetBool(string Key)
        {
            return (bool)Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public bool[] GetBoolArray(string Key)
        {
            return (bool[])Config[Plugin.GetPluginInfo().Guid][Key];
        }
        public Dictionary<string,object> GetConfigDictionary()
        {
            return Config[Plugin.GetPluginInfo().Guid];
        }
        #endregion
        #region Set
        public void SetString(string Key,string Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetStringArray(string Key, string[] Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetInt32(string Key, int Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetIntArray(string Key, int[] Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetLong(string Key, long Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetLongArray(string Key, long[] Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetObject(string Key, object Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetObjectArray(string Key, object[] Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetBool(string Key,bool Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetBoolArray(string Key, bool[] Value)
        {
            Config[Plugin.GetPluginInfo().Guid][Key] = Value;
        }
        public void SetDictionary(Dictionary<string,object> Value)
        {
            Config[Plugin.GetPluginInfo().Guid] = Value;
        }
        #endregion


    }
}
