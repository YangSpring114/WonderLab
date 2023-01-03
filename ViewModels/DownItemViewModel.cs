using Avalonia.Controls;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using JetBrains.Annotations;
using MinecraftLaunch.Modules.Enum;
using MinecraftLaunch.Modules.Installer;
using MinecraftLaunch.Modules.Models.Install;
using Natsurainko.FluentCore.Class.Model.Install;
using Natsurainko.FluentCore.Module.Installer;
using Natsurainko.FluentCore.Module.Launcher;
using Natsurainko.Toolkits.Network;
using Natsurainko.Toolkits.Network.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Enum;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;
using static System.Net.WebRequestMethods;
using ModLoaderType = Natsurainko.FluentCore.Class.Model.Install.ModLoaderType;

namespace WonderLab.ViewModels
{
    public partial class DownItemViewModel : ViewModelBase
    {
        public DownItemViewModel(string Id , DownType Type)
        {
            TaskTitle = $"游戏核心 {Id} 安装任务";
            if (Type is DownType.Vanllia)
            {
                VanlliaInstall(Id);
            }
            else if (Type is DownType.Mod)
            {

            }
            else if (Type is DownType.Java)
            {

            }
        }
        
        public DownItemViewModel(List<ModLoaderInformationViewData> ids)
        {
            ModLoaderInformationViewData om = null;
            ModLoaderInformationViewData fm = null;
            ModLoaderInformationViewData fam = null;
            //TaskTitle = $"游戏核心 {"{mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}_{mlimvd.Data.Version}"} 安装任务";
            ids.ForEach(x =>
            {
                if (x.Data.LoaderType is ModLoaderType.Forge)
                    fm = x;
                else if (x.Data.LoaderType is ModLoaderType.Fabric)
                    fam = x;
                else if (x.Data.LoaderType is ModLoaderType.OptiFine)
                    om = x;
            });

            if (fm is null && om is not null)
            {
                OptiFineInstall(om);
            }
            else if(fm is not null && om is not null)
            {
                ForgeOptiFineInstall(fm, om);
            }
            else if(om is null && fm is not null)
            {
                ForgeInstall(fm);
            }
            else if(fam is not null)
            {
                FabricInstall(fam);
            }
        }

        public DownItemViewModel(ModLoaderInformationViewData modLoaderInformationViewData)
        {
            TaskTitle = $"游戏核心 {modLoaderInformationViewData.Data.McVersion}-{modLoaderInformationViewData.Data.LoaderName}{modLoaderInformationViewData.Data.Version} 安装任务";
            if (modLoaderInformationViewData.Data.LoaderType is ModLoaderType.Forge)
            {
                ForgeInstall(modLoaderInformationViewData);
            }
            else if (modLoaderInformationViewData.Data.LoaderType is ModLoaderType.OptiFine)
            {
                OptiFineInstall(modLoaderInformationViewData);
            }
            else if (modLoaderInformationViewData.Data.LoaderType is ModLoaderType.Fabric)
            {
                FabricInstall(modLoaderInformationViewData);
            }
        }

        public DownItemViewModel(HttpDownloadRequest http)
        {
            ModInstall(http);
        }

        public DownItemViewModel(string http)
        {
            JavaInstall(http);
        }

        public void OpenFile()
        {
            try
            {
                using var res = Process.Start(new ProcessStartInfo(FilePath)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误",$"异常堆栈信息：{ex}", FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
            }
        }
        //Vanllia
        private async void VanlliaInstall(string Id)
        {
            int count = 0;
            int xlcount = 0;
            MainView.ViewModel.AllTaskCount++;
            Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
            await Task.Delay(4000);
            Dispatcher.UIThread.Post(() => IsLoadOk = false);
            Dispatcher.UIThread.Post(() => TaskProgress = 10);
            await Task.Run(async() =>
            {
                GameCoreInstaller installer = new(new(App.Data.FooterPath), Id);
                var returninfo = await installer.InstallAsync(async e =>
                {
                    if (count == 10)//尝试限流，一秒只更新十次
                    {
                        xlcount++;
                        count = 0;
                        Dispatcher.UIThread.Post(() =>
                        {
                            LittleTaskProgress = e.Item2;
                            MainTaskProgress = e.Item2;
                        });
                        Debug.WriteLine($"已限流 {xlcount} 次");
                        await Task.Run(() => Thread.Sleep(1000));
                    }
                    else
                    {
                        count++;
                        Dispatcher.UIThread.Post(() => TaskProgress = e.Item1);
                        //TaskProgress = e.Item1 * 100;
                    }
                });
                if (returninfo.Success)
                {
                    await Task.Delay(1000);
                    LittleTaskProgress = "已完成";
                    MainTaskProgress = "安装成功";
                }
            }); 
            MainView.ViewModel.AllTaskCount--;
            LittleTaskProgress = "已完成";
            MainTaskProgress = "安装成功";
        }
        //Forge
        private async void ForgeInstall(ModLoaderInformationViewData mlimvd)
        {
            MainView.ViewModel.AllTaskCount++;
            TaskTitle = $"游戏核心 {mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}{mlimvd.Data.Version} 安装任务";
            Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
            await Task.Delay(4000);
            Dispatcher.UIThread.Post(() => IsLoadOk = false);
            await Task.Run(async() =>
            {
                ForgeInstaller forgeInstaller = new(new(App.Data.FooterPath), (ForgeInstallEntity)mlimvd.Build, App.Data.JavaPath,customId:$"{mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}{mlimvd.Data.Version}");
                var res = await forgeInstaller.InstallAsync(x =>
                {
                    //TaskProgress = x.Item1 * 100;
                    Dispatcher.UIThread.Post(() => TaskProgress = x.Item1);
                    Dispatcher.UIThread.Post(() =>
                    {
                        LittleTaskProgress = x.Item2;
                        MainTaskProgress = x.Item2;
                    });
                });

                if (res.Success)
                {
                    LittleTaskProgress = "安装完成";
                    MainTaskProgress = "安装完成";
                    Dispatcher.UIThread.Post(() => TaskProgress = 1);
                    JsonToolkit.CreaftEnableIndependencyCoreInfoJson(App.Data.FooterPath,new GameCoreLocator(App.Data.FooterPath).GetGameCore(res.GameCore.Id),DownGameView.ViewModel.IsEnableIndependencyCore);
                }
                else if (res.Exception is not null)
                    MainWindow.ShowInfoBarAsync("失败：", $"未能成功安装 {mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}{mlimvd.Data.Version}，详细信息：{res.Exception}", InfoBarSeverity.Error);
            });
            MainView.ViewModel.AllTaskCount--;
        }
        //OptiFine
        private async void OptiFineInstall(ModLoaderInformationViewData mlimvd)
        {
            MainView.ViewModel.AllTaskCount++;
            TaskTitle = $"游戏核心 {mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}_{mlimvd.Data.Version} 安装任务";
            Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
            await Task.Delay(4000);
            Dispatcher.UIThread.Post(() => IsLoadOk = false);
            await Task.Run(async () =>
            {
                OptiFineInstaller oi = new(new(App.Data.FooterPath), (OptiFineInstallEntity)mlimvd.Build, App.Data.JavaPath,customId:$"{mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}_{mlimvd.Data.Version}");
                var res = await oi.InstallAsync(async x =>
                {
                    Dispatcher.UIThread.Post(() => TaskProgress = x.Item1);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        LittleTaskProgress = x.Item2;
                        MainTaskProgress = x.Item2;
                    });
                });

                if (res.Success)
                {
                    Dispatcher.UIThread.Post(() => TaskProgress = 1);
                    LittleTaskProgress = "已完成";
                    MainTaskProgress = "安装成功";
                    JsonToolkit.CreaftEnableIndependencyCoreInfoJson(App.Data.FooterPath, new GameCoreLocator(App.Data.FooterPath).GetGameCore(res.GameCore.Id), DownGameView.ViewModel.IsEnableIndependencyCore);
                    MainView.ViewModel.AllTaskCount--;
                }
            });
        }
        //Forge + OptiFine
        private async void ForgeOptiFineInstall(ModLoaderInformationViewData fm, ModLoaderInformationViewData om)
        {
            MainView.ViewModel.AllTaskCount++;
            TaskTitle = $"游戏核心 {fm.Data.McVersion}-{fm.Data.LoaderName}{fm.Data.Version}-{om.Data.LoaderName}_{om.Data.Version} 安装任务";
            Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
            await Task.Delay(4000);
            Dispatcher.UIThread.Post(() => IsLoadOk = false);
            await Task.Run(async () =>
            {
                ForgeInstaller forgeInstaller = new(new(App.Data.FooterPath), (ForgeInstallEntity)fm.Build, App.Data.JavaPath, customId: $"{fm.Data.McVersion}-{fm.Data.LoaderName}{fm.Data.Version}-{om.Data.LoaderName}_{om.Data.Version}");
                var res = await forgeInstaller.InstallAsync(async x =>
                {
                    Dispatcher.UIThread.Post(() => TaskProgress = x.Item1);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        LittleTaskProgress = x.Item2;
                        MainTaskProgress = x.Item2;
                    });
                });

                if (res.Success)
                {
                    OptiFineInstaller oi = new(new(App.Data.FooterPath), (OptiFineInstallEntity)om.Build, App.Data.JavaPath, customId: $"{fm.Data.McVersion}-{fm.Data.LoaderName}{fm.Data.Version}-{om.Data.LoaderName}_{om.Data.Version}");
                    var res1 = await oi.InstallAsync(async x =>
                    {
                        Dispatcher.UIThread.Post(() => TaskProgress = 1);
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            LittleTaskProgress = x.Item2;
                            MainTaskProgress = x.Item2;
                        });
                    });

                    if (res.Success)
                    {
                        JsonToolkit.CreaftEnableIndependencyCoreInfoJson(App.Data.FooterPath, new GameCoreLocator(App.Data.FooterPath).GetGameCore(res1.GameCore.Id), DownGameView.ViewModel.IsEnableIndependencyCore);
                    }
                }
            });
            MainView.ViewModel.AllTaskCount--;
        }
        //Fabric
        private async void FabricInstall(ModLoaderInformationViewData mlimvd)
        {
            MainView.ViewModel.AllTaskCount++;
            TaskTitle = $"游戏核心 {mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}_{mlimvd.Data.Version} 安装任务";
            Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
            await Task.Delay(4000);
            Dispatcher.UIThread.Post(() => IsLoadOk = false);
            await Task.Run(async () =>
            {
                FabricInstaller fi = new(new(App.Data.FooterPath), (FabricInstallBuild)mlimvd.Build,$"{mlimvd.Data.McVersion}-{mlimvd.Data.LoaderName}_{mlimvd.Data.Version}");
                var res = await fi.InstallAsync(async x =>
                {
                    Dispatcher.UIThread.Post(() => TaskProgress = x.Item1);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        LittleTaskProgress = x.Item2;
                        MainTaskProgress = x.Item2;
                    });
                });

                if (res.Success)
                {
                    JsonToolkit.CreaftEnableIndependencyCoreInfoJson(App.Data.FooterPath, new GameCoreLocator(App.Data.FooterPath).GetGameCore(res.GameCore.Id), DownGameView.ViewModel.IsEnableIndependencyCore);
                }
            });
            MainView.ViewModel.AllTaskCount--;
        }
        //Mod
        private async void ModInstall(HttpDownloadRequest http)
        {
            try
            {
                if (App.CurrentGameCore is not null)
                {
                    TaskTitle = $"模组 {http.FileName} 安装任务";
                    Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
                    await Task.Delay(2000);
                    Dispatcher.UIThread.Post(() => IsLoadOk = false);
                    FileLink = http.Url;
                    IsFileLinkVisible = true;
                    var res = await HttpWrapper.HttpDownloadAsync(http, (p, s) =>
                    {
                        Dispatcher.UIThread.Post(() => TaskProgress = p);
                        Dispatcher.UIThread.Post(() =>
                        {
                            LittleTaskProgress = s;
                            MainTaskProgress = s;
                        });
                    });

                    if (res.HttpStatusCode is HttpStatusCode.OK || res.FileInfo.Exists)
                    {
                        LittleTaskProgress = "安装完成";
                        MainTaskProgress = "已完成";
                        Dispatcher.UIThread.Post(() => TaskProgress = 1);
                        IsFileOpenVisible = true;
                        FilePath = res.FileInfo.Directory.FullName;
                    }
                }
            }
            catch { }
            MainView.ViewModel.AllTaskCount--;
        }
        //Java
        private async void JavaInstall(string url)
        {
            TaskTitle = $"Java 运行时 {Path.GetFileName(url)} 安装任务";
            Dispatcher.UIThread.Post(() => IsLoadOk = true);//先静等一下再开始下载，不然这sb进度条要炸
            await Task.Delay(4000);
            Dispatcher.UIThread.Post(() => IsLoadOk = false);
            FileLink = url;
            IsFileLinkVisible = true;
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Java");
            JavaInstaller javaInstaller = null;
            if (url.IndexOf("download.java.net") is not -1)
            {
                if (url.IndexOf("openjdk-8u42-b03") is not -1)
                {
                    javaInstaller = new(JdkDownloadSource.JdkJavaNet, OpenJdkType.OpenJdk8,folder);
                }

                if (url.IndexOf("openjdk-11+28") is not -1)
                {
                    javaInstaller = new(JdkDownloadSource.JdkJavaNet, OpenJdkType.OpenJdk11, folder);
                }

                if (url.IndexOf("openjdk-17+35") is not -1)
                {
                    javaInstaller = new(JdkDownloadSource.JdkJavaNet, OpenJdkType.OpenJdk17, folder);
                }

                if (url.IndexOf("openjdk-18+36") is not -1)
                {
                    javaInstaller = new(JdkDownloadSource.JdkJavaNet, OpenJdkType.OpenJdk18, folder);
                }
            }
            else
            {
                if (url.IndexOf("microsoft-jdk-17.0.4") is not -1)
                {
                    javaInstaller = new(JdkDownloadSource.Microsoft, OpenJdkType.OpenJdk17, folder);
                }

                if (url.IndexOf("microsoft-jdk-11.0.16") is not -1)
                {
                    javaInstaller = new(JdkDownloadSource.Microsoft, OpenJdkType.OpenJdk11, folder);
                }
            }
            var res = await javaInstaller.InstallAsync(async x =>
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    LittleTaskProgress = x.Item2;
                    MainTaskProgress = x.Item2;
                    Dispatcher.UIThread.Post(() => TaskProgress = x.Item1);

                    if (x.Item2.Contains("开始解压 Jdk"))
                        Dispatcher.UIThread.Post(() => IsLoadOk = true);
                });
            });

            if (res.Success)
            {
                LittleTaskProgress = "安装完成";
                MainTaskProgress = "已完成";
                Dispatcher.UIThread.Post(() => IsLoadOk = false);
                Dispatcher.UIThread.Post(() => TaskProgress = 1);
                IsFileOpenVisible = true;
                FilePath = JavaInstaller.StorageFolder;
                MainView.ViewModel.AllTaskCount--;
            }
        }
    }

    partial class DownItemViewModel
    {
        public string TaskTitle
        {
            get => _TaskType;
            set => RaiseAndSetIfChanged(ref _TaskType, value);
        }

        public string MainTaskProgress
        {
            get => _MainTaskProgress;
            set => RaiseAndSetIfChanged(ref _MainTaskProgress, value);
        }

        public string LittleTaskProgress
        {
            get => _LittleTaskProgress;
            set => RaiseAndSetIfChanged(ref _LittleTaskProgress, value);
        }

        public string FileLink
        {
            get => _FileLink;
            set => RaiseAndSetIfChanged(ref _FileLink, value);
        }

        public float TaskProgress
        {
            get => _TaskProgress;
            set => RaiseAndSetIfChanged(ref _TaskProgress, value);
        }

        public bool IsFileLinkVisible
        {
            get => _IsFileLinkVisible;
            set => RaiseAndSetIfChanged(ref _IsFileLinkVisible, value);
        }

        public bool IsFileOpenVisible
        {
            get => _IsFileOpenVisible;
            set => RaiseAndSetIfChanged(ref _IsFileOpenVisible, value);
        }

        public bool IsLoadOk
        {
            get => _IsLoadOk;
            set => RaiseAndSetIfChanged(ref _IsLoadOk, value);
        }
    }

    partial class DownItemViewModel
    {
        public string FilePath;
        public string _TaskType;
        public float _TaskProgress = 0;
        public string _MainTaskProgress = "准备进行安装";
        public string _LittleTaskProgress = "准备进行安装";
        public string _FileLink = "";
        public bool _IsFileLinkVisible = false;
        public bool _IsFileOpenVisible = false;
        public bool _IsLoadOk = true;
    }
}
//MainView.ViewModel.AllTaskCount++;
