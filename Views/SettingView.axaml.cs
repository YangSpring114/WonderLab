using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using System;
using WonderLab.Modules.Controls;

namespace WonderLab.Views
{
    public partial class SettingView : Page
    {
        public static SettingView sv { get; set; }
        public SettingView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
            sv = this;
            FrameView.Navigated += FrameView_Navigated;
            //home.IsSelected = true;
            //FrameView.Navigate(typeof(GameSettingView), null, new SlideNavigationTransitionInfo());
            RootNavigationView.ItemInvoked += RootNavigationView_ItemInvoked;
        }

        private void FrameView_Navigated(object sender, FluentAvalonia.UI.Navigation.NavigationEventArgs e)
        {
            foreach (NavigationViewItem item in RootNavigationView.MenuItems)
            {
                if ((string)item.Tag == e.SourcePageType.Name)
                {
                    RootNavigationView.SelectedItem = item;
                    item.IsSelected = true;
                }
            }
            FrameView.NavigateTo((Page)e.Content);
        }

        public override void OnNavigatedTo()
        {
            NavigatedToGameSettingView();
        }

        private void RootNavigationView_BackRequested(object? sender, NavigationViewBackRequestedEventArgs e)
        {
            FrameView.GoBack(null);
        }

        private void RootNavigationView_ItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            FrameView.Navigate((Page)FrameView.Content, e.IsSettingsInvoked ? typeof(GameSettingView) : (Type.GetType($"WonderLab.Views.{((NavigationViewItem)e.InvokedItemContainer).Tag ??= string.Empty}")) ?? typeof(GameSettingView), null, null);
        }
    }
}
