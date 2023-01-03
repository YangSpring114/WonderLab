using MinecraftLaunch.Modules.Toolkits;
using System.Diagnostics;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class HomeView : Page
    {
        public HomeView()
        {
            InitializeComponent(true);
            DataContext = HomeViewModel;
        }

        public override async void OnNavigatedTo()
        {
            HomeViewModel.GameSearchAsync();
            HomeViewModel.RefreshUserAsync();
        }
    }

    partial class HomeView
    {
        public static HomeViewModel HomeViewModel = new();
        public static HomeView home;
    }
}
