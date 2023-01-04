using System;
using System.Collections.Generic;
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
    }

    partial class ConsoleWindowViewModel
    {
        public async void ShowLogTypeBar()
        {
            AWidth = 0;
            await Task.Delay(600);
            AWidth = 35;
        }
    }

    partial class ConsoleWindowViewModel
    {
        public ConsoleWindowViewModel() => ShowLogTypeBar();
        public double _T = 0;
    }
}
