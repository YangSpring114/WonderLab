using Avalonia.Controls;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Analyzers;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Extension.Windows.Class.Model.Launch;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class ConsoleWindow : Window
    {
        public bool IsKill = false;
        LaunchResponse launchResponse;
        public ConsoleWindowViewModel ViewModel { get; set; }
        public ConsoleWindow()
        {
            InitializeComponent();
            //DataContext = ViewModel;
        }

        public ConsoleWindow(LaunchResponse lr,string gameId)
        {
            InitializeComponent();
            Title = $"游戏实时日志输出窗口 - {gameId}";
            //ViewModel = new(lr);            
            CloseButton.Click += CloseButton_Click;
            ViewModel = new ConsoleWindowViewModel(lr);
            lr.MinecraftProcessOutput += Lr_MinecraftProcessOutput;
            DataContext = ViewModel;
            LogList.DataContext = ViewModel;
        }

        private async void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            IsKill = true;
            CloseButton.IsEnabled = false;
            await Task.Delay(1000);
            Close();
        }

        List<LogModels> logs = new();
        private void Lr_MinecraftProcessOutput(object? sender, Natsurainko.FluentCore.Interface.IProcessOutput e)
        {
            try
            {
                var logres = GameLogAnalyzer.AnalyseAsync(e.Raw);
                Dispatcher.UIThread.Post(() =>
                {
                    var res = new LogModels()
                    {
                        Log = e.Raw,
                        LogLevel = logres.LogType
                    };
                    logs.Add(res);
                    var log = logs;
                    Debug.WriteLine($"[Console Log] {e.Raw}");
                    LogList.Items = null;
                    LogList.Items = logs;
                    ss.ScrollToEnd();
                });
            }
            catch { }
        }
    }
}
