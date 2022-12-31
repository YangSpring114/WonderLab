using Avalonia.Threading;
using MinecraftLaunch.Modules.Models.Download;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public partial class ModPropertyViewModel : ViewModelBase
    {
        public List<ModDataModel> ModPacks
        {
            get => _ModPacks;
            set => RaiseAndSetIfChanged(ref _ModPacks, value);
        }

        public bool HasMod
        {
            get => _HasMod;
            set => RaiseAndSetIfChanged(ref _HasMod, value);
        }
    }

    partial class ModPropertyViewModel
    {
        public async void LoadModList()
        {
            List<ModDataModel> temp = new();
            ModPacks = null;
            var orgin = (await Toolkit.LoadAllAsync()).ToList();
            foreach (var mod in orgin)
                temp.Add(new(mod));

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ModPacks = temp;
                if (ModPacks.Count > 0)
                    HasMod = false;
                else HasMod = true;
            });
        }
    }

    partial class ModPropertyViewModel
    {
        public ModPropertyViewModel()
        {
            var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, SelectedGameCore.ToNatsurainkoGameCore());
            bool isolate = App.Data.Isolate;
            if (res != null && res.IsEnableIndependencyCore)
                isolate = res.Isolate;

            Toolkit = new(SelectedGameCore, false, isolate, App.Data.FooterPath);
            ModDataModel.SetToolkit(Toolkit);
            LoadModList();
        }
    }

    partial class ModPropertyViewModel
    {
        public static GameCore SelectedGameCore { get; set; }
        public List<ModDataModel> _ModPacks;
        public bool _HasMod = true;
        public ModPackToolkit Toolkit;
    }
}
