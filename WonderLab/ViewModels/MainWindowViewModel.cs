using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;

namespace WonderLab.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<InfoBarModel> InfoBarItems
        {
            get => _InfoBarItems;
            set => RaiseAndSetIfChanged(ref _InfoBarItems, value);
        }

        private ObservableCollection<InfoBarModel> _InfoBarItems = new();

        public static MainWindowViewModel ViewModel;

        public static TitleBar TitleBar { get; set; }

        public Window Window;

        public MainWindowViewModel(Window window)
        {
            Window = window;
            ViewModel = this;
        }

        public void Colse() => TitleBar.OnClose();
        public void MaxWindowSize() => TitleBar.OnRestore();
        public void MiniWindowSize() => TitleBar.OnMinimize();            
    }
}
