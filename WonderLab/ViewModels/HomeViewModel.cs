using Avalonia.Controls.Notifications;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Module.Launcher;
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
            string version = "";
            Enabled = false;

            #region 预启动检查

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
