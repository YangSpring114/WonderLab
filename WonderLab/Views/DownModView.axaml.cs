using Avalonia.Controls;
using System.Diagnostics;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class DownModView : Page
    {
        public static DownModViewModel ViewModel = new();
        public DownModView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        
        public void NavigationToModInfo(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }

        public void InstallClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TasksTooklit.CreateModDownloadTask((CurseForgeModel)((Button)sender).DataContext!, sender as Control);
        }
    }
}
