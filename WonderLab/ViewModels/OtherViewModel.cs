using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;
using GithubLib;
using System.Net;
using System.IO;
using System.IO.Compression;
using WonderLab.Views;
using Natsurainko.Toolkits.Network.Model;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using static WonderLab.MainWindow;

namespace WonderLab.ViewModels
{
    public partial class OtherViewModel : ViewModelBase
    {
        public async Task Check()
        {
            try
            {
                await Task.Run(() =>
                {
                    string releaseUrl = GithubLib.GithubLib.GetRepoLatestReleaseUrl("Blessing-Studio", "WonderLab");
                    Release? release = GithubLib.GithubLib.GetRepoLatestRelease(releaseUrl);
                    if (release != null)
                    {
                        if (release.name != GetVersion())
                        {
                            ShowInfoBarAsync("自动更新", "发现新版本" + release.name + "  当前版本" + GetVersion() + "  ", InfoBarSeverity.Informational, 7000);
                        }
                    }
                });
            }
            finally
            {

            }
            await Task.Run(async () =>
            {
                IsCheckVersion = true;
                await Task.Delay(2000);
                IsCheckVersion = false;
                //string releaseUrl = GithubLib.GithubLib.GetRepoLatestReleaseUrl("Blessing-Studio", "WonderLab");
                //Release? release = GithubLib.GithubLib.GetRepoLatestRelease(releaseUrl);
                //if (release != null)
                //{
                //    if (release.name != GetVersion())
                //    {
                //        ShowInfoBarAsync("自动更新", "发现新版本" + release.name + "  当前版本" + GetVersion() + "  ", InfoBarSeverity.Informational, 7000);
                //    }
                //}
            });
        }
    }

    partial class OtherViewModel
    {
        public string Version
        {
            get => _Version;
            set => RaiseAndSetIfChanged(ref _Version, value);
        }
        public string ButtonContent
        {
            get => _ButtonContent;
            set => RaiseAndSetIfChanged(ref _ButtonContent, value);
        }

        public bool IsCheckVersion
        {
            get => _IsCheckVersion;
            set => RaiseAndSetIfChanged(ref _IsCheckVersion, value);
        }
    }

    partial class OtherViewModel
    {
        public string _Version = MainWindow.GetVersion(); 
        public string _ButtonContent = "检查更新";
        public bool _IsCheckVersion = false;
    }
}
