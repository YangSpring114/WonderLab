using Avalonia.Controls;
using MinecraftLaunch.Modules.Toolkits;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
            TasksTooklit.CreateModDownloadTask((CurseForgeModel)((Button)sender).DataContext!, sender as Control);

            //DemoBox.Items = GameCoreToolkit.GetGameCores(App.Data.FooterPath).Where(x => x.HasModLoader);
            //await SelectGameCoreDialog.ShowAsync();
            //var core = GameCoreToolkit.GetGameCore(App.Data.FooterPath, App.Data.SelectedGameCore!);
            //if (!string.IsNullOrEmpty(App.Data.SelectedGameCore) && core is not null && core.HasModLoader == true)               
            //{
            //    TasksTooklit.CreateModDownloadTask((CurseForgeModel)((Button)sender).DataContext!, sender as Control);
            //}
            //else if (string.IsNullOrEmpty(App.Data.SelectedGameCore))
            //{
            //    MainWindow.ShowInfoBarAsync("Debug - 警告：","未选游戏核心", FluentAvalonia.UI.Controls.InfoBarSeverity.Warning);
            //}
            //else if (core is not null && !core.HasModLoader)
            //{
            //    MainWindow.ShowInfoBarAsync("Debug - 警告：", "选择的游戏核心未安装模组加载器", FluentAvalonia.UI.Controls.InfoBarSeverity.Warning);
            //}
        }

        public async void SaveInstallClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mod = ((CurseForgeModel)((Button)sender!).DataContext!);
            SaveFileDialog dialog = new();
            dialog.Title = "请选择要保存的位置";
            dialog.Filters = new() { new() { Name = "模组文件", Extensions = new() { "jar" } } };
            dialog.InitialFileName = mod.CurrentFileInfo.FileName;

            if (!string.IsNullOrEmpty(mod.ChineseName) && Regex.IsMatch(mod.ChineseName, @"[\u4e00-\u9fa5]")) {
                dialog.InitialFileName = $"[{mod.ChineseName.Split("(").FirstOrDefault().Trim()}] {mod.CurrentFileInfo.FileName}";
            }
            var res = await dialog.ShowAsync(MainWindow.win);

            if (!string.IsNullOrEmpty(res)) {
                TasksTooklit.CreateModDownloadTask(mod, (sender as Control)!, res);
            }
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {        
            SelectGameCoreDialog.Hide();
        }
    }
}
