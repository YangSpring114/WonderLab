﻿using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Module.Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    //MetMod
    public partial class HomeViewModel : ViewModelBase
    {
        public HomeViewModel() =>
            GameSearchAsync();

        public void NavigationToUser() => _ = MainView.mv.FrameView.Navigate(typeof(UsersView));

        public async void GameSearchAsync()
        {
            await Task.Run(() =>
            {
                GameCores.Clear();
                List<TransformationModel> lmlist = new();
                var game = new GameCoreLocator(App.Data.FooterPath).GetGameCores();
                foreach (var item in game)
                {
                    string type = "";

                    if (item.Type is "release")
                        type = "正式版";
                    else if (item.Type is "snapshot")
                        type = "快照版";
                    else if (item.Type is "old_alpha")
                        type = "远古版";

                    TransformationModel tm = new()
                    {
                        HasModLoader = item.HasModLoader,
                        Id = item.Id,
                        Type = item.HasModLoader is true ? $"{type} 继承自 {item.Source}" : $"{type} {item.Source}",
                    };

                    lmlist.Add(tm);
                }
                GameCores = lmlist;
                GameCoreToolkit gt = new(App.Data.FooterPath);
            });            
        }

        public void RefreshUserAsync()
        {
            UserInfo = App.Data.SelectedUser;
        }

        public void LaunchAsync()
        {
            string version = "";
            Enabled = false;

            #region 检查游戏核心

            try
            {
                if (SelectedGameCore is null)
                {
                    Enabled = true;
                    MainWindow.ShowInfoBarAsync("错误：", $"未选择任何游戏核心！", InfoBarSeverity.Error);
                    return;
                }
                else
                    version = SelectedGameCore.Id;

                var v = new GameCoreLocator(App.Data.FooterPath).GetGameCore(version);
                if (v is null)
                {
                    Enabled = true;
                    MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏核心：{version} 不存在或已损坏！", InfoBarSeverity.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                Enabled = true;
                MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏核心：{version} 在检查时出现了异常，详细信息：{ex.Message}", InfoBarSeverity.Error);
                return;
            }

            #endregion

            #region 检查Java

            if (!File.Exists(App.Data.JavaPath))
            {
                Enabled = true;
                MainWindow.ShowInfoBarAsync("错误：", $"选择的Java不存在或已损坏！", InfoBarSeverity.Error);
                return;
            }

            #endregion

            #region 检查账户

            if (App.Data.SelectedUser is null)
            {
                Enabled = true;
                MainWindow.ShowInfoBarAsync("错误：", $"未选择任何游戏档案！", InfoBarSeverity.Error);
                return;
            }

            if (string.IsNullOrEmpty(App.Data.SelectedUser.UserName) || string.IsNullOrEmpty(App.Data.SelectedUser.UserUuid))
            {
                Enabled = true;
                MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏档案里有信息为空！", InfoBarSeverity.Error);
                return;
            }

            #endregion

            #region 启动

            var button = new HyperlinkButton()
            {
                Content = "转至 祝福终端>任务中心",
            };

            button.Click += Button_Click;

            MainWindow.ShowInfoBarAsync("提示：", $"开始启动游戏核心：{SelectedGameCore.Id}，可前往任务中心查看详细信息", InfoBarSeverity.Informational, 5000, button);

            LaunchItemView view = new(version, App.Data.SelectedUser);

            if (TaskView.itemView.Count is not 0 && TaskView.task is not null)
            {
                TaskView.task.infopanel.Children.Add(view);
                TaskView.task.nullText.IsVisible = false;
            }
            else if (TaskView.itemView.Count is not 0 && TaskView.task is null)
                TaskView.itemView.Add(view);
            else if (TaskView.itemView.Count is 0 && TaskView.task is null)
                TaskView.itemView.Add(view);
            else if (TaskView.itemView.Count is 0 && TaskView.task is not null)
                TaskView.task.AddItem(view);


            #endregion

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
        public List<TransformationModel> GameCores
        {
            get => _GameCores;
            set => RaiseAndSetIfChanged(ref _GameCores, value);
        }
        public List<TransformationModel> DemoGameCores => new()
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
        public TransformationModel SelectedGameCore
        {
            get => _SelectedGameCore;
            set
            {
                if (RaiseAndSetIfChanged(ref _SelectedGameCore, value))
                    App.CurrentGameCore = (SelectedGameCore is not null ? new GameCoreLocator(App.Data.FooterPath).GetGameCore(SelectedGameCore.Id) : null);
            }
        }
    }
    //Field
    partial class HomeViewModel
    {
        bool _Enabled = true;
        UserDataModels _UserInfo = App.Data.SelectedUser;
        List<TransformationModel> _GameCores = new();
        TransformationModel _SelectedGameCore = App.Data.SelectedGameCore;
    }
}
