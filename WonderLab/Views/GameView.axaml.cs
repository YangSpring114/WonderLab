using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Natsurainko.FluentCore.Module.Launcher;
using System.Collections.Generic;
using System;
using WonderLab.Modules.Models;
using Avalonia.OpenGL;
using FluentAvalonia.UI.Media.Animation;
using System.Diagnostics;
using WonderLab.ViewModels;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Models.Launch;
using Button = Avalonia.Controls.Button;
using Avalonia.Controls.Presenters;
using WonderLab.Modules.Controls;
using MenuFlyout = FluentAvalonia.UI.Controls.MenuFlyout;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using WonderLab.PluginAPI;
using PluginLoader;
using Avalonia.Media;
using MinecraftLaunch.Modules.Toolkits;

namespace WonderLab.Views
{
    public partial class GameView : Page
    {
        public static double ScrollViewerWidth => gv.ScrollViewerHost.Width;
        public static double HostPageWidth => gv.Width;
        public static GameView gv;
        public static GameViewModels ViewModel { get; } = new();
        public GameView()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ChangeNameDialog.DataContext = ViewModel;
            gv = this;            
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
        }

        public override void OnNavigatedTo()
        {             
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                ViewModel.SelectCoreVisibilityOption = 0;
                ViewModel.SelectCoreSortOption = 0;
                ViewModel.SelectedFooler = App.Data.FooterPath;
                ViewModel.GameSearchAsync();
                Trace.WriteLine($"[调试] 选中的游戏文件夹的值：{ViewModel.SelectedFooler}");
            };

            worker.RunWorkerAsync();
        }

        private void LaunchButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) =>
            ViewModel.LaunchClick((((Button)sender!).DataContext as GameCore)!);

        private void ButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) =>
            ViewModel.LaunchClick((((MenuFlyoutItem)sender!).DataContext as GameCore)!);

        private void ButtonClick1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainPropertyView.SelectGameCore = (((MenuFlyoutItem)sender!).DataContext as GameCore)!;
            var view = new MainPropertyView();
            MainView.mv.FrameView.Navigate(view.GetType());
        }

        private async void ButtonClick2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Temp = (((MenuFlyoutItem)sender!).DataContext as GameCore);
        }

        private async void ButtonClick4(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            GameCoreToolkit.Delete((((MenuFlyoutItem)sender!).DataContext as GameCore).Root.FullName, (((MenuFlyoutItem)sender!).DataContext as GameCore).Id);
        }

        private async void ButtonClick3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var core = (((MenuFlyoutItem)sender!).DataContext as GameCore)!;

            try {           
                await Task.Run(() => {
                    using var res = Process.Start(new ProcessStartInfo(core.Root.FullName)
                    {
                        UseShellExecute = true,
                        Verb = "open"
                    });
                });
            } finally {
                GC.Collect();
            }
        }
        //Fix #31
        public void DeleteGameCore(object? sender, Avalonia.Interactivity.RoutedEventArgs e2)
        {
            var e = new GameDeleteEvent(ViewModel.CurrentGameCore);
            if (Event.CallEvent(e))
            {
                if (!e.IsCanceled)
                {
                    MainWindow.ShowInfoBarAsync("成功", $"已将游戏核心 {ViewModel.CurrentGameCore.Id} 删除！", InfoBarSeverity.Success);
                }
                else
                {
                    MainWindow.ShowInfoBarAsync("提示", $"游戏核心删除被插件取消！", InfoBarSeverity.Informational);
                }
            }
            else
            {
                MainWindow.ShowInfoBarAsync("错误", $"在删除游戏核心时发生了错误！", InfoBarSeverity.Error);
            }
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //if (ViewModel.CurrentGameCore is not null)
            ChangeNameDialog.Hide();
            ViewModel.NewGameCoreName = string.Empty;
        }

        private void ChangeGameCoreNameClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ViewModel.ChangeGameCoreName(Temp);
            ChangeNameDialog.Hide();
        }

        GameCore Temp = null;
    }
}
//(((MenuFlyoutItem)sender!).DataContext as GameCore)