using PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.PluginAPI
{
    public abstract class PluginSetting<TValue>
    {
        public string Key = string.Empty;
        private TValue? Value = default;
        public virtual TValue? GetValue()
        {
            return Value;
        }
        public virtual void SetValue(TValue value)
        {
            Value = value;
        }
    }
    public class BoolSetting : PluginSetting<bool>
    {
        public string TrueText;
        public string FalseText;
        public BoolSetting(string key, string trueText, string falseText, bool @default = true)
        {
            Key = key;
            TrueText = trueText;
            FalseText = falseText;
        }
        public void Register(Plugin plugin)
        {

        }
    }
}
