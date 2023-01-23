using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Natsurainko.FluentCore.Module.Launcher;
using System.Collections.Generic;
using System;
using WonderLab.Modules.Models;
using Avalonia.OpenGL;
using FluentAvalonia.UI.Media.Animation;
using System.Diagnostics;
using WonderLab.ViewModels;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Modules.Models.Launch;
using Button = Avalonia.Controls.Button;
using Avalonia.Controls.Presenters;
using WonderLab.Modules.Controls;

namespace WonderLab.Views
{
    public partial class GameView : Page
    {
        public static double ScrollViewerWidth => gv.ScrollViewerHost.Width;
        public static double HostPageWidth => gv.Width;
        public static GameView gv;
        public static GameViewModels ViewModel { get; } = new GameViewModels();
        public GameView()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ChangeNameDialog.DataContext = ViewModel;
            gv = this;            
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
        }

        public override void OnNavigatedTo()
        {
            ViewModel.SelectCoreVisibilityOption = 0;
            ViewModel.SelectCoreSortOption = 0;
            ViewModel.FodlerList = null!;
            ViewModel.FodlerList = App.Data.GameFooterList;
            ViewModel.SelectedFooler = App.Data.SelectedGameFooter;
            ViewModel.GameSearchAsync();      
        }

        private void LaunchButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) =>
            ViewModel.LaunchClick((((Button)sender!).DataContext as GameCore)!);

        private void ButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) =>
            ViewModel.LaunchClick((((MenuFlyoutItem)sender!).DataContext as GameCore)!);

        private void ButtonClick1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainPropertyView.SelectGameCore = (((MenuFlyoutItem)sender!).DataContext as GameCore)!;
            var view = new MainPropertyView();
            MainView.mv.FrameView.Navigate(view.GetType());
        }

        private async void ButtonClick2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (ViewModel.CurrentGameCore is not null)
                await ChangeNameDialog.ShowAsync();
            else
                MainWindow.ShowInfoBarAsync("提示：","未选择任何游戏核心！", InfoBarSeverity.Error);
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //if (ViewModel.CurrentGameCore is not null)
            ChangeNameDialog.Hide();
            ViewModel.NewGameCoreName = string.Empty;
        }

        private void ChangeGameCoreNameClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ViewModel.ChangeGameCoreName();
            ChangeNameDialog.Hide();
        }
    }
}