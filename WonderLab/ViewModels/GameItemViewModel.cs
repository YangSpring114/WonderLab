using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Module.Launcher;
using System;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Models;
using WonderLab.Views;
using WonderLab.PluginAPI;
using MinecraftLaunch.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public class GameItemViewModel : ViewModelBase
    {
        public GameItemViewModel(GameCore model,GameDataModels gdm)
        {
            _InfoHeader = model.Id;
            _InfoMessage = model.Type;
            GameData = gdm;
        }
        public GameDataModels GameData { get; set; }

        public string _InfoHeader;

        public string _InfoMessage;

        public double _ButtonGroupTransition = 0;

        public double ButtonHeight => 30;

        public double ButtonGroupTransition
        {
            get => _ButtonGroupTransition;
            set => RaiseAndSetIfChanged(ref _ButtonGroupTransition, value);
        }

        public string InfoHeader => _InfoHeader;

        public string InfoMessage => _InfoMessage;

        public void MoveInAction(object? sender, Avalonia.Input.PointerEventArgs e) =>
            ButtonGroupTransition = 1;

        public void MoveOutAction(object? sender, Avalonia.Input.PointerEventArgs e) =>
            ButtonGroupTransition = 0;

        public void LaunchAsync()
        {
            string version = InfoHeader;
            var tmp = new GameCoreLocator(GameView.gv.fodlercombo.SelectedItem.ToString()).GetGameCore(version);
            if (!PluginLoader.Event.CallEvent(new GameLaunchAsyncEvent(GameCoreToolkit.GetGameCore(tmp.Root.FullName, tmp.Id))))
            {
                MainWindow.ShowInfoBarAsync("提示：", $"游戏启动任务被取消", InfoBarSeverity.Informational);
            }
        }

        public async void PropertyNavigation()
        {
            _ = MainView.mv.FrameView.Navigate(typeof(MainPropertyView));

            await Task.Run(async () =>
            {
                try
                {
                    if (GameView.gv.fodlercombo.SelectedIndex is not -1)
                    {
                        TaskBase.InvokeAsync(() =>
                        {
                            MainPropertyView.ViewModel.Id = InfoHeader;
                            var core = new GameCoreLocator(GameView.gv.fodlercombo.SelectedItem.ToString()).GetGameCore(InfoHeader);
                            string type = "";
                            if (core.Type is "release")
                                type = "正式版";
                            else if (core.Type is "snapshot")
                                type = "快照版";
                            else if (core.Type is "old_alpha")
                                type = "远古版";

                        });
                    }
                    else
                        PropertyView.PropertyViewModel.GamePath = App.Data.FooterPath;

                }
                catch (Exception ex)
                {
                    MainWindow.win.ShowDialog("错误", $"{ex.Message}");
                }
            });
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            BlessingView.IsTask = true;
            MainView.mv.FrameView.Navigate(typeof(BlessingView));
            MainView.mv.main.IsSelected = true;
            BlessingView.view.FrameView.Navigate(typeof(TaskView), null, new SlideNavigationTransitionInfo());
        }
    }
}
