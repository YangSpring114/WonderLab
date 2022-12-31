using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace WonderLab.Views
{
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent(true);
            nvSample6.SelectionChanged += NvSample6_SelectionChanged;
        }

        private void NvSample6_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
        {
            (sender as NavigationView).Content = new HomeView();
        }
    }
}
