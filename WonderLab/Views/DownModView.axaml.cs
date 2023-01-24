using Avalonia.Controls;
using MinecraftLaunch.Modules.Toolkits;
using System.Diagnostics;
using System.Linq;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class DownModView : Page
    {
        public static DownModViewModel ViewModel = new();
        public DownModView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        
        public void NavigationToModInfo(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }

        public async void InstallClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DemoBox.Items = GameCoreToolkit.GetGameCores(App.Data.FooterPath).Where(x => x.HasModLoader);
            await SelectGameCoreDialog.ShowAsync();
            var core = GameCoreToolkit.GetGameCore(App.Data.FooterPath, App.Data.SelectedGameCore!);
            if (!string.IsNullOrEmpty(App.Data.SelectedGameCore) && core is not null && core.HasModLoader == true)               
            {
                TasksTooklit.CreateModDownloadTask((CurseForgeModel)((Button)sender).DataContext!, sender as Control);
            }
            else if (string.IsNullOrEmpty(App.Data.SelectedGameCore))
            {
                MainWindow.ShowInfoBarAsync("Debug - 警告：","未选游戏核心", FluentAvalonia.UI.Controls.InfoBarSeverity.Warning);
            }
            else if (core is not null && !core.HasModLoader)
            {
                MainWindow.ShowInfoBarAsync("Debug - 警告：", "选择的游戏核心未安装模组加载器", FluentAvalonia.UI.Controls.InfoBarSeverity.Warning);
            }
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {        
            SelectGameCoreDialog.Hide();
        }
    }
}
