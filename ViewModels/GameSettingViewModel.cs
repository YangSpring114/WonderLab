using Avalonia.Controls;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Extension.Windows.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Base;

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
                List<FileDialogFilter> v = new List<FileDialogFilter>();
                var v1 = new FileDialogFilter();
                //如果为win就设置后缀限制为exe
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    v1.Extensions.Add("exe");

                v1.Name = "Java路径";
                v.Add(v1);

                var dialog = new OpenFileDialog
                {
                    Title = "请选择Java路径",
                    Filters = v
                };
                var result = await dialog.ShowAsync(MainWindow.win);
                if (result is not null)
                {
                    foreach (var i in result)
                        App.Data.JavaList.Add(i);
                    Javas = null;
                    Javas = App.Data.JavaList;
                    CurrentJava = result[0];
                }
            }
            catch (Exception ex)
            {
                MainWindow.win.ShowDialog("", "");
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
            Thread thread = new Thread(async () =>
            {
                var res = await Task.Run(() =>
                {
                    return JavaToolkit.GetJavas().Distinct();//数组去重，防止出现多个相同的java
                });

                foreach (var j in res)
                {
                    App.Data.JavaList.Add(j.JavaPath);
                }

                Javas = null;
                Javas = App.Data.JavaList;
                if (Javas.Count>0)
                {
                    JavaRemoveVisible = true;
                    CurrentJava = Javas[0];
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        public void FindJavasAction()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FindJavas();
                MainWindow.ShowInfoBarAsync("成功", "已将搜索到的Java加入至列表", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
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
