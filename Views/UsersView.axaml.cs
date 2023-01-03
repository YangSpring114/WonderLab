using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;
using ComboBoxItem = Avalonia.Controls.ComboBoxItem;

namespace WonderLab.Views
{
    public partial class UsersView : Page
    {
        public UsersView()
        {
            InitializeComponent(true);
            View = this;
            this.DataContext = ViewModel;
            AuthenticatorTypeDialog.DataContext = ViewModel;
            LoginDialog.DataContext = ViewModel;
        }
        public override void OnNavigatedTo()
        {
            ViewModel.CurrentUser = ViewModel.Users.GetUserInIndex(App.Data.SelectedUser.UserName);
        }

        public static async void ShowLoginDialog()
        {
            View.AuthenticatorTypeDialog.Hide();
            await Task.Delay(500);
            await View.LoginDialog.ShowAsync();
        }

        public static void CloseDialog()
        {
            View.AuthenticatorTypeDialog.Hide();
            View.LoginDialog.Hide();
        }

        private async void StartButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => await AuthenticatorTypeDialog.ShowAsync();

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => AuthenticatorTypeDialog.Hide();

        private void CancelButtonClick1(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => LoginDialog.Hide();
    }

    partial class UsersView
    {
        public static UsersViewModel ViewModel { get; } = new();        
        protected static UsersView View { get; set; }
    }
}
