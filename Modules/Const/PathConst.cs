using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Const
{
    /// <summary>
    /// 路径常量表
    /// </summary>
    public class PathConst
    {
        public const string SettingJsonPtah = $"{MainDirectory}\\MainSetting.json";

        public const string OtherJsonPtah = $"{MainDirectory}\\Other.json";

        public const string MainDirectory = @"WonderLab";

        public const string SkinHeadDirectory = $"{MainDirectory}\\Temp";

        public static readonly string X = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true ? "\\" : "/";

        public static string GetVersionsFolder(string root) => $"{root}{X}versions";

        public static string GetVersionFolder(string root, string id) => $"{root}{X}versions{X}{id}";

        public static string GetVersionModsFolder(string root, string id) => $"{root}{X}versions{X}{id}{X}mods";

        public static string GetLibrariesFolder(string root) => $"{root}{X}libraries";

        public static string GetAssetsFolder(string root) => $"{root}{X}assets";

        public static string GetAssetIndexFolder(string root) => $"{root}{X}assets{X}indexes";

        public static string GetLogConfigsFolder(string root) => $"{GetAssetsFolder(root)}{X}log_configs";
    }
}
