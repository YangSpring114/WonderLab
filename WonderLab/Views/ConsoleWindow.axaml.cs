using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Analyzers;
using MinecraftLaunch.Modules.Interface;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Class.Model.Launch;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using WonderLab.ViewModels;
using GameCore = MinecraftLaunch.Modules.Models.Launch.GameCore;

namespace WonderLab.Views
{
    public partial class ConsoleWindow : Window
    {
        public bool IsKill = false;
        JavaClientLaunchResponse launchResponse;
        public ConsoleWindowViewModel ViewModel { get; set; }
        public ConsoleWindow()
        {
            InitializeComponent();
            //DataContext = ViewModel;
        }

        public ConsoleWindow(JavaClientLaunchResponse lr,string gameId)
        {
            InitializeComponent();
            //Title = $"游戏实时日志输出窗口 - {gameId}";
            ////ViewModel = new(lr);            
            //CloseButton.Click += CloseButton_Click;
            //ViewModel = new ConsoleWindowViewModel(lr);
            //ss.ScrollChanged += Ss_ScrollChanged;
            //lr.ProcessOutput += Lr_MinecraftProcessOutput;
            //lr.Exited += Lr_Exited;
            //DataContext = ViewModel;
            //LogList.DataContext = ViewModel;
        }

        public void ShowAsync<TOutputs>(TOutputs log, GameCore id, JavaClientLaunchResponse response) where TOutputs: List<string> {
            Title = $"游戏实时日志输出窗口 - {id.Id}";
            CloseButton.Click += CloseButton_Click;

            ViewModel = new(log, response, ss);
            DataContext = ViewModel;
            Show();
        }

        private async void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            IsKill = true;
            CloseButton.IsEnabled = false;
            await Task.Delay(1000);
            Close();
        }
    }
}
