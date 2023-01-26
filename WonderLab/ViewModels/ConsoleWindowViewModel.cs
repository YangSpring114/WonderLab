using Avalonia.Controls;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Analyzers;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public partial class ConsoleWindowViewModel : ViewModelBase
    {
        public double AWidth
        {
            get => _T;
            set => RaiseAndSetIfChanged(ref _T, value);
        }

        public ObservableCollection<LogModels> Outputs
        {
            get => _Logs;
            set => RaiseAndSetIfChanged(ref _Logs, value);
        }

        public LogModels LastOutput
        {
            get => _Log;
            set => RaiseAndSetIfChanged(ref _Log, value);
        }
    }

    partial class ConsoleWindowViewModel
    {
        public ConsoleWindowViewModel(List<string> outputs, JavaClientLaunchResponse process, ScrollViewer box) {        
            ShowLogTypeBar();
            Process = process;
            Box = box;
            Process.ProcessOutput += Process_ProcessOutput;
            if (outputs is not null && outputs.Count > 0) {
                outputs.ForEach(x =>
                {
                    var output = GameLogAnalyzer.AnalyseAsync(x);
                    Outputs.Add(output.ToOutput());
                });
            }

            LastOutput = Outputs.Last();
        }

        private async void Process_ProcessOutput(object? sender, MinecraftLaunch.Modules.Interface.IProcessOutput e)
        {
            Outputs.Add(GameLogAnalyzer.AnalyseAsync(e.Raw).ToOutput());
            LastOutput = Outputs.Last();
            
            Trace.WriteLine($"[调试] Outputs的索引为 {LastOutput.Log}");
            await Dispatcher.UIThread.InvokeAsync(() => Box.ScrollToEnd());
            //await Dispatcher.UIThread.InvokeAsync(() =>

        }

        public async void ShowLogTypeBar()
        {
            await Task.Run(async() =>
            {
                AWidth = 0;
                await Task.Delay(400);
                AWidth = 35;
            });
        }

        public void KillGame()
        {
            if (!Process.Process.HasExited)
                Process.Process.Kill();
        }
    }

    partial class ConsoleWindowViewModel
    {
        ScrollViewer Box;
        public JavaClientLaunchResponse Process = null;
        public Process GameProcess = null;
        public ConsoleWindowViewModel() => ShowLogTypeBar();
        public double _T = 0;
        public ObservableCollection<LogModels> _Logs = new();
        public LogModels _Log = new();
    }
}
