using Avalonia.Controls;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Analyzers;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Interface;
using System;
using System.Collections.Generic;
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

        public List<LogModels> Outputs
        {
            get => _Logs;
            set => RaiseAndSetIfChanged(ref _Logs, value);
        }
    }

    partial class ConsoleWindowViewModel
    {
        public ConsoleWindowViewModel(List<string> outputs, JavaClientLaunchResponse process, ScrollViewer box) {        
            ShowLogTypeBar();
            Process = process;
            Box = box;
            Process.ProcessOutput += Process_ProcessOutput;
            Outputs = outputs.Select(x =>
            {
                var output = GameLogAnalyzer.AnalyseAsync(x);

                return output.ToOutput();
            }).ToList();
        }

        private async void Process_ProcessOutput(object? sender, MinecraftLaunch.Modules.Interface.IProcessOutput e)
        {
            OutputsInsertAction(GameLogAnalyzer.AnalyseAsync(e.Raw).ToOutput());
            await Dispatcher.UIThread.InvokeAsync(() => Box.ScrollToEnd());         
        }

        [Obsolete]
        public ConsoleWindowViewModel(JavaClientLaunchResponse process)
        {
            ShowLogTypeBar();
            Process = process;
        }

        public void OutputsInsertAction(LogModels raw)
        {
            var outputs = Outputs;
            outputs.Add(raw);
            Outputs = outputs.Select(x => x).ToList();
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
        public Dictionary<string, string> _Test = new();
        public List<LogModels> _Logs = null;
    }
}
