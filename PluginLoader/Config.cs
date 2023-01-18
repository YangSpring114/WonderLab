using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace PluginLoader
{
    public class Config<T>
    {
        public readonly ConfigManager ConfigManager;
        public T this[string Key]
        {
            get { return Get<T>(Key); }
            set { Set<T>(Key, value); }
        }
        public Config(ConfigManager configManager)
        {
            ConfigManager = configManager;
        }
        public Config(Plugin plugin)
        {
            ConfigManager = new ConfigManager(plugin);
        }
        public Config(string path)
        {
            ConfigManager = new ConfigManager(path);
        }
        public void SaveConfig()
        {
            ConfigManager.SaveConfig();
        }
        public T Get<T>(string Key)
        {
           return (T)ConfigManager.GetObject(Key);
        }
        public void Set<T>(string Key,T Value)
        {
            if(Value == null)
            {
                return;
            }
            ConfigManager.SetObject(Key, Value);
        }
        public ConfigManager GetConfigManager()
        {
            return ConfigManager;
        }
    }
    public class Config : IDictionary<string, object>
    {
        public readonly ConfigManager ConfigManager;

        public ICollection<string> Keys
        {
            get { return (ICollection<string>)ConfigManager.GetConfigDictionary();}
        }

        public ICollection<object> Values
        {
            get 
            {
                return ConfigManager.GetConfigDictionary().Values;
            }
        }

        public int Count 
        {
            get { return ConfigManager.GetConfigDictionary().Count;}
        }

        public bool IsReadOnly 
        {
            get { return false;}
        }

        public object this[string Key]
        {
            get { return Get<object>(Key); }
            set { Set(Key, value); }
        }
        public Config(ConfigManager configManager)
        {
            ConfigManager = configManager;
        }
        public Config(Plugin plugin)
        {
            ConfigManager = new ConfigManager(plugin);
        }
        public Config(string path)
        {
            ConfigManager = new ConfigManager(path);
        }
        public void SaveConfig()
        {
            ConfigManager.SaveConfig();
        }
        public T Get<T>(string Key)
        {
            return (T)ConfigManager.GetObject(Key);
        }
        public void Set<T>(string Key, T Value)
        {
            if (Value == null)
            {
                return;
            }
            ConfigManager.SetObject(Key, Value);
        }
        public ConfigManager GetConfigManager()
        {
            return ConfigManager;
        }

        public void Add(string key, object value)
        {
            ConfigManager.SetObject(key, value);
        }

        public bool ContainsKey(string key)
        {
            return ConfigManager.GetConfigDictionary().ContainsKey(key);
        }

        public bool Remove(string key)
        {
            Dictionary<string, object> tmp = ConfigManager.GetConfigDictionary();
            bool ret =  tmp.Remove(key);
            ConfigManager.SetDictionary(tmp);
            return ret;
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
        {
            return ConfigManager.GetConfigDictionary().TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this[item.Key] = item.Value;
        }

        public void Clear()
        {
            ConfigManager.SetDictionary(new Dictionary<string, object>());
        }

        public bool Contains(KeyValuePair<string, object> item) 
        {
            return ConfigManager.GetConfigDictionary().Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            int c = array.Length - arrayIndex;
            Dictionary<string, object> tmp = ConfigManager.GetConfigDictionary();
            if (c < tmp.Count)
            {
                return;
            }
            else
            {
                KeyValuePair<string,object>[] tmp2 = tmp.ToArray();
                int Count = 0;
                for(int i = arrayIndex; i < array.Length; i++)
                {
                    array[i] = tmp2[Count];
                    Count++;
                } 
            }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            Dictionary<string, object> tmp = ConfigManager.GetConfigDictionary();
            bool ret = tmp.Remove(item.Key);
            ConfigManager.SetDictionary(tmp);
            return ret;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ConfigManager.GetConfigDictionary().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ConfigManager.GetConfigDictionary().GetEnumerator();
        }
    }
}
