using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public partial class GameSettingViewModel : ViewModelBase
    {
        public string CurrentGameFolder 
        {
            get => _CurrentGameFolder;
            set
            {
                if (RaiseAndSetIfChanged(ref _CurrentGameFolder, value) && value is not null)
                {
                    App.Data.SelectedGameFooter = value;
                    App.Data.FooterPath = value;
                }
            } 
        }

        public List<string> GameFolders
        {
            get => _GameFolders;
            set => RaiseAndSetIfChanged(ref _GameFolders, value);
        }

        public List<string> Javas
        {
            get => _Javas;
            set => RaiseAndSetIfChanged(ref _Javas, value);
        }

        public List<string> Langs => new()
        {
            "简体中文",
            "English",
            "日本語",
            "한국어"
        };

        public string CurrentJava
        {
            get => _CurrentJava;
            set
            {
                if (RaiseAndSetIfChanged(ref _CurrentJava, value) && value is not null)
                {
                    App.Data.JavaPath = value;
                }
            }
        }

        public string Jvm
        {
            get => _Jvm;
            set
            {
                if (RaiseAndSetIfChanged(ref _Jvm, value) && value is not null)
                {
                    App.Data.Jvm = value;
                }
            }
        }

        public bool GameRemoveVisible
        {
            get => _GameRemoveVisible;
            set => RaiseAndSetIfChanged(ref _GameRemoveVisible, value);
        }

        public bool JavaRemoveVisible
        {
            get => _JavaRemoveVisible;
            set => RaiseAndSetIfChanged(ref _JavaRemoveVisible, value);
        }

        public string MaxMemory
        {
            get => _MaxMemory;
            set
            {
                if (RaiseAndSetIfChanged(ref _MaxMemory, value))
                {
                    App.Data.Max = int.Parse(value);
                }
            }
        }

        public bool IsFullWindow
        {
            get => _IsFullWindow;
            set
            {
                if (RaiseAndSetIfChanged(ref _IsFullWindow, value))
                {
                    App.Data.IsFull = value;
                }
            }
        }

        public bool IsOlate
        {
            get => _IsOlate;
            set
            {
                if (RaiseAndSetIfChanged(ref _IsOlate, value))
                {
                    App.Data.Isolate = value;
                }
            }
        }

        public int SelectedLang
        {
            get => _SelectedLang;
            set
            {
                if (RaiseAndSetIfChanged(ref _SelectedLang,value))
                    App.Data.SelectedLang = value;
            }
        }
    }

    partial class GameSettingViewModel
    {
        public string _MaxMemory = App.Data.Max.ToString();
        public bool _GameRemoveVisible = true;
        public bool _JavaRemoveVisible = true;
        public bool _IsFullWindow = App.Data.IsFull;
        public bool _IsOlate = App.Data.Isolate;
        public string _CurrentGameFolder = App.Data.SelectedGameFooter;
        public string _CurrentJava = App.Data.JavaPath;
        public string _Jvm = App.Data.Jvm;
        public List<string> _GameFolders = App.Data.GameFooterList;
        public List<string> _Javas = App.Data.JavaList;
        public int _SelectedLang = App.Data.SelectedLang;
    }

    partial class GameSettingViewModel
    {
        async void OpenFolderDialog()
        {
            try
            {
                var dialog = new OpenFolderDialog
                {
                    Title = "请选择游戏目录",
                };
                var result = await dialog.ShowAsync(MainWindow.win);
                if (result is not null)
                {
                    App.Data.GameFooterList.Add(result);
                    GameFolders = null;
                    GameFolders = App.Data.GameFooterList;
                    CurrentGameFolder = result;
                    GameRemoveVisible = true;
                }
                JsonToolkit.JsonWrite();
            }
            catch (Exception ex)
            {
                MainWindow.win.ShowDialog("", "");
            }
        }
        async void OpenFileDialog()
        {
            try
            {
                List<FileDialogFilter> filters = new List<FileDialogFilter>();
                var filter = new FileDialogFilter();
                //如果为win就设置后缀限制为exe
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    filter.Extensions.Add("exe");

                filter.Name = "Java路径";
                filters.Add(filter);

                var dialog = new OpenFileDialog
                {
                    Title = "请选择Java路径",
                    Filters = filters
                };

                var result = await dialog.ShowAsync(MainWindow.win);

                if (result is not null)
                {
                    foreach (var i in result)
                        App.Data.JavaList.Add(i);
                    Javas = null;
                    Javas = App.Data.JavaList;
                    CurrentJava = result.FirstOrDefault()!;
                }
                JsonToolkit.JsonWrite();
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误：", $"发生了意想不到的错误：\n{ex}", InfoBarSeverity.Error);
            }
        }
        public void AddGameAction()
        {
            OpenFolderDialog();
        }
        public void AddJavaAction()
        {
            OpenFileDialog();
        }
        public void FindJavas()
        {
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                try
                {
                    var res = Modules.Toolkits.JavaToolkit.GetJavas().Distinct();//数组去重，防止出现多个相同的java
                    foreach (var j in res)
                    {
                        App.Data.JavaList.Add(j.JavaPath);
                    }

                    Javas = null;
                    Javas = App.Data.JavaList;
                    if (Javas.Count > 0)
                    {
                        JavaRemoveVisible = true;
                        CurrentJava = Javas[0];
                        MainWindow.ShowInfoBarAsync("成功", "已将搜索到的Java加入至列表", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
                        JsonToolkit.JsonWrite();
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.ShowInfoBarAsync("错误：", $"WonderLab在找 Java 时发生了意想不到的错误：\n{ex}", InfoBarSeverity.Error);
                }
            };

            worker.RunWorkerAsync();
        }
        public void FindJavasAction()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MainWindow.ShowInfoBarAsync("提示", "正在搜索Java", FluentAvalonia.UI.Controls.InfoBarSeverity.Informational);
                FindJavas();
            }
            else
            {
                MainWindow.win.ShowDialog("错误", "回肠抱歉，该功能只能在Windows上跑（正在研究当中）");
            }
        }
        public void OutGameAction()
        {
            App.Data.GameFooterList.Remove(App.Data.SelectedGameFooter);
            GameFolders = null;
            GameFolders = App.Data.GameFooterList;
            CurrentGameFolder = App.Data.GameFooterList.Any() ? App.Data.GameFooterList[0] : null;
            GameRemoveVisible = CurrentGameFolder is null ? false : true;
        }
        public void OutJavaAction()
        {
            App.Data.JavaList.Remove(App.Data.JavaPath);
            Javas = null;
            Javas = App.Data.JavaList;
            CurrentJava = App.Data.JavaList.Any() ? App.Data.JavaList[0] : null;
            JavaRemoveVisible = CurrentJava is null ? false : true;
        }
    }

    partial class GameSettingViewModel
    {
        public GameSettingViewModel()
        {
            if (GameFolders.Count == 0 || GameFolders.Count == -1)
                GameRemoveVisible = false;

            if (Javas.Count == 0 || Javas.Count == -1)
                JavaRemoveVisible = false;
        }
    }
}
