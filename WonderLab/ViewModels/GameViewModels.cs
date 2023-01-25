using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Module.Launcher;
using PluginLoader;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.PluginAPI;
using WonderLab.Views;
using GameCore = MinecraftLaunch.Modules.Models.Launch.GameCore;

namespace WonderLab.ViewModels
{
    public partial class GameViewModels : ViewModelBase
    {
        public string TipsLink => "转到 祝福终端 或点击 安装新的游戏核心 按钮以安装新的游戏核心";

        public ListBox CoresList { get; set; }

        public ObservableCollection<GameCore> GameCores
        {
            get => _gameCores;
            set
            {
                if (RaiseAndSetIfChanged(ref _gameCores, value))
                {
                    //Debug.WriteLine(value.Count);
                }
            }
        }
        
        public ObservableCollection<string> FodlerList
        {
            get => _FodlerList;
            set => RaiseAndSetIfChanged(ref _FodlerList, value);
        }

        public List<string> CoreSortOption => new()
        {
            "按名称排序",
            "按启动时间排序"
        };

        public List<string> CoreVisibilityOption => new()
        {
            "全部显示",
            "仅显示正式版",
            "仅显示快照版"
        };

        public GameCore CurrentGameCore
        {
            get => _gameCore;
            set
            {
                if (RaiseAndSetIfChanged(ref _gameCore, value))
                {
                    if (_gameCore is not null && value is not null)
                    {
                        App.CurrentGameCore = new GameCoreLocator(App.Data.FooterPath).GetGameCore(value.Id);
                        App.Data.SelectedGameCore = value.Id;
                    }
                    else App.Data.SelectedGameCore = null;
                }
            }
        }

        public string SelectedFooler
        {
            get => _SelectedIndex;
            set
            {
                if (RaiseAndSetIfChanged(ref _SelectedIndex, value) && !string.IsNullOrEmpty(value))
                {
                    App.Data.FooterPath = value;
                    App.Data.SelectedGameFooter = value;
                    GameSearchAsync();
                }
            }
        }

        public bool IsCoresHas
        {
            get => _iscorehas;
            set => RaiseAndSetIfChanged(ref _iscorehas, value);
        }

        public string NewGameCoreName
        {
            get => _NewGameCoreName;
            set => RaiseAndSetIfChanged(ref _NewGameCoreName, value);
        }

        public int SelectCoreSortOption
        {
            get => _SelectCoreSortOption;
            set
            {
                if (RaiseAndSetIfChanged(ref _SelectCoreSortOption, value))
                {
                    UpdateGameCores();
                }
            }
        }

        public int SelectCoreVisibilityOption
        {
            get => _SelectCoreVisibilityOption;
            set
            {
                if (RaiseAndSetIfChanged(ref _SelectCoreVisibilityOption, value))
                {
                    UpdateGameCores();
                }
            }
        }

        public string GameCoresFilter
        {
            get => _GameCoresFilter;
            set
            {
                if (RaiseAndSetIfChanged(ref _GameCoresFilter, value))
                    SearchGameCores();
            }
        }
    }
    
    partial class GameViewModels
    {
        public GameViewModels() {
            if(App.Data.GameFooterList is not null && App.Data.GameFooterList.Count > 0) {
                FodlerList = App.Data.GameFooterList.BuildObservableCollection();
            }
        }

        public void LaunchClick(GameCore core)
        {
            LaunchAsync(core);
        }

        public void NavigationToDownGameCore()
        {
            MainView.mv.FrameView.Navigate(typeof(DownGameView));
        }

        public void FodlerRefresh()
        {
            //if (FodlerList.Count != App.Data.GameFooterList.Count) {
            //    FodlerList = new();
            //    App.Data.GameFooterList.Distinct().ToList().ForEach(x => FodlerList.Add(x));
            //}

            //if (!SelectedFooler.Equals(App.Data.FooterPath)) {
            //    SelectedFooler = App.Data.FooterPath;
            //}
        }

        public void ChangeGameCoreName()
        {
            var oldname = CurrentGameCore.Id;
            var game = new GameCoreToolkit(App.Data.FooterPath);
            if ("".Equals(NewGameCoreName))
            {
                MainWindow.ShowInfoBarAsync("错误", $"游戏核心名不能为空值！", InfoBarSeverity.Error);
                return;
            }
            var res = game.GameCoreNameChange(oldname, NewGameCoreName);
            if (NewGameCoreName.Equals(res.Id))
            {
                MainWindow.ShowInfoBarAsync("成功", $"游戏核心 {oldname} 的名字已更改为 {NewGameCoreName}", InfoBarSeverity.Success);
                GameSearchAsync();
                NewGameCoreName = string.Empty;
            }
        }

        public async void GameSearchAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(SelectedFooler))
                    {
                        GameCores.Clear();

                        var game = new GameCoreToolkit(App.Data.FooterPath).GetGameCores().Distinct();
                        game.ToList().ForEach(x =>
                        {
                            var type = x.Type!.ToVersionType();
                            x.Type = x.HasModLoader ? $"{type} 继承自 {x.Source}" : $"{type} {x.Source}";
                            GameCores.Add(x);
                        });

                        Trace.WriteLine($"[信息] 游戏列表的元素个数为 {GameCores.Count}");

                        UpdateTips();

                        //if (GameCores.Count > 0) {                        
                        //    CurrentGameCore = GameCores.ToList().GetGameCoreInIndex(App.Data.SelectedGameCore);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"[错误] {ex}");
                }
            });
        }

        public void LaunchAsync(GameCore core)
        {
            var e = new GameLaunchAsyncEvent(core);
            Event.CallEvent(e);
            if (e.IsCanceled)
            {
                MainWindow.ShowInfoBarAsync("提示：", $"游戏启动任务被取消", InfoBarSeverity.Informational);
            }
        }

        public async void SearchGameCores()
        {
            if (!string.IsNullOrEmpty(GameCoresFilter))
            {
                GameSearchAsync();
                await Task.Delay(100);
                GameCores.Where(x =>
                {
                    if (x.Id.Contains(GameCoresFilter))
                        return true;

                    return false;
                }).ToList().ForEach(x => GameCores.Add(x));

                UpdateTips();
            }
        }

        private void UpdateGameCores()
        {
            string SelectedGameCore = string.Empty;
            if (!string.IsNullOrEmpty(SelectedFooler))
            {
                var gameCores = (new GameCoreToolkit(SelectedFooler).GetGameCores()).Where(x =>
                {
                    if (SelectCoreVisibilityOption == 0)
                        return true;
                    else if (SelectCoreVisibilityOption == 1 && x.Type == "release")
                        return true;
                    else if (SelectCoreVisibilityOption == 2 && x.Type == "snapshot")
                        return true;
                    else if (SelectCoreVisibilityOption == 3 && x.Type.StartsWith("old"))
                        return true;

                    return false;
                }).ToList();

                if (SelectCoreSortOption == 0)
                    gameCores.Sort((a, b) => a.Id.CompareTo(b.Id));
                else
                {
                    var tempgamecore = gameCores.Select(x => x.CreateViewData<GameCore, GameCoreViewData<GameCore>>()).ToList();
                    tempgamecore.ForEach(x =>
                    {
                        Trace.WriteLine(x.Data.Id);
                        var cache = JsonToolkit.GetEnableIndependencyCoreData(SelectedFooler,x.Data.ToNatsurainkoGameCore());
                        x.LastLaunchTime = cache is null ? default : cache.LastLaunchTimeVM;
                    });
                    tempgamecore.Sort((x, x1) => x.LastLaunchTime.CompareTo(x1.LastLaunchTime));
                    gameCores = tempgamecore.Select(x => x.Data).ToList();
                    gameCores.Reverse();
                }

                gameCores = gameCores.Where(x => 
                {
                    var type = x.Type.ToVersionType();
                    x.Type = x.HasModLoader ? $"{type} 继承自 {x.Source}" : $"{type} {x.Source}";
                    return true;
                }).ToList();

                GameCores = gameCores.BuildObservableCollection();

                if (GameCores.Count > 0 && GameCores.Where(x => x.Id == SelectedGameCore).Count() == 1) {
                    App.Data.SelectedGameCore = SelectedGameCore;
                    CurrentGameCore = GameCores.ToList().GetGameCoreInIndex(SelectedGameCore);
                }
            }            

            UpdateTips();
            //CoresList.ScrollIntoView(CurrentGameCore);
        }

        void UpdateTips()
        {
            if (GameCores.Count <= 0)
            {
                IsCoresHas = true;
                return;
            }
            IsCoresHas = false;
        }

        public void NavigatedToDownView()
        {
            Page.NavigatedToDownView();
        }

        private void ButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Page.NavigatedToTaskView();
        }
    }

    partial class GameViewModels
    {
        public ObservableCollection<GameCore> _gameCores = new();
        public GameCore _gameCore = new();
        public bool _iscorehas = true;
        public int _SelectCoreSortOption = 0;
        public int _SelectCoreVisibilityOption = 0;
        public string _SelectedIndex = App.Data.FooterPath;
        public string _NewGameCoreName = string.Empty;
        public string _GameCoresFilter = string.Empty;
        public ObservableCollection<string> _FodlerList = new();
    }
}
