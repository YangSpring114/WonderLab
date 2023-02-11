using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.ViewModels
{
    public class ExplosionRadioViewModel : ReactiveObject 
    {
        public ExplosionRadioViewModel() {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Trace.WriteLine($"[信息] ReactiveUI 测试，改变的值为：{e.PropertyName}");
        }
    }
}
