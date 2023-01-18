using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Models.Download;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Views;

namespace WonderLab.Modules.Models
{
    public class ModDataModel : ViewModelBase
    {
        public ModDataModel(ModPack modPack)
        {
            Id = modPack.Id;
            FileName = modPack.FileName;
            DisplayName = modPack.DisplayName;
            Description = modPack.Description;
            Version = modPack.Version;
            GameVersion = modPack.GameVersion;
            Authors = modPack.Authors;
            Url = modPack.Url;
            Path = modPack.Path;
            IsEnabled = modPack.IsEnabled;
        }

        public string Id { get; set; }

        public string FileName { get => _FileName; set => RaiseAndSetIfChanged(ref _FileName, value); }  

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }

        public string GameVersion { get; set; }

        public string Authors { get; set; }

        public string Url { get; set; }

        public string Path { get => _Path; set => RaiseAndSetIfChanged(ref _Path, value); }

        public string _Path;

        public string _FileName;

        public bool IsEnabled { get; set; }

        public static ModPackToolkit ModPackToolkit { get; set; }

        public static void SetToolkit(ModPackToolkit modPack)
        {
            ModPackToolkit = modPack;
        }

        public void ModStateChange()
        {
            try
            {
                Debug.WriteLine($"ModStateChange,Path:{Path}");                
                Path = ModPackToolkit.ModStateChange(Path);
                FileName = System.IO.Path.GetFileName(Path);
            }
            catch (Exception ex) when (ex.Message.Contains("The process cannot access the file because it is being used by another process"))
            {
                MainWindow.ShowInfoBarAsync("提示:", "更改模组启用状态失败，请尝试关闭当前正在运行的 Minecraft！", InfoBarSeverity.Warning);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MainWindow.ShowInfoBarAsync("错误：", ex.ToString(), FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
            }
        }
    }
}
