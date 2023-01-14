using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Media;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;
using Button = Avalonia.Controls.Button;
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
            //ViewModel.CurrentUser = ViewModel.Users.GetUserInIndex(App.Data.SelectedUser.UserName);
            MainWindow.win.TipShow();
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

        private void DeleteButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var user = (UserModels)(sender as Button).DataContext!;
            
            for (int i = 0; i < App.Data.UserList.Count; i++)
            {
                if (ViewModel.Users[i].Name == App.Data.UserList[i].UserName && ViewModel.Users[i].Uuid == App.Data.UserList[i].UserUuid)
                    App.Data.UserList.Remove(App.Data.UserList[i]);
            }

            ViewModel.Users.Remove(user.Current);
            var temp = ViewModel.Users;
            ViewModel.Users = null;
            ViewModel.Users = new(temp);
            if (App.Data.SelectedUser is not null && App.Data.SelectedUser.UserName.Equals(user.Name) && App.Data.SelectedUser.UserUuid.Equals(user.Uuid))
                App.Data.SelectedUser = null;

            MainWindow.ShowInfoBarAsync("成功:",$"账户 {user.Name} 已成功被移除！", InfoBarSeverity.Success);
        }

        private async void UserSettingOpenPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            ZoomOutAnimation animation = new(true);
            animation.RunAsync((Border)sender);
            await Task.Delay(100);
            animation.IsReversed = false;
            animation.RunAsync((Border)sender);
        }

        private void UserSettingOpenAction(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            ((Border)sender).Height = 230;
        }

        private void BorderImagePointerEnter(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            LogToolkit.WriteLine("鼠标移入头像 Border！");
            ZoomOutAnimation animation = new();
            animation.RunAsync((Border)sender);
        }

        private void BorderImagePointerLeave(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            LogToolkit.WriteLine("鼠标移出头像 Border！");
            ZoomOutAnimation animation = new(true);
            animation.RunAsync((Border)sender);
        }
    }

    partial class UsersView
    {
        public static UsersViewModel ViewModel { get; } = new();        
        protected static UsersView View { get; set; }
    }
}
