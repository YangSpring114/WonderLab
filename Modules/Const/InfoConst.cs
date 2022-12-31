using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;

namespace WonderLab.Modules.Const
{
    public class InfoConst
    {
        public const string CForgeToken = "$2a$10$/R2wMCh2My1cWvaQN96qmOuXLeu.L9R29cy6RwaXnoC3jyBbvLYNe";

        public const string ClientId = "9fd44410-8ed7-4eb3-a160-9f1cc62c824c";

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
