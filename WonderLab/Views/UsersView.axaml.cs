using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using System;
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
        }

        private async void UsersView_Initialized(object? sender, EventArgs e)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                View = this;
                this.DataContext = ViewModel;
                AuthenticatorTypeDialog.DataContext = ViewModel;
                LoginDialog.DataContext = ViewModel;
                SetupDnd("Text", d => d.Set(DataFormats.Text,
                         $""), DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
            });
        }

        public override void OnNavigatedTo()
        {
            //ViewModel.CurrentUser = ViewModel.Users[ViewModel.Users.IndexOf(new(App.Data.SelectedUser!))];
            //MainWindow.win.TipShow();ShowUserInfoDialogClick
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

        private async void ShowUserInfoDialogClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => await UserInfoDialog.ShowAsync();

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

        void SetupDnd(string suffix, Action<DataObject> factory, DragDropEffects effects)
        {
            async void DoDrag(object? sender, Avalonia.Input.PointerPressedEventArgs e)
            {
                var dragData = new DataObject();
                factory(dragData);

                var result = await DragDrop.DoDragDrop(e, dragData, effects);
                Trace.WriteLine($"[信息] DragDrop类型如下 {result}");
            }

            void DragOver(object? sender, DragEventArgs e)
            {
                if (e.Source is Control c && c.Name == "MoveTarget")
                {
                    e.DragEffects = e.DragEffects & (DragDropEffects.Move);
                }
                else
                {
                    e.DragEffects = e.DragEffects & (DragDropEffects.Copy);
                }

                // Only allow if the dragged data contains text or filenames.
                if (!e.Data.Contains(DataFormats.Text)
                    && !e.Data.Contains(DataFormats.FileNames)
                    && !e.Data.Contains(CustomFormat))
                    e.DragEffects = DragDropEffects.None;
            }

            void DragEnter(object? sender, DragEventArgs e)
            {
                if (e.Data.GetText().Contains("authlib-injector:yggdrasil-server")) {
                    
                }
            }


            void Drop(object? sender, DragEventArgs e)
            {
                if (e.Source is Control c && c.Name == "MoveTarget")
                {
                    e.DragEffects = e.DragEffects & (DragDropEffects.Move);
                }
                else
                {
                    e.DragEffects = e.DragEffects & (DragDropEffects.Copy);
                    ViewModel.UrlTextBoxText = e.Data.GetText().Replace("authlib-injector:yggdrasil-server:", string.Empty).Replace("%2F", "/").Replace("%3A", ":");
                }

                if (e.Data.Contains(DataFormats.Text))
                {
                }
                else if (e.Data.Contains(DataFormats.FileNames))
                    Trace.WriteLine(string.Join(Environment.NewLine, e.Data.GetFileNames() ?? Array.Empty<string>()));
                else if (e.Data.Contains(CustomFormat))
                    Trace.WriteLine("Custom: " + e.Data.Get(CustomFormat));
            }

            YggUrl.PointerPressed += DoDrag;
            YggUrl.AddHandler(DragDrop.DropEvent, Drop);
            YggUrl.AddHandler(DragDrop.DragEnterEvent, DragEnter);
            YggUrl.AddHandler(DragDrop.DragOverEvent, DragOver);
        }
    }

    partial class UsersView
    {
        private const string CustomFormat = "application/xxx-avalonia-controlcatalog-custom";
        public static UsersViewModel ViewModel { get; } = new();        
        protected static UsersView View { get; set; }
    }
}
