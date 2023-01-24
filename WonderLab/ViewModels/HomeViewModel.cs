using Avalonia.Controls.Notifications;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Module.Launcher;
using PluginLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.PluginAPI;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    //MetMod
    public partial class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            GameSearchAsync();
        }

        public void NavigationToUser() => _ = MainView.mv.FrameView.Navigate(typeof(UsersView));

        public void GameSearchAsync()
        {
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                GameCores.Clear();
                List<GameCore> lmlist = new();
                var game = new GameCoreToolkit(App.Data.FooterPath).GetGameCores();
                foreach (var i in game)
                {
                    string type = string.Empty;
                    if (i.Type is "release" || i.Type.Contains("正式版"))
                        type = "正式版";
                    else if (i.Type is "snapshot" || i.Type.Contains("快照版"))
                        type = "快照版";
                    else if (i.Type.Contains("old_alpha") || i.Type.Contains("远古版"))
                        type = "远古版";
                    var res = i.HasModLoader ? $"{type} 继承自 {i.Source}" : $"{type} {i.Source}";
                    i.Type = res;
                    lmlist.Add(i);
                }
                GameCores = lmlist;
                SelectedGameCore = GameCores.GetGameCoreInIndex(App.Data.SelectedGameCore);
            };
            worker.RunWorkerAsync();
        }

        public void RefreshUserAsync()
        {
            UserInfo = App.Data.SelectedUser;
        }

        public void LaunchAsync()
        {
            Enabled = false;
            var e = new GameLaunchAsyncEvent(SelectedGameCore);
            Event.CallEvent(e);
            if (e.IsCanceled)
            {
                MainWindow.ShowInfoBarAsync("提示：", $"游戏启动任务被取消", InfoBarSeverity.Informational);
            }
            Enabled = true;
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Page.NavigatedToTaskView();
        }
    }
    //Property
    partial class HomeViewModel
    {
        public bool Enabled
        {
            get => _Enabled;
            set => RaiseAndSetIfChanged(ref _Enabled, value);
        }
        public UserDataModels UserInfo
        {
            get => _UserInfo;
            set => RaiseAndSetIfChanged(ref _UserInfo, value);
        }
        public List<GameCore> GameCores
        {
            get => _GameCores;
            set => RaiseAndSetIfChanged(ref _GameCores, value);
        }
        public List<GameCore> DemoGameCores => new()
        {
            new()
            {
                Id = "1",
                Type = "11"
            },
            new()
            {
                Id = "2",
                Type = "22"
            },
        };
        public GameCore SelectedGameCore
        {
            get => _SelectedGameCore;
            set
            {
                if (RaiseAndSetIfChanged(ref _SelectedGameCore, value))
                    App.Data.SelectedGameCore = (SelectedGameCore is not null ? GameCoreToolkit.GetGameCore(App.Data.FooterPath, SelectedGameCore.Id).Id : null);
            }
        }
    }
    //Field
    partial class HomeViewModel
    {
        bool _Enabled = true;
        UserDataModels _UserInfo = App.Data.SelectedUser;
        List<GameCore> _GameCores = new();
        GameCore _SelectedGameCore = GameCoreToolkit.GetGameCore(App.Data.FooterPath, App.Data.SelectedGameCore);
    }
}
