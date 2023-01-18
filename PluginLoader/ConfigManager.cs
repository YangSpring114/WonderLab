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
                return Config[((PluginInfo)Plugin).Guid][Key];
            }
            set 
            { 
                Config[((PluginInfo)Plugin).Guid][Key] = value;
            } 
        }
        public Plugin Plugin { get; set; }
        public ConfigManager(Plugin plugin)
        {
            Plugin = plugin;
            string tmp = getSubPath(PluginLoader.PluginPath, ((PluginInfo)Plugin).Name);
            FilePath = getSubPath(tmp, "Config.json");
            if (!Config.ContainsKey(((PluginInfo)Plugin).Guid))
            {
                LoadConfig();
            }
        }
        public ConfigManager(string ConfigFilePath)
        {
            Plugin = new EmptyPlugin(ConfigFilePath);
            FilePath = ConfigFilePath;
            if (!Config.ContainsKey(((PluginInfo)Plugin).Guid))
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
            string json = JsonConvert.SerializeObject(Config[((PluginInfo)Plugin).Guid]);
            File.WriteAllText(FilePath, json);
        }
        public void LoadConfig()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(getSubPath(PluginLoader.PluginPath, ((PluginInfo)Plugin).Name));
                File.Create(FilePath).Close();
            }
            configData = File.ReadAllText(FilePath);
            Dictionary<string,object>? tmp = JsonConvert.DeserializeObject<Dictionary<string, object>>(configData);

            if (tmp == null)
            {
                Config[((PluginInfo)Plugin).Guid] = new Dictionary<string, object>();
            }
            else { Config[((PluginInfo)Plugin).Guid] = tmp;}
        }
        #region Get
        public string GetString(string Key)
        {
            return (string)Config[((PluginInfo)Plugin).Guid][Key];
        }
        public string[] GetStringArray(string Key)
        {
            return (string[])Config[((PluginInfo)Plugin).Guid][Key];
        }
        public int GetInt32(string Key)
        {
            return (int)Config[((PluginInfo)Plugin).Guid][Key];
        }
        public int[] GetInt32Array(string Key)
        {
            return (int[])Config[((PluginInfo)Plugin).Guid][Key];
        }
        public long GetLong(string Key)
        {
            return (long)Config[((PluginInfo)Plugin).Guid][Key];
        }
        public long[] GetLongArray(string Key)
        {
            return (long[])Config[((PluginInfo)Plugin).Guid][Key];
        }
        public object GetObject(string Key)
        {
            return (object)Config[((PluginInfo)Plugin).Guid][Key];
        }
        public object[] GetObjectArray(string Key)
        {
            return (object[])Config[((PluginInfo)Plugin).Guid][Key];
        }
        public bool GetBool(string Key)
        {
            return (bool)Config[((PluginInfo)Plugin).Guid][Key];
        }
        public bool[] GetBoolArray(string Key)
        {
            return (bool[])Config[((PluginInfo)Plugin).Guid][Key];
        }
        public Dictionary<string,object> GetConfigDictionary()
        {
            return Config[((PluginInfo)Plugin).Guid];
        }
        #endregion
        #region Set
        public void SetString(string Key,string Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetStringArray(string Key, string[] Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetInt32(string Key, int Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetIntArray(string Key, int[] Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetLong(string Key, long Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetLongArray(string Key, long[] Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetObject(string Key, object Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetObjectArray(string Key, object[] Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetBool(string Key,bool Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetBoolArray(string Key, bool[] Value)
        {
            Config[((PluginInfo)Plugin).Guid][Key] = Value;
        }
        public void SetDictionary(Dictionary<string,object> Value)
        {
            Config[((PluginInfo)Plugin).Guid] = Value;
        }
        #endregion


    }
}
