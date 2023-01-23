using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Module.Launcher;
using PluginLoader;
using WonderLab.Modules.Controls;
using WonderLab.Views;

namespace WonderLab.PluginAPI
{
    public class GameLaunchAsyncEvent : Event, ICancellable
    {
        public GameCore SelectedGameCore;
        public GameLaunchAsyncEvent(GameCore GameCore)
        {
            SelectedGameCore = GameCore;
        }
        public bool IsCanceled { get; set; }
        public override string Name
        {
            get
            {
                return "GameLaunchAsyncEvent";
            }
        }


        public override bool Do()
        {
            string version = "";


            #region 预启动检查

            #region 检查游戏核心

            try
            {
                if (SelectedGameCore is null)
                {
                    MainWindow.ShowInfoBarAsync("错误：", $"未选择任何游戏核心！", InfoBarSeverity.Error);
                    return false;
                }
                else
                    version = SelectedGameCore.Id;

                var v = new GameCoreLocator(App.Data.FooterPath).GetGameCore(version);
                if (v is null)
                {
                    MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏核心：{version} 不存在或已损坏！", InfoBarSeverity.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏核心：{version} 在检查时出现了异常，详细信息：{ex.Message}", InfoBarSeverity.Error);
                return false;
            }

            #endregion

            #region 检查Java

            if (!File.Exists(App.Data.JavaPath))
            {
                MainWindow.ShowInfoBarAsync("错误：", $"选择的Java不存在或已损坏！", InfoBarSeverity.Error);
                return false;
            }

            #endregion

            #region 检查账户

            if (App.Data.SelectedUser is null)
            {
                MainWindow.ShowInfoBarAsync("错误：", $"未选择任何游戏档案！", InfoBarSeverity.Error);
                return false;
            }

            if (string.IsNullOrEmpty(App.Data.SelectedUser.UserName) || string.IsNullOrEmpty(App.Data.SelectedUser.UserUuid))
            {
                MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏档案里有信息为空！", InfoBarSeverity.Error);
                return false;
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
            return true;
        }
        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Page.NavigatedToTaskView();
        }
    }
}
