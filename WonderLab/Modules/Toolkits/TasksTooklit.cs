using Avalonia.Controls;
using Avalonia.OpenGL;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Launch;
using MinecraftLaunch.Modules.Enum;
using MinecraftLaunch.Modules.Interface;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.Toolkits.Network.Model;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Enum;
using WonderLab.Modules.Models;
using WonderLab.ViewModels;
using WonderLab.Views;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 任务操作工具类
    /// </summary>
    public class TasksTooklit
    {
        /// <summary>
        /// 创建游戏进程任务
        /// </summary>
        /// <param name="core"></param>
        public static async ValueTask<JavaClientLaunchResponse> CreateGameLaunchTask(GameCore core, LaunchItemViewModel viewModel)
        {
            Trace.WriteLine($"游戏核心 {core.Id} 的启动任务");
            viewModel.Title = $"游戏核心 {core.Id} 的启动任务";
            bool IsEnableIndependencyCore = false;
            var setting = new LaunchConfig();
            var toolkit = new GameCoreToolkit(App.Data.FooterPath);
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, core.Id!);

            setting.JvmConfig = new(App.Data.JavaPath)
            {
                MaxMemory = App.Data.Max,
                AdvancedArguments = new List<string>() { App.Data.Jvm },
            };

            setting.GameWindowConfig = new()
            {
                IsFullscreen = App.Data.IsFull,
            };

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
                Trace.WriteLine("[Launch] 已启用独立游戏核心设置");
            }

            setting.Account = viewModel.Account;
            if (setting.Account.Type is AccountType.Yggdrasil)
            {                
                if (!File.Exists(Path.Combine(PathConst.TempDirectory, "authlib-injector.jar")))
                {
                    await HttpToolkit.HttpDownloadAsync("https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/45/authlib-injector-1.1.45.jar",
                        PathConst.TempDirectory, "authlib-injector.jar");
                }
                //setting.JvmConfig.AdvancedArguments = new List<string>() { viewModel.Account.AIJvm };
            }

            JsonToolkit.JsonWrite();
            JavaClientLauncher launcher = new(setting, toolkit, IsEnableIndependencyCore);
            var res = await launcher.LaunchTaskAsync(core.Id!);

            if (res.State is LaunchState.Succeess)
            {
                viewModel.LaunchTime = DateTime.Now;
                viewModel.State = "游戏运行中";
            }
            else if (res.State is LaunchState.Failed)
            {
                viewModel.LaunchFailedAction(LaunchFailedType.LaunchFailed, res.Exception);
            }
            return null;
        }

        /// <summary>
        /// 创建模组下载任务
        /// </summary>
        /// <param name="curseForgeModpack"></param>
        /// <param name="control"></param>
        public static void CreateModDownloadTask(CurseForgeModel curseForgeModpack, Control control,string save = "")
        {
            control.IsEnabled = false;

            FileInfo info = null;
            string filename = curseForgeModpack.CurrentFileInfo.FileName;
            if (string.IsNullOrEmpty(App.Data.FooterPath))
                return;

            var folder = App.CurrentGameCore != null
                ? PathConst.GetVersionModsFolder(App.Data.FooterPath, App.CurrentGameCore.Id)
                : Path.Combine(App.Data.FooterPath, "mods");

            if (!string.IsNullOrEmpty(save)) {
                info = new FileInfo(save);
                folder = info.Directory.FullName;
                filename = info.Name;
            }

            var downloaderProcess = new HttpDownloadRequest
            {
                Directory = new DirectoryInfo(folder),
                FileName = filename,
                Url = curseForgeModpack.CurrentFileInfo.DownloadUrl
            };

            DownItemView downItemView1 = new(downloaderProcess);
            TaskView.Add(downItemView1);

            var hyperlinkButton = new HyperlinkButton { Content = "转至 祝福终端>任务中心"  };
            hyperlinkButton.Click += (_, _) => Page.NavigatedToTaskView();

            MainWindow.ShowInfoBarAsync("成功",$"已将 \"{curseForgeModpack.CurrentFileInfo.FileName}\" 添加至下载队列",button: hyperlinkButton);     
            MainView.ViewModel.AllTaskCount++;
            control.IsEnabled = true;
        }

        /// <summary>
        /// 创建 Java 安装任务
        /// </summary>
        /// <param name="s"></param>
        /// <param name="control"></param>
        public static void CreateJavaInstallTask(string s, Control control)
        {
            control.IsEnabled = false;

            if (string.IsNullOrEmpty(App.Data.FooterPath))
                return;

            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Java");
            Debug.WriteLine(folder);
            DownItemView downItemView1 = new(s);
            TaskView.Add(downItemView1);

            var hyperlinkButton = new HyperlinkButton { Content = "转至 祝福终端>任务中心" };
            hyperlinkButton.Click += (_, _) => Page.NavigatedToTaskView();

            MainWindow.ShowInfoBarAsync("成功", $"已将 \"{Path.GetFileName(s)}\" 添加至下载队列", button: hyperlinkButton);
            MainView.ViewModel.AllTaskCount++;
            control.IsEnabled = true;
        }

        /// <summary>
        /// 创建 启动脚本
        /// </summary>
        public async static void CreateLaunchScript(GameCore core)
        {
            var setting = new LaunchConfig();
            var toolkit = new GameCoreToolkit(App.Data.FooterPath);
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, core.Id!);
            bool IsEnableIndependencyCore = false;

            setting.JvmConfig = new(App.Data.JavaPath)
            {
                MaxMemory = App.Data.Max,
                AdvancedArguments = new List<string>() { App.Data.Jvm },
            };

            setting.GameWindowConfig = new()
            {
                IsFullscreen = App.Data.IsFull,
            };

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
                Trace.WriteLine("[Launch] 已启用独立游戏核心设置");
            }

            setting.Account = App.Data.SelectedUser.ToAccount();
            if (setting.Account.Type is AccountType.Yggdrasil)
            {
                if (!File.Exists(Path.Combine(PathConst.TempDirectory, "authlib-injector.jar")))
                {
                    await HttpToolkit.HttpDownloadAsync("https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/45/authlib-injector-1.1.45.jar",
                        PathConst.TempDirectory, "authlib-injector.jar");
                }
                setting.JvmConfig.AdvancedArguments = new List<string>() { App.Data.SelectedUser.AIJvm };
            }

            ScriptToolkit.LaunchScriptBuildAsync("C:\\Users\\w\\Desktop\\test.bat", core, setting, true);
        }
    }
}
