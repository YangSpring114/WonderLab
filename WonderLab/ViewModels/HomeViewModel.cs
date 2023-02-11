using Avalonia.Controls.Notifications;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Module.Launcher;
using PluginLoader;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class HomeViewModel : ReactiveObject
    {
        public HomeViewModel() {
            PropertyChanged += OnPropertyChanged;
            GameSearchAsync();
            SelectedGameCore = GameCoreToolkit.GetGameCore(App.Data.FooterPath, App.Data.SelectedGameCore);
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (e.PropertyName is nameof(SelectedGameCore)) {
                App.Data.SelectedGameCore = (SelectedGameCore is not null ? GameCoreToolkit.GetGameCore(App.Data.FooterPath, SelectedGameCore.Id).Id : null);
            }
        }

        public void NavigationToUser() => _ = MainView.mv.FrameView.Navigate(typeof(UsersView));

        public void GameSearchAsync() {       
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                GameCores.Clear();

                var game = new GameCoreToolkit(App.Data.FooterPath).GetGameCores().Distinct();
                game.ToList().ForEach(async x =>
                {
                    var type = x.Type!.ToVersionType();
                    x.Type = x.HasModLoader ? $"{type} 继承自 {x.Source}" : $"{type} {x.Source}";
                    GameCores.Add(x);
                });              
            };
            worker.RunWorkerAsync();
        }

        public void RefreshUserAsync() {       
            UserInfo = App.Data.SelectedUser!;
        }

        public void LaunchAsync() {       
            Enabled = false;            
            var e = new GameLaunchAsyncEvent(SelectedGameCore);
            Event.CallEvent(e);
            if (e.IsCanceled) {           
                MainWindow.ShowInfoBarAsync("提示：", $"游戏启动任务被取消", InfoBarSeverity.Informational);
            }
            Enabled = true;
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {     
            Page.NavigatedToTaskView();
        }
    }
    //Property
    partial class HomeViewModel
    {
        [Reactive]
        public bool Enabled { get; set; } = true;

        [Reactive]
        public UserDataModels UserInfo { get; set; } = App.Data.SelectedUser;

        [Reactive]
        public ObservableCollection<GameCore> GameCores { get; set; } = new();

        [Reactive]
        public GameCore SelectedGameCore { get; set; }
    }
}
