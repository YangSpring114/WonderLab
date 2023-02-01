using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    public partial class DownViewModel : ViewModelBase
    {
        private string _DownloadUrl = "下载链接";
        public string DownloadUrl
        {
            get => _DownloadUrl;
            set { RaiseAndSetIfChanged(ref _DownloadUrl, value); }
        }
        private string _FileName = "保存的文件名";
        public string FileName
        {
            get => _FileName;
            set { RaiseAndSetIfChanged(ref _FileName, value); }
        }
        public void NavigationToDownGameCore()
        {            
            _ = MainView.mv.FrameView.Navigate(typeof(DownGameView));
        }

        public void NavigationToDownMod()
        {
            _ = MainView.mv.FrameView.Navigate(typeof(DownModView));
        }

        public void NavigationToDown()
        {
            _ = MainView.mv.FrameView.Navigate(typeof(DownCustomView));
        }
    }

    partial class DownViewModel
    {
        public static Dictionary<string, KeyValuePair<string, string>[]> OpenJdkDownloadSources => new()
        {
            {
                "OpenJDK 8",
                new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net","https://download.java.net/openjdk/jdk8u42/ri/openjdk-8u42-b03-windows-i586-14_jul_2022.zip")
                }
            },
            {
                "OpenJDK 11", new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net", "https://download.java.net/openjdk/jdk11/ri/openjdk-11+28_windows-x64_bin.zip"),
                    new ("Microsoft", "https://aka.ms/download-jdk/microsoft-jdk-11.0.16-windows-x64.zip")
                }
            },
            {
                "OpenJDK 17", new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net", "https://download.java.net/openjdk/jdk17/ri/openjdk-17+35_windows-x64_bin.zip"),
                    new ("Microsoft", "https://aka.ms/download-jdk/microsoft-jdk-17.0.4-windows-x64.zip")
                }
            },
            {
                "OpenJDK 18", new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net", "https://download.java.net/openjdk/jdk18/ri/openjdk-18+36_windows-x64_bin.zip")
                }
            }
        };
    }

    partial class DownViewModel
    {
        public KeyValuePair<string, string>[] _Urls;
        public KeyValuePair<string, string>? _CurrentUrl;
        public string _CurrentDownloadSource = string.Empty;
        public bool _ConfirmEnabled;
    }

    partial class DownViewModel
    {
        public Dictionary<string, KeyValuePair<string, string>[]> DownloadSources { get; set; } = OpenJdkDownloadSources;
        public KeyValuePair<string, string>[] Urls { get => _Urls; set => RaiseAndSetIfChanged(ref _Urls, value); }
        public KeyValuePair<string, string>? CurrentUrl { get => _CurrentUrl; set { RaiseAndSetIfChanged(ref _CurrentUrl, value); ConfirmEnabled = CurrentUrl != null; } }
        public string CurrentDownloadSource { get => _CurrentDownloadSource; set { if (RaiseAndSetIfChanged(ref _CurrentDownloadSource, value)) { Urls = null; Urls = DownloadSources[CurrentDownloadSource]; } } }
        public bool ConfirmEnabled { get => _ConfirmEnabled; set => RaiseAndSetIfChanged(ref _ConfirmEnabled, value); }
    }
}
// set => RaiseAndSetIfChanged(ref _CurrentDownloadSource, value);e);