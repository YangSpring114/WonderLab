using MinecraftLaunch.Modules.Installer;
using MinecraftLaunch.Modules.Models.Download;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;

namespace WonderLab.ViewModels
{
    ///Binding
    public partial class DownSettingViewModel : ViewModelBase
    {
        public List<string> DownloadAPI => new() { "MCBBS (国内镜像源，速度最快)", "BmclAPI (国内镜像源，速度适中)", "Mojang (官方源，速度最慢)" };

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

        public int Max
        {
            get => _Max;
            set
            {
                if (RaiseAndSetIfChanged(ref _Max, value))
                {
                    App.Data.MaxThreadCount = Convert.ToInt32(value);
                    ResourceInstaller.MaxDownloadThreads = Convert.ToInt32(value);
                }
            }
        }
    }
    //Other
    partial class DownSettingViewModel
    {
        public int _SelectDownloadAPI = 0;
        public int _Max = App.Data.MaxThreadCount is 0 ? 128 : App.Data.MaxThreadCount;
    }
    //Method
    partial class DownSettingViewModel
    {            
        public DownSettingViewModel() => SelectDownloadAPI = App.Data.SelectedAPI;
    }
}
