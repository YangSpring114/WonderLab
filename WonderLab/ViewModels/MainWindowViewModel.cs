using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Natsurainko.Toolkits.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;

namespace WonderLab.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<InfoBarModel> InfoBarItems
        {
            get => _InfoBarItems;
            set => RaiseAndSetIfChanged(ref _InfoBarItems, value);
        }

        public bool IsStartInstall
        {
            get => _IsStartInstall;
            set => RaiseAndSetIfChanged(ref _IsStartInstall, value);
        }

        public bool IsStart
        {
            get => _IsStart;
            set => RaiseAndSetIfChanged(ref _IsStart, value);
        }

        public float Progress
        {
            get => _Progress;
            set => RaiseAndSetIfChanged(ref _Progress, value);
        }

        public string StringProgress
        {
            get => _StringProgress;
            set => RaiseAndSetIfChanged(ref _StringProgress, value);
        }

        public string Time
        {
            get => _Time;
            set => RaiseAndSetIfChanged(ref _Time, value);
        }

        public static MainWindowViewModel ViewModel;

        public static TitleBar TitleBar { get; set; }

        public Window Window;

        public MainWindowViewModel(Window window)
        {
            Window = window;
            ViewModel = this;
        }

        public void Colse() => TitleBar.OnClose();
        public void MaxWindowSize() => TitleBar.OnRestore();
        public void MiniWindowSize() => TitleBar.OnMinimize();

        /// <summary>
        /// 光影包拖拽安装方法
        /// </summary>
        public void ShaderPacksInstallAction() {
            if (string.IsNullOrEmpty(App.Data.FooterPath) || string.IsNullOrEmpty(App.Data.SelectedGameCore)) {
                MainWindow.win.CloseAnimaction();
                MainWindow.ShowInfoBarAsync("错误", "您没有选择游戏目录或游戏核心，无法继续安装光影包", InfoBarSeverity.Error);
                return;
            }

            var saveDirpath = PathConst.GetShaderPacksFolder(App.Data.FooterPath, App.Data.SelectedGameCore!);
            Trace.WriteLine($"[信息]光影包将被保存的目录为 {saveDirpath}");

            try {
                if (!Directory.Exists(saveDirpath)) {
                    Directory.CreateDirectory(saveDirpath);
                }

                File.Copy(MainWindow.win.FileNames.First(),
                          Path.Combine(saveDirpath, Path.GetFileName(MainWindow.win.FileNames.First())), true);

                MainWindow.ShowInfoBarAsync("成功", $"光影包已成功安装至选择的游戏核心！", InfoBarSeverity.Success);

                if (MainWindow.win.FileNames.Count > 1) {
                    MainWindow.ShowInfoBarAsync("提示", "您拖入了多个文件，WonderLab会选取第一个安装，余下需重复此操作！");
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误", $"WonderLab 在安装光影包时出现了一些不可描述的异常，异常堆栈如下：\n{ex}", InfoBarSeverity.Error);
            }finally {
                MainWindow.win.CloseAnimaction();
            }
        }

        /// <summary>
        /// 材质包拖拽安装方法
        /// </summary>
        public void ResourcePacksInstallAction()
        {
            if (string.IsNullOrEmpty(App.Data.FooterPath) || string.IsNullOrEmpty(App.Data.SelectedGameCore))
            {
                MainWindow.win.CloseAnimaction();
                MainWindow.ShowInfoBarAsync("错误", "您没有选择游戏目录或游戏核心，无法继续安装材质包", InfoBarSeverity.Error);
                return;
            }

            var saveDirpath = PathConst.GetResourcePacksFolder(App.Data.FooterPath, App.Data.SelectedGameCore!);
            Trace.WriteLine($"[信息]材质包将被保存的目录为 {saveDirpath}");

            try
            {
                if (!Directory.Exists(saveDirpath))
                {
                    Directory.CreateDirectory(saveDirpath);
                }

                File.Copy(MainWindow.win.FileNames.First(),
                          Path.Combine(saveDirpath, Path.GetFileName(MainWindow.win.FileNames.First())), true);

                MainWindow.ShowInfoBarAsync("成功", $"材质包已成功安装至选择的游戏核心！", InfoBarSeverity.Success);

                if (MainWindow.win.FileNames.Count > 1)
                {
                    MainWindow.ShowInfoBarAsync("提示", "您拖入了多个文件，WonderLab会选取第一个安装，余下需重复此操作！");
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误", $"WonderLab 在安装材质包时出现了一些不可描述的异常，异常堆栈如下：\n{ex}", InfoBarSeverity.Error);
            }
            finally
            {
                MainWindow.win.CloseAnimaction();
            }
        }

        /// <summary>
        /// 模组包拖拽安装方法
        /// </summary>
        public void ModnstallAction()
        {
            if (string.IsNullOrEmpty(App.Data.FooterPath) || string.IsNullOrEmpty(App.Data.SelectedGameCore))
            {
                MainWindow.win.CloseAnimaction();
                MainWindow.ShowInfoBarAsync("错误", "您没有选择游戏目录或游戏核心，无法继续安装模组", InfoBarSeverity.Error);
                return;
            }

            var saveDirpath = PathConst.GetModsFolder(App.Data.FooterPath, App.Data.SelectedGameCore!);
            Trace.WriteLine($"[信息]模组将被保存的目录为 {saveDirpath}");

            try
            {
                if (!Directory.Exists(saveDirpath))
                {
                    Directory.CreateDirectory(saveDirpath);
                }

                File.Copy(MainWindow.win.FileNames.First(),
                          Path.Combine(saveDirpath, Path.GetFileName(MainWindow.win.FileNames.First())), true);

                MainWindow.ShowInfoBarAsync("成功", $"模组已成功安装至选择的游戏核心！", InfoBarSeverity.Success);

                if (MainWindow.win.FileNames.Count > 1)
                {
                    MainWindow.ShowInfoBarAsync("提示", "您拖入了多个文件，WonderLab会选取第一个安装，余下需重复此操作！");
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误", $"WonderLab 在安装模组时出现了一些不可描述的异常，异常堆栈如下：\n{ex}", InfoBarSeverity.Error);
            }
            finally
            {
                MainWindow.win.CloseAnimaction();
            }
        }
        private int _Width = 1000;
        private int _Height = 600;
        public int Width
        {
            get => _Width;
        }
        public int Height
        {
            get => _Height;
        }

        public async void Install()
        {
            IsStart = false;
            IsStartInstall= true;
            InstallTime = DateTime.Now;
            Timer timer = new(1000);
            timer.Elapsed += TimerElapsed;
            timer.Start();
            await Task.Delay(500);

            var res = await HttpWrapper.HttpDownloadAsync(MainWindow.win.UpdateInfo.Value, Environment.CurrentDirectory, (x, e) =>
            {
                StringProgress = $"{Math.Round(x * 100, 2)}%";
                Progress = x;
            }, "WonderLab.update");

            if (res.HttpStatusCode is System.Net.HttpStatusCode.OK) {
                StringProgress = "100%";
                Progress = 1f;
                timer.Stop();

                //安装更新
                int currentPID = Process.GetCurrentProcess().Id;
                string name = Process.GetCurrentProcess().ProcessName;
                string filename = $"{name}.dll";

                string psCommand =
                            $"Stop-Process -Id {currentPID} -Force;" +
                            $"Wait-Process -Id {currentPID} -ErrorAction SilentlyContinue;" +
                            "Start-Sleep -Milliseconds 500;" +
                            $"Remove-Item {filename} -Force;" +
                            $"Rename-Item WonderLab.update {filename};" +
                            $"Start-Process {name}.exe -Args updated;";
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = psCommand,
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                    });
                }
                catch (Exception ex)
                {}                
            }
        }

        private void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Time = (DateTime.Now - InstallTime).ToString(@"hh\:mm\:ss");
        }
    }

    partial class MainWindowViewModel
    {
        private ObservableCollection<InfoBarModel> _InfoBarItems = new();
        public DateTime InstallTime;
        public bool _IsStart = true;
        public bool _IsStartInstall = false;
        public float _Progress = 0f;
        public string _StringProgress = "0%";
        public string _Time = "00:00:00";
    }
}
