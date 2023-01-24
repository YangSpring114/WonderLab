using Avalonia.Controls;
using Avalonia.Remote.Protocol;
using Microsoft.VisualBasic;
using MinecraftLaunch.Modules.Installer;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Class.Model.Install;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Module.Downloader;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    public class PropertyViewModel : ViewModelBase
    {
        public static PropertyViewModel ViewModel { get; set; }
        public static GameCore GameCore { get; set; }
        public static string Instance = new string("");
        public string _LibraryCount;
        public string _AssetCount;
        public string _TotalSize;
        public string _ModLoader;
        public string _LastLaunchTime = "";
        public bool _IsHasModLoader;
        public bool _IsLaunched = false;
        public List<ModLoaderInformation> _ModLoaders;

        /// <summary>
        /// 构造函数 
        /// </summary>
        public PropertyViewModel()
        {
            ViewModel = this;
            if (GameView.gv is not null && GameView.gv.fodlercombo.SelectedItem is not null)
            {
                Instance = GameView.gv.fodlercombo.SelectedItem.ToString();
                _LibraryCount = "";
                _AssetCount = "";
                _TotalSize = "";
                _ModLoader = "";
                _ModLoaders = new();
            }
            else
            {
                Instance = App.Data.FooterPath;
                _ModLoaders = new();
                _LibraryCount = "";
                _AssetCount = "";
                _TotalSize = "";
                _ModLoader = "";
            }
        }
        /// <summary>
        /// 游戏路径
        /// </summary>
        public string GamePath
        {
            get => Instance;
            set => RaiseAndSetIfChanged(ref Instance, value);
        }
        /// <summary>
        /// Libraries文件个数
        /// </summary>
        public string LibraryCount
        {
            get => _LibraryCount;
            set => RaiseAndSetIfChanged(ref _LibraryCount, value);
        }
        /// <summary>
        /// Asset文件个数
        /// </summary>
        public string AssetCount
        {
            get => _AssetCount;
            set => RaiseAndSetIfChanged(ref _AssetCount, value);
        }
        /// <summary>
        /// 总大小
        /// </summary>
        public string TotalSize
        {
            get => _TotalSize;
            set => RaiseAndSetIfChanged(ref _TotalSize, value);
        }
        /// <summary>
        /// 模组加载器
        /// </summary>
        public string ModLoaders
        {
            get => _ModLoader;
            set => RaiseAndSetIfChanged(ref _ModLoader, value);
        }
        /// <summary>
        /// 是否包含模组加载器
        /// </summary>
        public bool IsHasModLoader
        {
            get => _IsHasModLoader;
            set => RaiseAndSetIfChanged(ref _IsHasModLoader, value);
        }
        /// <summary>
        /// 是否启动过
        /// </summary>
        public bool IsLaunched
        {
            get => _IsLaunched;
            set => RaiseAndSetIfChanged(ref _IsLaunched, value);
        }
        /// <summary>
        /// 上次启动的时间
        /// </summary>
        public string LastLaunchTime
        {
            get => _ModLoader;
            set => RaiseAndSetIfChanged(ref _ModLoader, value);
        }
        /// <summary>
        /// 按钮事件
        /// </summary>
        public void OpenFooter()
        {
            using var res = Process.Start(new ProcessStartInfo(GamePath)
            {
                UseShellExecute = true,
                Verb = "open"
            });
        }
        /// <summary>
        /// 按钮事件
        /// </summary>
        public void DeleteGameCore()
        {
            GameCoreToolkit gameCoreLocator = new(GamePath);
            gameCoreLocator.Delete(GameCore.Id);
            GameView.ViewModel.GameSearchAsync();
            MainView.mv.FrameView.Navigate(typeof(GameView));
            MainWindow.ShowInfoBarAsync("成功", $"已将游戏核心 {GameCore.Id} 删除！", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
        }

        public string GetLibraryCount(GameCore id)
        {
            GameCore = id;
            return $"共计 {id.LibraryResources.Count} 个";
        }

        public string GetAssetCount(GameCore id)
        {
            GameCore = id;
            ResourceDownloader resource = new ResourceDownloader(id);
            var v = resource.GetAssetResourcesAsync();
            return $"共计 {v.Result.Count} 个";
        }

        public string GetTotalSize(GameCore id)
        {
            GameCore = id;
            double total = 0;
            foreach (var library in id.LibraryResources)
            {
                if (library.Size != 0)
                    total += library.Size;
                else if (library.Size == 0 && library.ToFileInfo().Exists)
                    total += library.ToFileInfo().Length;
            }

            try
            {
                var assets = new ResourceDownloader(id).GetAssetResourcesAsync().Result;

                foreach (var asset in assets)
                {
                    if (asset.Size != 0)
                        total += asset.Size;
                    else if (asset.Size == 0 && asset.ToFileInfo().Exists)
                        total += asset.ToFileInfo().Length;
                }
            }
            catch { }

            return $"{double.Parse(((double)total / (1024 * 1024)).ToString("0.00"))}";
        }

        public List<ModLoaderInformation> GetModLoader(GameCore core)
        {
            try
            {
                _ModLoaders.Clear();
                core.BehindArguments.ToList().ForEach(x =>
                {
                    switch (x)
                    {
                        case "--tweakClass optifine.OptiFineTweaker":
                            foreach (var lib in core.LibraryResources)
                                if (lib.Name.StartsWith("optifine:OptiFine"))
                                {
                                    var id = lib.Name.Split(':')[2];

                                    _ModLoaders.Add(new ModLoaderInformation
                                    {
                                        LoaderType = ModLoaderType.OptiFine,
                                        Version = id.Substring(id.IndexOf('_') + 1),
                                    });

                                    break;
                                }
                            break;
                        case "--tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker":
                        case "--fml.forgeGroup net.minecraftforge":
                            foreach (var lib in core.LibraryResources)
                                if (lib.Name.StartsWith("net.minecraftforge:forge:") || lib.Name.StartsWith("net.minecraftforge:fmlloader:"))
                                {
                                    _ModLoaders.Add(new ModLoaderInformation
                                    {
                                        LoaderType = ModLoaderType.Forge,
                                        Version = lib.Name.Split(':')[2].Split('-')[1]
                                    });

                                    break;
                                }
                            break;
                    }
                });
            }
            catch { }

            try
            {
                core.FrontArguments.ToList().ForEach(x =>
                {
                    if (x.Contains("-DFabricMcEmu= net.minecraft.client.main.Main"))
                        foreach (var lib in core.LibraryResources)
                            if (lib.Name.StartsWith("net.fabricmc:fabric-loader"))
                            {
                                _ModLoaders.Add(new ModLoaderInformation
                                {
                                    LoaderType = ModLoaderType.Fabric,
                                    Version = lib.Name.Split(':')[2]
                                });

                                break;
                            }
                });
            }
            catch { }

            if (_ModLoaders.Count > 0)
                IsHasModLoader = true;
            else
                IsHasModLoader = false;

            return _ModLoaders;
        }

        public (string,bool) GetLastLaunchTime(GameCore core)
        {
            (string, bool) info = new();
            var time = JsonToolkit.GetTimeInfoJson(GamePath,core);
            if (!string.IsNullOrEmpty(time))
                info.Item2 = true;
            else
                info.Item2 = false;
            info.Item1 = time;
            return info;
        }
    }
}
