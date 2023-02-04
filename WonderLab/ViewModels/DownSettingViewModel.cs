using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Installer;
using MinecraftLaunch.Modules.Models.Download;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    ///Binding
    public partial class DownSettingViewModel : ViewModelBase
    {
        private string _DownloadPath = App.Data.CustomDownloadPath;
        public string DownloadPath
        {
            get {
                if (Directory.Exists(_DownloadPath))
                {
                    return _DownloadPath;
                }
                else
                {
                    App.Data.CustomDownloadPath = Environment.CurrentDirectory;
                    return Environment.CurrentDirectory;
                }
            }
            set
            {
                RaiseAndSetIfChanged(ref _DownloadPath, value);
            }
        }

        public List<string> DownloadAPI => new() { "MCBBS (国内镜像源，速度最快)", "BmclAPI (国内镜像源，速度适中)", "Mojang (官方源，速度最慢)" };

        public List<string> Branchs => new() { "Lsaac (预览版，速度最快)", "Albert (稳定版，稍慢，稳定性高)" };

        public int SelectDownloadAPI
        {
            get => _SelectDownloadAPI;
            set
            {
                if (value is not -1 && RaiseAndSetIfChanged(ref _SelectDownloadAPI, value))
                    switch (value)
                    {
                        case 0:
                            APIManager.Current = APIManager.Mcbbs;
                            break;
                        case 1:
                            APIManager.Current = APIManager.Bmcl;
                            break;
                        case 2:
                            APIManager.Current = APIManager.Mojang;
                            break;
                        default:
                            APIManager.Current = APIManager.Mcbbs;
                            break;
                    }
            }
        }
        

        public int CurrentBranch
        {
            get => _CurrentBranch;
            set
            {
                if (value is not -1 && RaiseAndSetIfChanged(ref _CurrentBranch, value)) {
                    App.Data.CurrentBranch = value; 
                }
            }
        }

        public int Max
        {
            get => _Max;
            set
            {
                if (RaiseAndSetIfChanged(ref _Max, value))
                {
                    App.Data.MaxThreadCount = value;
                    ResourceInstaller.MaxDownloadThreads = value;
                }
            }
        }
    }
    //Other
    partial class DownSettingViewModel
    {        
        public int _SelectDownloadAPI = 0;
        public int _CurrentBranch = App.Data.CurrentBranch;
        public int _Max = App.Data.MaxThreadCount is 0 ? 128 : App.Data.MaxThreadCount;
    }
    //Method
    partial class DownSettingViewModel
    {            
        public DownSettingViewModel() => SelectDownloadAPI = App.Data.SelectedAPI;
    }
}
