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

namespace WonderLab.ViewModels
{
    public partial class ConsoleWindowViewModel : ViewModelBase
    {
        public double AWidth
        {
            get => _T;
            set => RaiseAndSetIfChanged(ref _T, value);
        }

        public List<LogModels> Logs
        {
            get => _Logs;
            set => RaiseAndSetIfChanged(ref _Logs, value);
        }
    }

    partial class ConsoleWindowViewModel
    {
        public ConsoleWindowViewModel(JavaClientLaunchResponse process)
        {
            ShowLogTypeBar();
            Process = process;
        }

        public async void ShowLogTypeBar()
        {
            //await Task.Delay(1000);
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
        public JavaClientLaunchResponse Process = null;
        public ConsoleWindowViewModel() => ShowLogTypeBar();
        public double _T = 0;
        public Dictionary<string, string> _Test = new();
        public List<LogModels> _Logs = new();
    }
}
