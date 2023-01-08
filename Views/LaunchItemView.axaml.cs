using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.OpenGL;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using MinecraftLaunch.Events;
using MinecraftLaunch.Launch;
using MinecraftLaunch.Modules.Analyzers;
using MinecraftLaunch.Modules.Interface;
using MinecraftLaunch.Modules.Models.Auth;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Interface;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;
using Button = Avalonia.Controls.Button;

namespace WonderLab.Views
{
    public partial class LaunchItemView : Page, ITask
    {
        Process GameProcess = null;

        ConsoleWindow Window = null;

        string Path = "";

        public LaunchItemView() => InitializeComponent();

        public LaunchItemView(string version, UserDataModels userData,string IsList = "")
        {
            MainView.ViewModel.AllTaskCount++;
            InitializeComponent();
            Path = IsList;
            Async(version, userData);
        }

        async void Async(string version,UserDataModels userData)
        {
            var v = await CheckFileAsync(version,Path);
            if (v.Item1 is true && v.Item2 is "")
                LaunchAsync(version, userData);
            else
            {
                MainWindow.ShowInfoBarAsync("错误", "游戏资源文件补全失败", InfoBarSeverity.Error);
            }
        }//MainWindow.ShowInfoBarAsync

        private void InitializeComponent()
        {
            InitializeComponent(true);
            gamelog.Click += Gamelog_Click;
            closegame.Click += Closegame_Click;
        }

        private void Gamelog_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window = null;
            Window = new(LaunchResponse, GameId);
            Window.Show();
        }

        private void Closegame_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) =>
            CloseGame();

        public void CloseGame()
        {
            if (GameProcess is not null && !GameProcess.HasExited)
            {
                try
                {
                    GameProcess.Kill();
                    main.Description = "游戏进程已退出";
                    gameout.IsVisible = true;
                    IsKill = true;
                    MainWindow.ShowInfoBarAsync("成功", "游戏进程成功被关闭！", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
                    Close.IsVisible = false;
                }
                catch (InvalidOperationException)
                {
                    MainWindow.ShowInfoBarAsync("错误", "游戏进程已经被关闭或崩溃", FluentAvalonia.UI.Controls.InfoBarSeverity.Warning);
                }
            }
            else
                MainWindow.ShowInfoBarAsync("作甚", "游戏都没开就想关，搁着给我搁着呢", FluentAvalonia.UI.Controls.InfoBarSeverity.Warning);
        }

        /// <summary>
        /// 检测游戏文件是否缺失
        /// </summary>
        /// <returns></returns>
        public async Task<(bool,string)> CheckFileAsync(string version,string i = "")
        {
            (bool, string) refs = new();
            try
            {
                var locator = new GameCoreToolkit(i is "" ? App.Data.FooterPath : Path);

                var gc = locator.GetGameCore(version);
                if (gc is not null)
                {
                    var resourceDownloader = new MinecraftLaunch.Modules.Installer.ResourceInstaller(gc);
                    await resourceDownloader.DownloadAsync(async (e, x) =>
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            main.Description = "资源补全进度：" + e;
                        }, DispatcherPriority.Background);
                    });
                    refs.Item1 = true;
                    refs.Item2 = "";
                }
                else
                {
                    gameout.IsVisible = true;
                    Close.IsVisible = false;
                    MainWindow.ShowInfoBarAsync("错误", "文件补全失败，可能是核心不存在导致的！", InfoBarSeverity.Error);
                    refs.Item1 = false;
                    refs.Item2 = "异常";
                }
                return refs;
            }
            catch (Exception ex)
            {
                refs.Item1 = false;
                refs.Item2 = ex.GetType().Name;
                return refs;
            }
        }

        public void LaunchAsync(string version, UserDataModels userData)
        {
            LaunchConfig settings = null;
            bool IsEnableIndependencyCore = false;
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += async (_, _) =>
            {
                settings = new LaunchConfig(); // 初始化启动配置
                #region 综合设置
                var locator = new GameCoreToolkit(Path is "" ? App.Data.FooterPath : Path);
                var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath,locator.GetGameCore(version).ToNatsurainkoGameCore());
                settings.JvmConfig = new(App.Data.JavaPath)
                {
                    MaxMemory = App.Data.Max,
                    AdvancedArguments = new List<string>() { App.Data.Jvm, IsYgg },
                };

                settings.GameWindowConfig = new()
                {
                    IsFullscreen = App.Data.IsFull,
                };

                if (App.Data.Isolate is true)
                    settings.WorkingFolder = new(PathConst.GetVersionFolder(Path is "" ? App.Data.FooterPath : Path, version));
                else
                    settings.WorkingFolder = new(Path is "" ? App.Data.FooterPath : Path);
                Debug.WriteLine(settings.WorkingFolder);
                IsEnableIndependencyCore = App.Data.Isolate;
                //如果不是windows就可以手动设置Natives目录
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    settings.NativesFolder = new(Path is "" ? App.Data.FooterPath : Path);
                if (res is not null && res.IsEnableIndependencyCore)
                {
                    //settings.EnableIndependencyCore = res.Isolate;
                    IsEnableIndependencyCore = res.Isolate;
                    settings.JvmConfig = new(App.Data.JavaPath)
                    {
                        MaxMemory = App.Data.Max,
                        AdvancedArguments = new List<string>() { res.Jvm, IsYgg },
                    };

                    settings.GameWindowConfig = new()
                    {
                        IsFullscreen = res.IsFullWindows,
                        Height = res.WindowHeight,
                        Width = res.WindowWidth
                    };
                    Trace.WriteLine("[Launch] 已启用独立游戏核心设置");
                    //if (res.Isolate is true)
                    //    settings.WorkingFolder = new(PathConst.GetVersionFolder(Path is "" ? App.Data.FooterPath : Path, version));
                    //else
                    //    settings.WorkingFolder = new(Path is "" ? App.Data.FooterPath : Path);
                }
                #endregion

                #region 账户设置
                if (userData.UserType is "微软账户")
                {
                    settings.Account = new MicrosoftAccount()
                    {
                        Name = userData.UserName,
                        AccessToken = userData.UserAccessToken,
                        Uuid = Guid.Parse(userData.UserUuid),
                        RefreshToken = userData.UserRefreshToken,
                    };
                }
                else if (userData.UserType is "离线账户")
                {
                    settings.Account = new OfflineAccount()
                    {
                        Name = userData.UserName,
                        AccessToken = userData.UserAccessToken,
                        Uuid = Guid.Parse(userData.UserUuid),
                    };
                }
                else
                {
                    if (!File.Exists($"{PathConst.MainDirectory}\\authlib-injector.jar"))
                    {
                        await HttpToolkit.HttpDownloadAsync("https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/45/authlib-injector-1.1.45.jar",
                            PathConst.MainDirectory, "authlib-injector.jar");
                    }
                    settings.JvmConfig.AdvancedArguments = new List<string>() { userData.AIJvm };
                    settings.JvmConfig.AdvancedArguments.ToList().ForEach(x => MainWindow.ShowInfoBarAsync(x));
                    settings.Account = new YggdrasilAccount()
                    {
                        Name = userData.UserName,
                        AccessToken = userData.UserAccessToken,
                        Uuid = Guid.Parse(userData.UserUuid),
                    };
                }
                #endregion

                #region 启动
                bool IsCanel = false;
                GameId = version;
                var launcher = new JavaClientLauncher(settings, locator, IsEnableIndependencyCore);
                using var response = await launcher.LaunchTaskAsync(version, x => Debug.WriteLine(x.Item2));
                LaunchResponse = response;
                response.ProcessOutput += Response_MinecraftProcessOutput;
                response.Exited += Response_MinecraftExited;
                if (response.State is MinecraftLaunch.Modules.Enum.LaunchState.Succeess) // 判断启动状态是否成功
                {
                    #region 记录时间
                    var core = new GameCoreToolkit(App.Data.FooterPath).GetGameCore(version);
                    JsonToolkit.ChangeEnableIndependencyCoreInfoJsonTime(App.Data.FooterPath, core.ToNatsurainkoGameCore(), JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, core.ToNatsurainkoGameCore()));
                    #endregion
                    await Dispatcher.UIThread.InvokeAsync(() => main.Description = "等待游戏窗口出现", DispatcherPriority.Background);
                    await Task.Run(response.Process.WaitForInputIdle);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        main.Description = "游戏运行中";
                    }, DispatcherPriority.Background);
                    GameProcess = response.Process;
                    MainView.ViewModel.AllTaskCount--;
                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        for (; ; )
                        {
                            if (IsCanel is false)
                            {
                                if (response.RunTime.Elapsed.Hours <= 9 && response.RunTime.Elapsed.Minutes <= 9 && response.RunTime.Elapsed.Seconds <= 9)
                                    time.Text = $"0{response.RunTime.Elapsed.Hours}:0{response.RunTime.Elapsed.Minutes}:0{response.RunTime.Elapsed.Seconds}";
                                else if (response.RunTime.Elapsed.Hours <= 9 && response.RunTime.Elapsed.Minutes <= 9 && response.RunTime.Elapsed.Seconds >= 9)
                                    time.Text = $"0{response.RunTime.Elapsed.Hours}:0{response.RunTime.Elapsed.Minutes}:{response.RunTime.Elapsed.Seconds}";
                                else if (response.RunTime.Elapsed.Hours <= 9 && response.RunTime.Elapsed.Minutes >= 9 && response.RunTime.Elapsed.Seconds >= 9)
                                    time.Text = $"0{response.RunTime.Elapsed.Hours}:{response.RunTime.Elapsed.Minutes}:{response.RunTime.Elapsed.Seconds}";
                                await Task.Run(() => { Thread.Sleep(1000); });
                            }
                            else
                                break;
                        }
                    });
                    await response.WaitForExitAsync();
                    IsCanel = true;
                }
                #endregion
            };
            backgroundWorker.RunWorkerAsync();
        }

        private void Response_MinecraftExited(object? sender, ExitedArgs e)
        {
            TaskBase.InvokeAsync(() =>
            {
                if (!e.Crashed || IsKill || (Window is not null && Window.IsKill))
                {
                    main.Description = $"游戏进程已退出";
                    Close.IsVisible = false;
                    gameout.IsVisible = true;
                    exitcode.Text = $"退出码：{e.ExitCode}";
                }
                else
                {
                    main.Description = $"游戏进程已退出";
                    Close.IsVisible = false;
                    gameout.IsVisible = true;
                    exitcode.Text = $"退出码：{e.ExitCode}";
                    GameCrashAnalyzer analyzer = new(Logs);
                    IsKill = false;
                    var res = analyzer.AnalyseAsync().Keys.ToList();
                    Debug.WriteLine($"[Launch] 导致游戏崩溃的可能的异常有 {res.Count} 个");
                    Debug.WriteLine($"[Launch] {string.Join("\n", res)}");
                }
            });
        }

        private void Response_MinecraftProcessOutput(object? sender, IProcessOutput e)
        {
            //Window.ViewModel.Logs.Add(new() { Source = e.Raw });
            Logs.Add(e.Raw);
            Debug.WriteLine($"[Game Log] {e.Raw}");
            //IBrush color = default;
            ////日志等级判断
            //var level = GameLogToolkit.GameLogParSing(e.Raw);
            //if(level.LogType is LogType.Error)
            //    color = Brushes.Orange;
            //else if(level.LogType is LogType.Warning)
            //    color = Brushes.Yellow;
            //else if(level.LogType is LogType.Info)
            //    color = Brushes.Green;
            //else if(level.LogType is LogType.Fatal)
            //    color = Brushes.Red;
            //else if(level.LogType is LogType.Unknown)
            //    color = Brushes.Green;

            //if (ConsoleView.console is null)
            //{
            //    ConsoleView.logModels.Add(new()
            //    {
            //        Log = e.Raw,
            //        LogLevel = color
            //    });
            //}
            //else
            //{
            //    ConsoleView.console.AddLog(new()
            //    {
            //        Log = e.Raw,
            //        LogLevel = color,
            //    });
            //}
        }

        public void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TaskView.Remove(this);
        }

        public bool IsKill { get; set; } = false;

        string GameId = string.Empty;

        JavaClientLaunchResponse LaunchResponse = null;

        public List<string> Logs = new();

        public string IsYgg
        {
            get
            {
                return "";
            }
        }
    }
}
