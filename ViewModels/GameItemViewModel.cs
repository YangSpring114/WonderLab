using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using Microsoft.VisualBasic;
using Natsurainko.FluentCore.Module.Launcher;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Models;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    public class GameItemViewModel : ViewModelBase
    {
        public GameItemViewModel(TransformationModel model,GameDataModels gdm)
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
            string version = "";
            version = InfoHeader;

            #region 检查游戏核心

            try
            {
                var v = new GameCoreLocator(GameView.gv.fodlercombo.SelectedItem.ToString()).GetGameCore(version);
                if (v is null)
                {
                    MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏核心：{version} 不存在或已损坏！", InfoBarSeverity.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏核心：{version} 在检查时出现了异常，详细信息：{ex.Message}", InfoBarSeverity.Error);
                return;
            }

            #endregion

            #region 检查Java

            if (!File.Exists(App.Data.JavaPath))
            {
                MainWindow.ShowInfoBarAsync("错误：", $"选择的Java不存在或已损坏！", InfoBarSeverity.Error);
                return;
            }

            #endregion

            #region 检查账户

            if (string.IsNullOrEmpty(App.Data.SelectedUser.UserName) || string.IsNullOrEmpty(App.Data.SelectedUser.UserUuid))
            {
                MainWindow.ShowInfoBarAsync("错误：", $"选择的游戏档案里有信息为空！", InfoBarSeverity.Error);
                return;
            }

            #endregion

            var button = new HyperlinkButton()
            {
                Content = "转至 祝福终端>任务中心",
            };

            button.Click += Button_Click;

            MainWindow.ShowInfoBarAsync("提示：", $"开始启动游戏核心：{InfoHeader}，可前往任务中心查看详细信息", InfoBarSeverity.Informational, 5000, button);

            LaunchItemView view = new(version, App.Data.SelectedUser, GameView.gv.fodlercombo.SelectedItem.ToString());

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
