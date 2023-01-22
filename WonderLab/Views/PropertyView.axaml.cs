using Avalonia.Controls;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Module.Launcher;
using System;
using System.Diagnostics;
using System.Linq;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class PropertyView : Page
    {
        public static GameCore GameCore { get; set; }
        public static PropertyView Instance { get; set; }
        public static PropertyViewModel PropertyViewModel = new PropertyViewModel();
        public PropertyView()
        {
            InitializeComponent(true);
            DataContext = PropertyViewModel;
            Instance = this;
        }

        public override void OnNavigatedTo()
        {
            try
            {
                if (GameCore is null)
                {
                    Debug.WriteLine("Null");
                    return;
                }

                var app = App.Data;
                var core = new GameCoreLocator(app.FooterPath).GetGameCore(GameCore.Id);
                PropertyViewModel.GamePath = app.FooterPath;
                var time = PropertyViewModel.GetLastLaunchTime(core);
                PropertyViewModel.IsLaunched = time.Item2;
                PropertyViewModel.LastLaunchTime = time.Item1;
                PropertyViewModel.AssetCount = PropertyViewModel.GetAssetCount(core);
                PropertyViewModel.LibraryCount = PropertyViewModel.GetLibraryCount(core);
                PropertyViewModel.TotalSize = PropertyViewModel.GetTotalSize(core);
                PropertyViewModel.ModLoaders = PropertyViewModel.GetModLoader(core)
                    .Any() ? string.Join('，', PropertyViewModel.GetModLoader(core)
                    .Select(x => $"{x.LoaderType}，{x.Version}")) : null;
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine("Chao");
            }
        }
    }
}
