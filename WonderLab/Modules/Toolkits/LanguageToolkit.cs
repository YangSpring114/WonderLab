using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 语言工具类
    /// </summary>
    public class LanguageToolkit
    {
        /// <summary>
        /// 语言切换方法
        /// </summary>
        /// <param name="tag"></param>
        public static void LanguageChange(string tag)
        {
            var news = (ResourceDictionary)AvaloniaXamlLoader.Load(new($"{LanguageDir}{tag}.axaml"));
            var old = (ResourceDictionary)AvaloniaXamlLoader.Load(new($"{LanguageDir}{App.Data.LanguageTag}.axaml"));
            Application.Current!.Resources.MergedDictionaries.Remove(old);
            Application.Current!.Resources.MergedDictionaries.Add(news);
        }

        /// <summary>
        /// 获取指定键值对的文本信息
        /// </summary>
        public static string GetText(string key) {
            object temp = string.Empty;
            if (CurrentLanguage.TryGetValue(key, out temp)) {
                return CurrentLanguage[key]?.ToString() ?? "Not Found";
            }
            
            return "Not Found";
        }
        
        public const string LanguageDir = "avares://WonderLab/Resources/Strings/";
        public static ResourceDictionary CurrentLanguage { get; set; } = (ResourceDictionary)AvaloniaXamlLoader.Load(new($"{LanguageDir}{App.Data.LanguageTag}.axaml"));
    }
}
