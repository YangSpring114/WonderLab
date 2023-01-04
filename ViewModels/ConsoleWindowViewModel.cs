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

namespace WonderLab.ViewModels
{
    public partial class ConsoleWindowViewModel : ViewModelBase
    {
        public double AWidth
        {
            get => _T;
            set => RaiseAndSetIfChanged(ref _T, value);
        }

        public List<IProcessOutput> Logs
        {
            get => _Logs;
            set => RaiseAndSetIfChanged(ref _Logs, value);
        }
    }

    partial class ConsoleWindowViewModel
    {
        public ConsoleWindowViewModel(LaunchResponse process)
        {
            Process = process;
            Process.MinecraftProcessOutput += Process_MinecraftProcessOutput;
        }
        List<IProcessOutput> Outputs = new();
        private void Process_MinecraftProcessOutput(object? sender, Natsurainko.FluentCore.Interface.IProcessOutput e)
        {
            Outputs.Add(e);
            Logs = null;
            Logs = Outputs;
            Debug.WriteLine(Logs[0].Raw);
        }

        public async void ShowLogTypeBar()
        {
            AWidth = 0;
            await Task.Delay(600);
            AWidth = 35;
        }
    }

    partial class ConsoleWindowViewModel
    {
        public LaunchResponse Process = null;
        public ConsoleWindowViewModel() => ShowLogTypeBar();
        public double _T = 0;
        public Dictionary<string, string> _Test = new();
        public List<IProcessOutput> _Logs = new();
    }
}
