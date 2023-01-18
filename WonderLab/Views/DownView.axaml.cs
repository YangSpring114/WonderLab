using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class DownView : Page
    {
        public DownViewModel ViewModel { get; } = new DownViewModel();
        public DownView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
            DataContext = ViewModel;
            JavaInstallDialog.DataContext = ViewModel;
            JavaInstall.PointerPressed += JavaInstall_PointerPressed;
        }

        private async void JavaInstall_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) => await JavaInstallDialog.ShowAsync();

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            JavaInstallDialog.Hide();
            MainWindow.ShowInfoBarAsync($"信息", "已取消安装 Java 运行时", severity: InfoBarSeverity.Informational);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            JavaInstallDialog.Hide();
            TasksTooklit.CreateJavaInstallTask(ViewModel.CurrentUrl.Value.Value, sender as Control);
        }
    }
}
