using System.Threading.Tasks;
using WonderLab.Modules.Base;
using GithubLib;
using FluentAvalonia.UI.Controls;
using static WonderLab.MainWindow;
using PluginLoader;
using WonderLab.PluginAPI;

namespace WonderLab.ViewModels
{
    public partial class OtherViewModel : ViewModelBase
    {
        public async Task Check()
        {
            Event.CallEvent(new UpdataCheckEvent());
            try
            {
                await Task.Run(async () =>
                {
                    IsCheckVersion = true;
                    await Task.Delay(2000);
                    IsCheckVersion = false;
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
            catch { }
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
