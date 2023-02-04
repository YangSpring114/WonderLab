using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using Natsurainko.FluentCore.Class.Model.Install.Vanilla;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Module.Installer;
using Natsurainko.FluentCore.Module.Launcher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class DownGameView : Page
    {
        public static DownGameViewModel ViewModel { get; } = new();
        public DownGameView()
        {
            InitializeComponent(true);
            DataContext = ViewModel;
            HasModLoader.DataContext = ViewModel;
        }

        public override async void OnNavigatedTo()
        {
            ViewModel.ClearAll();
            if (ViewModel.SelectedGameCore is not null)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ViewModel.SelectedGameCoreList();
                });
            }
        }

        public void ButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }
    }
}
