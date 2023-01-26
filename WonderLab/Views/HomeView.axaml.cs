using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Toolkits;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class HomeView : Page
    {
        public HomeView()
        {
            InitializeComponent(true);
            DataContext = HomeViewModel;
            SettingButton.PointerEnter += Launchbutton_PointerEnter;
            SettingButton.PointerLeave += Launchbutton_PointerLeave;
            SettingButton.Click += SettingButton_Click;

            Trace.WriteLine($"[信息] 语言中 Select 键的值为 {LanguageToolkit.GetText("Select")}");
        }

        private void SettingButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainPropertyView.SelectGameCore = HomeViewModel.SelectedGameCore;
            var view = new MainPropertyView();
            MainView.mv.FrameView.Navigate(view.GetType());
        }

        private async void Launchbutton_PointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            await Task.Delay(200);
            if (SettingButton.Width is 25)
            {
                SettingButton.Width = 0;
            }
        }

        private void Launchbutton_PointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            SettingButton.Width = 25;
        }

        public override void OnNavigatedTo()
        {
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                HomeViewModel.GameSearchAsync();
                HomeViewModel.RefreshUserAsync();
            };
            worker.RunWorkerAsync();
        }
    }

    partial class HomeView
    {
        public static HomeViewModel HomeViewModel = new();
        public static HomeView home;
    }
}
