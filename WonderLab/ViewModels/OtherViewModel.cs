using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public partial class OtherViewModel : ViewModelBase
    {
        public async void Check()
        {
            MainWindow.ShowInfoBarAsync("提示：", "开始检查更新");

            var res = await new WebToolkit().VersionCheckAsync();

            if (res != Version)
            {
                MainWindow.ShowInfoBarAsync("提示：", $"有可用更新，版本号：{res}", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
                ButtonContent = "立即更新";
                return;
            }
            else
            {
                MainWindow.ShowInfoBarAsync("提示：", "检查更新完成，无可用更新！", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
                return;
            }
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

    }

    partial class OtherViewModel
    {
        public string _Version = "测试版 1.0.8"; 
        public string _ButtonContent = "检查更新";
    }
}
