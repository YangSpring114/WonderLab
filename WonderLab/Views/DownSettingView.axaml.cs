using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SkiaSharp;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class DownSettingView : Page
    {
        public DownSettingView()
        {
            InitializeComponent(true);
            DataContext = new DownSettingViewModel();
        }
    }
}


    //resm:WonderLab.Resources.HarmonyOS#HarmonyOS Sans