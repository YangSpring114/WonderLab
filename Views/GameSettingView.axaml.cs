using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.VisualBasic;
using Natsurainko.FluentCore.Extension.Windows.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class GameSettingView : Page
    {
        public GameSettingViewModel ViewModel = new();
        public GameSettingView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public override void OnNavigatedTo()
        {
            if (App.Data.JavaList.Count != 0)
                ViewModel.CurrentJava = App.Data.JavaPath;
            if (App.Data.GameFooterList.Count != 0)
                ViewModel.CurrentGameFolder = App.Data.FooterPath;
            ViewModel.IsOlate = App.Data.Isolate;
        }

        private void InitializeComponent() => InitializeComponent(true);
    }
}
