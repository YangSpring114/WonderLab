using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab.Modules.Const
{
    public class InfoConst
    {
        public const string CForgeToken = "$2a$10$Awb53b9gSOIJJkdV3Zrgp.CyFP.dI13QKbWn/4UZI4G4ff18WneB6";

        public const string ClientId = "9fd44410-8ed7-4eb3-a160-9f1cc62c824c";

        public const string LauncherVersion = "1.0.2.0";

        public const bool IsDevelopVersion = true;

        public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsWindows11 => (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.OSVersion.Version.Build >= 22000);

        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// 指定的游戏核心是否启用了版本隔离
        /// </summary>
        /// <returns></returns>
        public static bool IsEnableIndependencyCore(string id)
        {
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, id);
            if (IndependencyCoreData is not null && IndependencyCoreData.IsEnableIndependencyCore && IndependencyCoreData.Isolate) {  
                Trace.WriteLine("[信息] 此核心将使用独立版本隔离");
                return true;
            }

            if (App.Data.Isolate) {
                Trace.WriteLine("[信息] 此核心将使用全局版本隔离");
                return true;
            }

            return false;
        }

        public static Dictionary<string, KeyValuePair<string, string>[]> OpenJdkDownloadSources => new()
        {
            {
                "OpenJDK 8",
                new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net","https://download.java.net/openjdk/jdk8u42/ri/openjdk-8u42-b03-windows-i586-14_jul_2022.zip")
                }
            },
            {
                "OpenJDK 11", new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net", "https://download.java.net/openjdk/jdk11/ri/openjdk-11+28_windows-x64_bin.zip"),
                    new ("Microsoft", "https://aka.ms/download-jdk/microsoft-jdk-11.0.16-windows-x64.zip")
                }
            },
            {
                "OpenJDK 17", new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net", "https://download.java.net/openjdk/jdk17/ri/openjdk-17+35_windows-x64_bin.zip"),
                    new ("Microsoft", "https://aka.ms/download-jdk/microsoft-jdk-17.0.4-windows-x64.zip")
                }
            },
            {
                "OpenJDK 18", new KeyValuePair<string, string>[]
                {
                    new ("jdk.java.net", "https://download.java.net/openjdk/jdk18/ri/openjdk-18+36_windows-x64_bin.zip")
                }
            }
        };

        public static Dictionary<string, ModLangDataModel>? ModLangDatas = new();
    }
}
