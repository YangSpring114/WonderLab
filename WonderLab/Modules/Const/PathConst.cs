using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Toolkits;

namespace WonderLab.Modules.Const
{
    /// <summary>
    /// 路径常量表
    /// </summary>
    public class PathConst
    {
        public static readonly string X = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true ? "\\" : "/";

        public static string DownloaderPath = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WonderLab","Temp")}{X}WonderLab.Desktop{(InfoConst.IsWindows ? ".exe" : "")}";

        public static string SettingJsonPtah = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WonderLab")}{X}MainSetting.json";

        public static string OtherJsonPtah = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WonderLab")}{X}Other.json";

        public static string MainDirectory = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"WonderLab")}";

        public static string TempDirectory = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WonderLab")}{X}Temp";

        public static string GetVersionsFolder(string root) => $"{root}{X}versions";

        public static string GetVersionFolder(string root, string id) => $"{root}{X}versions{X}{id}";

        public static string GetVersionModsFolder(string root, string id) => $"{root}{X}versions{X}{id}{X}mods";

        public static string GetLibrariesFolder(string root) => $"{root}{X}libraries";

        public static string GetAssetsFolder(string root) => $"{root}{X}assets";

        public static string GetAssetIndexFolder(string root) => $"{root}{X}assets{X}indexes";

        public static string GetLogConfigsFolder(string root) => $"{GetAssetsFolder(root)}{X}log_configs";

        public static string GetModsFolder(string root,string id) {
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, id);

            if (IndependencyCoreData is not null && IndependencyCoreData.IsEnableIndependencyCore && IndependencyCoreData.Isolate) {
                return Path.Combine(root, "versions", id, "mods");
            }

            if (App.Data.Isolate) {            
                return Path.Combine(root, "versions", id, "mods");
            }

            return Path.Combine(root,"mods");
        }

        public static string GetResourcePacksFolder(string root,string id) {
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, id);

            if (IndependencyCoreData is not null && IndependencyCoreData.IsEnableIndependencyCore && IndependencyCoreData.Isolate) {
                return Path.Combine(root, "versions", id, "resourcepacks");
            }

            if (App.Data.Isolate) {
                return Path.Combine(root, "versions", id, "resourcepacks");
            }

            return Path.Combine(root, "resourcepacks");
        }

        public static string GetShaderPacksFolder(string root,string id) {
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, id);

            if (IndependencyCoreData is not null && IndependencyCoreData.IsEnableIndependencyCore && IndependencyCoreData.Isolate) {
                return Path.Combine(root, "versions", id, "shaderpacks");
            }

            if (App.Data.Isolate) {           
                return Path.Combine(root, "versions", id, "shaderpacks");
            }

            return Path.Combine(root, "shaderpacks");
        }

        public static string GetOptions(string root, string id)
        {
            var IndependencyCoreData = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, id);

            if (IndependencyCoreData is not null && IndependencyCoreData.IsEnableIndependencyCore && IndependencyCoreData.Isolate)
            {
                return Path.Combine(root, "versions", id, "options.txt");
            }

            if (App.Data.Isolate)
            {
                return Path.Combine(root, "versions", id, "options.txt");
            }

            return Path.Combine(root, "options.txt");
        }
    }
}
