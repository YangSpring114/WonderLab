using MinecraftLaunch.Modules.Installer;
using System.CommandLine;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace WonderLab.Desktop
{
    /// <summary>
    /// 耗时操作专用进程
    /// </summary>
    public class Program
    {
        //WonderLab.Desktop GameCoreDownloader --core 1.19.3 --folder C:\Users\w\Desktop\temp\.minecraft
        static async Task Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            var rootcommand = new RootCommand();
            
            var core = new Option<string>(name: "--core");
            var coreId = new Option<string>(name: "--coreId");
            var folder = new Option<string>(name: "--folder", getDefaultValue: () => ".minecraft");
            var maxThread = new Option<int>(name: "--maxThread", getDefaultValue: () => 128);
            var java = new Option<string>(name: "--javapath");
            var modLoader = new Option<string>(name: "--modLoader");

            var downloader = new Command("GameCoreDownloader")
            {
                core,
                folder,
                maxThread,
                coreId,
                java,
                modLoader,
            };

            rootcommand.Add(downloader);
            downloader.SetHandler(async (core, folder, maxThread, coreId, java, modLoader) =>
            {
                await GameCoreDownloadAsync(core, folder, maxThread, modLoader, coreId, java);
            }, core, folder, maxThread, coreId, java, modLoader);
            
            await rootcommand.InvokeAsync(args);
        }

        /// <summary>
        /// 游戏核心下载方法
        /// </summary>
        public static async ValueTask GameCoreDownloadAsync(string core,string folder,int maxthread, string modLoader = null,string coreId = null, string java = null)
        {
            ResourceInstaller.MaxDownloadThreads = maxthread;
            if (string.IsNullOrEmpty(modLoader) || modLoader.Split("<>").Length <= 0)
            {
                GameCoreInstaller installer = new(new(folder), core);
                await installer.InstallAsync(x =>
                {
                    Console.WriteLine($"{x.Item2}|{x.Item1}");
                });
            }
            else
            {
                if (modLoader.Split("<>").Length is 2)
                {
                    var mls = modLoader.Split("<>");
                    var resforge = (await ForgeInstaller.GetForgeBuildsOfVersionAsync(core)).Where(x =>
                    {
                        var forge = mls.First().ToLower().Contains("forge") ? mls.First() : mls.Last();
                        return forge.Contains(x.ForgeVersion);
                    });
                    var resoptifine = (await OptiFineInstaller.GetOptiFineBuildsFromMcVersionAsync(core)).Where(x =>
                    {
                        var optifine = mls.First().ToLower().Contains("optifine") ? mls.First() : mls.Last();
                        return optifine.Contains(x.Type);
                    });

                    ForgeInstaller installer = new(new(folder), resforge.First(), java, coreId);
                    var Ires = await installer.InstallAsync(x =>
                    {
                        Console.WriteLine($"{x.Item2}|{x.Item1}");
                    });

                    if (Ires.Success)
                    {
                        OptiFineInstaller fineInstaller = new(new(folder), resoptifine.First(), java, customId: coreId);
                        var res = await fineInstaller.InstallAsync(x =>
                        {
                            Console.WriteLine($"{x.Item2}|{x.Item1}");
                        });

                        if (!res.Success)
                        {
                            Console.WriteLine("安装失败");
                        }
                    }
                    else
                    {
                        Console.WriteLine("安装失败");
                    }
                }
                else if(modLoader.ToLower().Contains("forge"))
                {
                    var res = (await ForgeInstaller.GetForgeBuildsOfVersionAsync(core)).Where(x => modLoader.Contains(x.ForgeVersion));

                    ForgeInstaller installer = new(new(folder), res.First(), java,coreId);
                    var Ires = await installer.InstallAsync(x =>
                    {
                        Console.WriteLine($"{x.Item2}|{x.Item1}");
                    });

                    if (!Ires.Success)
                        Console.WriteLine("安装失败");
                }
                else if (modLoader.ToLower().Contains("fabric"))
                {
                    var res = (await FabricInstaller.GetFabricBuildsByVersionAsync(core)).Where(x => modLoader.Contains(x.Loader.Version));

                    FabricInstaller installer = new(new(folder), res.First(), customId: coreId);
                    var Ires = await installer.InstallAsync(x =>
                    {
                        Console.WriteLine($"{x.Item2}|{x.Item1}");
                    });

                    if (!Ires.Success)
                        Console.WriteLine("安装失败");
                }
                else if (modLoader.ToLower().Contains("optifine"))
                {
                    var ores = (await OptiFineInstaller.GetOptiFineBuildsFromMcVersionAsync(core)).Where(x => modLoader.Contains(x.Type));
                    OptiFineInstaller fineInstaller = new(new(folder), ores.First(), java, customId: coreId);
                    var res = await fineInstaller.InstallAsync(x =>
                    {
                        Console.WriteLine($"{x.Item2}|{x.Item1}");
                    });

                    if (!res.Success)
                    {
                        Console.WriteLine("安装失败");
                    }
                }
            }
        }
    }
}