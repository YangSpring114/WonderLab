using Avalonia.Controls;
using MinecraftLaunch.Modules.Models.Launch;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class IndependencyCoreSettingView : Page
    {
        public static GameCore GameCore { get; set; }
        public static IndependencyCoreSettingViewModel ViewModel { get; set; } = new();
        public IndependencyCoreSettingView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public override void OnNavigatedTo()
        {
            GameCore = MainPropertyView.SelectGameCore;
            var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, GameCore.ToNatsurainkoGameCore());
            if (res is not null)
            {
                ViewModel.IsEnableIndependencyCore = res.IsEnableIndependencyCore;
                ViewModel.IsFullWindow = res.IsFullWindows;
                ViewModel.IsOlate = res.Isolate;
                ViewModel.WindowHeight = res.WindowHeight.ToString();
                ViewModel.WindowWidth = res.WindowWidth.ToString();
            }
            else
            {
                var res1 = JsonToolkit.CreaftEnableIndependencyCoreInfoJson(App.Data.FooterPath, GameCore.ToNatsurainkoGameCore(), false);
                ViewModel.IsEnableIndependencyCore = res1.IsEnableIndependencyCore;
                ViewModel.IsFullWindow = res1.IsFullWindows;
                ViewModel.IsOlate = res1.Isolate;
                ViewModel.WindowHeight = res1.WindowHeight.ToString();
                ViewModel.WindowWidth = res1.WindowWidth.ToString();
            }
        }
    }
}
