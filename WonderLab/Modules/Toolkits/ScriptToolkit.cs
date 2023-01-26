using FluentAvalonia.Interop;
using MinecraftLaunch.Modules.ArgumentsBuilders;
using MinecraftLaunch.Modules.Models.Launch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Const;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 游戏脚本生成工具类
    /// </summary>
    public class ScriptToolkit
    {
        /// <summary>
        /// 异步构建启动脚本方法
        /// </summary>
        public static async void LaunchScriptBuildAsync(string path, GameCore core,LaunchConfig config,bool IsBBGL)
        {
            FileInfo info = new(path);
            JavaClientArgumentsBuilder builder = new(core, config, IsBBGL);
            List<string> scripts = null;            

            if (InfoConst.IsWindows)
            {
                if (!info.Directory.Exists) {
                    info.Directory.Create();
                }
                scripts = new()
                {
                    "@echo off",
                    $"title 启动 - {core.Id}",
                    "echo 开始尝试启动游戏进程，这个过程很快的，坐和放宽",
                    $"set APPDATA=\"{core.Root.FullName}\"",
                    $"cd /D \"{core.Root.FullName}\""
                };
                scripts.Add($"\"{string.Join(" ", builder.Build())}\"");
                scripts.AddRange(new List<string>() { "echo 游戏已退出", "pause" });
                info.Create();
                await Task.Delay(1000);
            }

            await File.WriteAllTextAsync(info.FullName,string.Join("\n",scripts));
        }
    }
}
