using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
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
            if (!IsCanceled)
            {
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
                string javapath = App.Data.JavaPath;
                javapath = GetCorrectOfGameJava(App.Data.JavaList.AsParallel().Select(x => JavaToolkit.GetJavaInfo(x)), SelectedGameCore);

                if (App.Data.JavaList.Count <= 0) {
                    MainWindow.ShowInfoBarAsync("错误", "没有任何 Java", InfoBarSeverity.Error);
                    return false;
                }

                if (!App.Data.AutoSelectJava && (string.IsNullOrEmpty(App.Data.JavaPath) || !File.Exists(App.Data.JavaPath))) {                
                    MainWindow.ShowInfoBarAsync("错误", "选择的 Java 不存在或已损坏", InfoBarSeverity.Error);
                    return false;
                }

                if (App.Data.AutoSelectJava && string.IsNullOrEmpty(javapath)) {
                    MainWindow.ShowInfoBarAsync("错误", "未能成功自动选择到合适的 Java 运行时，可能是您没有安装导致的", InfoBarSeverity.Error);
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

                LaunchItemView view = new(version, App.Data.SelectedUser, javapath);

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
            }
            return true;
        }
        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Page.NavigatedToTaskView();
        }

        /// <summary>
        /// 自动获取适合游戏版本的 Java 运行时
        /// </summary>
        /// <param name="Javas"></param>
        /// <param name="gameCore"></param>
        /// <returns></returns>
        public string GetCorrectOfGameJava(IEnumerable<JavaInfo> Javas, GameCore gameCore)
        {
            string javaInfo = null;
            foreach (JavaInfo Java in Javas)
            {
                if (Java.JavaSlugVersion == gameCore.JavaVersion && Java.JavaDirectoryPath.ToLower().Contains("jdk")
                    && !Java.JavaDirectoryPath.ToLower().Contains("jre") && Java.Is64Bit)
                {
                    javaInfo = Java.JavaPath;
                }
            }

            if (javaInfo is null)
            {
                foreach (var Java in Javas)
                {
                    if (Java.JavaSlugVersion == gameCore.JavaVersion && Java.Is64Bit && Java.JavaDirectoryPath.ToLower().Contains("jdk"))
                    {
                        javaInfo = Java.JavaPath;
                    }
                }
            }

            if (javaInfo == null)
            {
                foreach (JavaInfo Java2 in Javas)
                {
                    if (Java2.JavaSlugVersion == gameCore.JavaVersion && Java2.Is64Bit)
                    {
                        javaInfo = Java2.JavaPath;
                    }
                }
            }

            if (javaInfo == null)
            {
                foreach (JavaInfo Java2 in Javas)
                {
                    if (Java2.JavaSlugVersion == gameCore.JavaVersion)
                    {
                        javaInfo = Java2.JavaPath;
                    }
                }
            }

            return javaInfo;
        }
    }
}
