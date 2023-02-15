using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using DynamicData;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

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

        public ObservableCollection<string> GameFolders
        {
            get => _GameFolders;
            set {
                if (RaiseAndSetIfChanged(ref _GameFolders, value)) {
                    ///App.Data.GameFooterList = value;
                }
            }
        }

        public ObservableCollection<string> Javas
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
                if (RaiseAndSetIfChanged(ref _CurrentJava, value) && value is not null) {               
                    App.Data.JavaPath = value;
                }
            }
        }

        public string Jvm
        {
            get => _Jvm;
            set
            {
                if (RaiseAndSetIfChanged(ref _Jvm, value) && value is not null) {               
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
                    if(!string.IsNullOrEmpty(value)) {
                        App.Data.Max = int.Parse(value);
                    }
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

        public bool IsSearchJavaLoading
        {
            get => _IsSearchJavaLoading;
            set => RaiseAndSetIfChanged(ref _IsSearchJavaLoading, value);
        }

        public bool AutoSelectJava
        {
            get => _AutoSelectJava;
            set { RaiseAndSetIfChanged(ref _AutoSelectJava, value); App.Data.AutoSelectJava = value; }
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
        public bool _AutoSelectJava = App.Data.AutoSelectJava;
        public bool _JavaRemoveVisible = true;
        public bool _IsFullWindow = App.Data.IsFull;
        public bool _IsOlate = App.Data.Isolate;
        public bool _IsSearchJavaLoading = false;
        public string _CurrentGameFolder = App.Data.SelectedGameFooter;
        public string _CurrentJava = App.Data.JavaPath;
        public string _Jvm = App.Data.Jvm;
        public ObservableCollection<string> _GameFolders = new();
        public ObservableCollection<string> _Javas = new();
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
                    if (App.Data.GameFooterList is null)
                        App.Data.GameFooterList = new();

                    App.Data.GameFooterList.Add(result);

                    GameFolders.Add(result);
                    GameView.ViewModel.FodlerList.Add(result);

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
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    filter.Extensions.Add("exe");
                filter.Name = "Java路径";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    filters.Add(filter);
                OpenFileDialog dialog;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {                
                    dialog = new OpenFileDialog
                    {
                        Title = "请选择Java路径",
                        Filters = filters
                    };
                } else {
                    dialog = new OpenFileDialog
                    {
                        Title = "请选择Java路径",
                    };
                }

                var result = await dialog.ShowAsync(MainWindow.win);

                if (result is not null)
                {
                    var javapath = result.Where(x => new FileInfo(x).Exists).ToList().First();

                    App.Data.JavaList.Add(javapath);
                    Trace.WriteLine($"[调试] 添加的 Java 路径为 {javapath}");
                    Javas.Add(javapath);
                    //Javas = new();
                    //Javas = App.Data.JavaList.Distinct().ToList();
                    CurrentJava = javapath;

                    Trace.WriteLine($"[调试] 活动 Java 路径为 {CurrentJava}");
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
            IsSearchJavaLoading = true;
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                try
                {
                    if (InfoConst.IsWindows)
                    {
                        var res = Modules.Toolkits.JavaToolkit.GetJavas().Distinct();//数组去重，防止出现多个相同的java

                        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        basePath = Path.Combine(basePath, ".minecraft", "runtime");
                        var paths = new[] { "java-runtime-alpha", "java-runtime-beta", "jre-legacy" };
                        App.Data.JavaList.AddRange(paths.Select(path => Path.Combine(basePath, path, "bin", "javaw.exe")).Where(File.Exists).Distinct());  
                        
                        foreach (var j in res)
                            App.Data.JavaList.Add(j.JavaPath);
                        //Fix #36
                        var tmp = App.Data.JavaList.Distinct();
                        App.Data.JavaList = new List<string>(tmp);
                    }
                    else if (InfoConst.IsLinux)
                    {
                        //尝试搜索官方游戏文件夹目录里的 Java
                        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        basePath = Path.Combine(basePath, ".minecraft", "runtime");
                        var paths = new[] { "java-runtime-alpha", "java-runtime-beta", "jre-legacy" };
                        App.Data.JavaList.AddRange(paths.Select(path => Path.Combine(basePath, path, "bin", "java")).Where(File.Exists).Distinct());
                    }
                    else//傻逼MacOS
                    {
                        //尝试搜索官方游戏文件夹目录里的 Java
                        var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        var basePath = Path.Combine(path, "Application Support");
                        basePath = Path.Combine(basePath, ".minecraft", "runtime");
                        var paths = new[] { "java-runtime-alpha", "java-runtime-beta", "jre-legacy" };
                        App.Data.JavaList.AddRange(paths.Select(path => Path.Combine(basePath, path, "bin", "java")).Where(File.Exists).Distinct());
                    }
                    //使用javaSearcher项目的跨平台搜索方法
                    var javas = javaInfo.JavaInfo.FindJava().ToList();
                    foreach (var java in javas)
                        App.Data.JavaList.Add(java.Path);
                    App.Data.JavaList = new List<string>(App.Data.JavaList.Distinct());
                    //END
                    Javas.AddRange(App.Data.JavaList.Distinct());
                    Javas = Javas.Distinct().BuildObservableCollection();
                    IsSearchJavaLoading = false;
                    if (Javas.Count > 0)
                    {
                        CurrentJava = string.Empty;
                        JavaRemoveVisible = true;
                        CurrentJava = Javas[0];
                        MainWindow.ShowInfoBarAsync("成功", $"已将搜索到的Java加入至列表，总计 {Javas.Count} 个", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                MainWindow.ShowInfoBarAsync("提示", "正在搜索Java", FluentAvalonia.UI.Controls.InfoBarSeverity.Informational);
                FindJavas();
            }
            else
            {
                MainWindow.win.ShowDialog("错误", "回肠抱歉，这个功能还不支持你的系统（正在研究当中）");
            }
        }

        public void OutGameAction()
        {
            App.Data.GameFooterList.Remove(App.Data.SelectedGameFooter);
            GameFolders.Remove(CurrentGameFolder);
            GameView.ViewModel.FodlerList.Remove(CurrentGameFolder);
            CurrentGameFolder = App.Data.GameFooterList.Any() ? App.Data.GameFooterList[0] : null!;
            GameRemoveVisible = CurrentGameFolder is null ? false : true;
            //App.Data.GameFooterList.Remove(App.Data.SelectedGameFooter);
            //GameFolders = App.Data.GameFooterList;
        }

        public void OutJavaAction()
        {
            Trace.WriteLine($"[调试] 即将被移除的 Java 运行时 {App.Data.JavaPath}");
            App.Data.JavaList.Remove(App.Data.JavaPath);
            Javas.Remove(App.Data.JavaPath);
            CurrentJava = Javas.Any()! ? Javas[0] : null!;
            JavaRemoveVisible = Javas.Any()! ? true : false;
            //Javas = new();
            //Javas = App.Data.JavaList.Distinct().ToList();
            Trace.WriteLine($"[调试] 被移除的 Java 运行时后面的 Java 运行时为 {CurrentJava}");
        }
    }

    partial class GameSettingViewModel
    {
        public GameSettingViewModel()
        {
            App.Data.JavaList.ForEach(x => Javas.Add(x));
            App.Data.GameFooterList.ForEach(x => GameFolders.Add(x));

            if (GameFolders is not null && (GameFolders.Count == 0 || GameFolders.Count == -1))
                GameRemoveVisible = false;

            if (Javas is not null && (Javas.Count == 0 || Javas.Count == -1))
                JavaRemoveVisible = false;
        }
    }
}
