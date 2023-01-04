using Avalonia.Controls;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Extension.Windows.Class.Model.Launch;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class ConsoleWindow : Window
    {
        public ConsoleWindowViewModel ViewModel { get; set; } = new();
        public ConsoleWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public ConsoleWindow(LaunchResponse lr)
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel = new(lr);
        }
    }
}
