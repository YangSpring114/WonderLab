using Avalonia;
using Microsoft.CodeAnalysis;
using MinecraftLaunch.Launch;
using MinecraftLaunch.Modules.Enum;
using MinecraftLaunch.Modules.Installer;
using MinecraftLaunch.Modules.Interface;
using MinecraftLaunch.Modules.Models.Auth;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Enum;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public partial class LaunchItemViewModel : ViewModelBase
    {
        public string Title { get => _Title; set => RaiseAndSetIfChanged(ref _Title, value); }

        public string State { get => _State; set => RaiseAndSetIfChanged(ref _State, value); }

        public string RunTime { get => _RunTime; set => RaiseAndSetIfChanged(ref _RunTime, value); }

        public string ExitCode { get => _ExitCode; set => RaiseAndSetIfChanged(ref _ExitCode, value); }

        public Thickness TimerMargin { get => _TimerMargin; set => RaiseAndSetIfChanged(ref _TimerMargin, value); }

        public bool IsLaunchOk { get => _IsLaunchOk; set => RaiseAndSetIfChanged(ref _IsLaunchOk, value); }

        public bool IsWindowsLoadOk { get => _IsWindowsLoadOk; set => RaiseAndSetIfChanged(ref _IsWindowsLoadOk, value); }

        public bool IsClose { get => _IsClose; set => RaiseAndSetIfChanged(ref _IsClose, value); }

        public bool IsCanGoToConsole { get => _IsCanGoToConsole; set => RaiseAndSetIfChanged(ref _IsCanGoToConsole, value); }
    }

    partial class LaunchItemViewModel
    {
        /// <summary>
        /// 游戏结束事件
        /// </summary>
        public void GameKillAction()
        {
            if (!GameProcess.HasExited)
                GameProcess.Kill();
        }

        /// <summary>
        /// 游戏启动事件
        /// </summary>
        public void GameLaunchAction()
        {
            BackgroundWorker worker = new();
            worker.DoWork += async (_, _) =>
            {
                var IsCompletedSuccess = await ResourceCompletionedAction();

                if (!IsCompletedSuccess) {                    
                    return;
                }

                Title = $"游戏核心 {GameCore.Id} 的启动任务";
                bool IsEnableIndependencyCore = false;
                var setting = new LaunchConfig();
                var toolkit = new GameCoreToolkit(App.Data.FooterPath);
                var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, GameCore.Id!);

                setting.Account = Account;
                if (setting.Account.Type is AccountType.Yggdrasil)
                {
                    if (!File.Exists(Path.Combine(PathConst.TempDirectory, "authlib-injector.jar")))
                    {
                        await HttpToolkit.HttpDownloadAsync("https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/45/authlib-injector-1.1.45.jar",
                            PathConst.TempDirectory, "authlib-injector.jar");
                    }
                    //setting.JvmConfig.AdvancedArguments = new List<string>() { viewModel.Account.AIJvm };
                }

                setting.JvmConfig = new(App.Data.JavaPath)
                {
                    MaxMemory = App.Data.Max,
                    AdvancedArguments = new List<string>() { App.Data.Jvm },
                };

                setting.GameWindowConfig = new()
                {
                    IsFullscreen = App.Data.IsFull,
                };

                JavaClientLauncher launcher = new(setting, toolkit, App.Data.Isolate);

                if (IndependencyCoreData is not null && IndependencyCoreData.IsEnableIndependencyCore)
                {
                    IsEnableIndependencyCore = IndependencyCoreData.Isolate;
                    setting.JvmConfig = new(App.Data.JavaPath)
                    {
                        MaxMemory = App.Data.Max,
                        AdvancedArguments = new List<string>() { IndependencyCoreData.Jvm },
                    };

                    setting.GameWindowConfig = new()
                    {
                        IsFullscreen = IndependencyCoreData.IsFullWindows,
                        Height = IndependencyCoreData.WindowHeight,
                        Width = IndependencyCoreData.WindowWidth
                    };
                    launcher = new(setting, toolkit, IsEnableIndependencyCore);
                    Trace.WriteLine("[Launch] 已启用独立游戏核心设置");
                }

                JsonToolkit.JsonWrite();
                var res = await launcher.LaunchTaskAsync(GameCore.Id!);

                if (res.State is LaunchState.Succeess)
                {
                    LaunchTime = DateTime.Now;
                    res.Exited += GameExitedAction;
                    res.ProcessOutput += ProcessOutputAction;
                    GameProcess = res.Process;
                    State = "游戏运行中";
                    IsLaunchOk = true;
                    await Task.Run(GameProcess.WaitForInputIdle);
                    TimerMargin = new(0, 25, 0, 0);
                    IsWindowsLoadOk = true;
                    IsCanGoToConsole = true;
                }
                else if (res.State is LaunchState.Failed)
                {
                    LaunchFailedAction(LaunchFailedType.LaunchFailed, res.Exception);
                }

                GameRunTimer = new(1000);
                GameRunTimer.Start();
                GameRunTimer.Elapsed += GameRunTimerElapsed;
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// 游戏资源补全事件
        /// </summary>
        public async ValueTask<bool> ResourceCompletionedAction()
        {
            try
            {
                ResourceInstaller installer = new(GameCore);
                var res = await installer.DownloadAsync((x, _) =>
                {
                    State = $"资源补全进度：{x}";
                });

                State = "资源补全完成";
                return true;
            }
            catch (Exception)
            {
                LaunchFailedAction(LaunchFailedType.CompletionedFailed);
                return false;
            }
        }

        /// <summary>
        /// 游戏启动失败事件
        /// </summary>
        public void LaunchFailedAction(LaunchFailedType failedType, Exception ex = null)
        {
            if (failedType is LaunchFailedType.LaunchFailed)
            {
                MainWindow.ShowInfoBarAsync("启动失败：", $"游戏在启动的过程中遭遇了不可描述的异常，信息如下：\n{ex}");
            }
            else if(failedType is LaunchFailedType.CompletionedFailed)
            {
                MainWindow.ShowInfoBarAsync("启动失败：", $"游戏在 资源补全 的过程中遭遇了不可描述的异常，请检查您的网络并尝试重新启动");
            }
            else
            {
                MainWindow.ShowInfoBarAsync("启动失败：",$"未知异常，信息已提交至 终端记录仪");
            }
        }

        /// <summary>
        /// 游戏运行时间记录事件
        /// </summary>
        public void GameRunTimerElapsed(object? sender, ElapsedEventArgs e) => RunTime = (DateTime.Now - LaunchTime).ToString(@"hh\:mm\:ss");

        /// <summary>
        /// 游戏退出事件
        /// </summary>
        public void GameExitedAction(object? sender, MinecraftLaunch.Events.ExitedArgs e)
        {
            GameRunTimer.Stop();
            State = "游戏进程已退出";
            IsClose = true;
            IsWindowsLoadOk = false;
            ExitCode = $"退出码为 {e.ExitCode}";
            if (e.Crashed)
            {

            }
        }

        /// <summary>
        /// 游戏日志输出事件
        /// </summary>
        public void ProcessOutputAction(object? sender, IProcessOutput e)
        {
            Trace.WriteLine(e.Raw);
        }
    }

    partial class LaunchItemViewModel
    {
        public LaunchItemViewModel(GameCore core,Account account)
        {
            GameCore = core;
            Account = account;
        }
        public Timer GameRunTimer = null;
        public DateTime LaunchTime;
        public GameCore GameCore = null;
        public Account Account = null;
        public Process GameProcess = null;
        private Thickness _TimerMargin = new(0);
        private string _Title = string.Empty;
        private string _ExitCode = string.Empty;
        private bool _IsLaunchOk = false;
        private bool _IsClose = false;
        private bool _IsWindowsLoadOk = false;
        private bool _IsCanGoToConsole = false;
        private string _RunTime = "00:00:00";
        private string _State = "准备启动游戏核心";
    }
}
