using Avalonia.Controls;

namespace ControlX
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            s.Click += S_Click;
        }

        private void S_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ss.IsEnabled = true;
        }
    }
}
