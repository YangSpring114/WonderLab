using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLaunch.Modules.Models.Launch;
using Newtonsoft.Json;

namespace WonderLab.Modules.Models
{
    /// <summary>
    ///  数据保存模型
    /// </summary>
    public class DataModels
    {
        public int Max { get; set; } = 1024;
        public int SelectedJava { get; set; } = 0;
        public int SelectedLang { get; set; } = 0;
        public int SelectedAPI { get; set; } = 0;
        public int MaxThreadCount { get; set; } = 0;
        public string SelectedGameFooter { get; set; } = string.Empty;
        public string? SelectedGameCore { get; set; } = null;
        public UserDataModels? SelectedUser { get; set; } = null;
        public string FooterPath { get; set; } = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft")}";
        public string NativesFolderPath { get; set; } = string.Empty;
        public string JavaPath { get; set; } = string.Empty;
        public string Jvm { get; set; } = string.Empty;
        public string LanguageTag { get; set; } = "zh-cn";
        public bool IsFull { get; set; } = false;
        public bool Isolate { get; set; } = false;
        public bool AutoSelectJava { get; set; } = true;
        public List<string> JavaList { get; set; } = new();
        public List<string> GameFooterList { get; set; } = new() { Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft") };
        public List<UserDataModels> UserList { get; set; } = new();
    }    
}