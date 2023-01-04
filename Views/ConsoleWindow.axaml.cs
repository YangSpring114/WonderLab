using Avalonia.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class ConsoleWindow : Window
    {
        //public static ConsoleWindowViewModel ViewModel { get; set; } = new();
        public ConsoleWindow()
        {
            InitializeComponent();
            DataContext = new ConsoleWindowViewModel();
        }
    }
}
