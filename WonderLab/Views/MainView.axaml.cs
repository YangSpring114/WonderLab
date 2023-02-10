using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using FluentAvalonia.Core.ApplicationModel;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class MainView : Page
    {
        public static MainViewModel ViewModel { get; } = new();
        public static NavigationViewDisplayMode NavigationViewDisplayMode;
        public static IEnumerable RootMenuItems;
        public static MainView mv;
        public MainView()
        {
            InitializeComponent();
            mv = this;
            AppTitleBar.PointerPressed += AppTitleBar_PointerPressed;
            DataContext = ViewModel;
        }

        private void MainView_Initialized(object? sender, EventArgs e)
        {
            FrameView.Navigate(typeof(HomeView));
        }

        private void AppTitleBar_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            MainWindow.win.BeginMoveDrag(e);
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
            RootMenuItems = RootNavigationView.MenuItems;
            RootNavigationView.BackRequested += RootNavigationView_BackRequested;
            RootNavigationView.ItemInvoked += RootNavigationView_ItemInvoked;
            RootNavigationView.DisplayModeChanged += RootNavigationView_DisplayModeChanged;
            RootNavigationView.PaneOpened += RootNavigationView_PaneOpened;
            RootNavigationView.PaneClosed += RootNavigationView_PaneClosed;
            FrameView.Navigated += FrameView_Navigated;
        }

        private async void FrameView_Navigated(object sender, FluentAvalonia.UI.Navigation.NavigationEventArgs e)
        {
            try
            {
                foreach (NavigationViewItem item in RootMenuItems)
                {
                    if ((string)item.Tag! == e.SourcePageType.Name)
                    {
                        item.IsSelected = true;
                    }
                }
                await Dispatcher.UIThread.InvokeAsync(() => ((Page)e.Content).OnNavigatedTo());
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("´íÎó", ex.ToString(), InfoBarSeverity.Error);
            }
        }

        private void RootNavigationView_PaneClosed(NavigationView sender, EventArgs args) =>
            UpdateAppTitleMargin();

        private void RootNavigationView_PaneOpened(NavigationView sender, EventArgs args) =>
            UpdateAppTitleMargin();

        private void RootNavigationView_ItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            var info = new EntranceNavigationTransitionInfo();
            info.FromVerticalOffset = 100;
            RootNavigationView.IsBackEnabled = true;
            var res = FrameView.Navigate(e.IsSettingsInvoked ? typeof(SettingView) : 
                (Type.GetType($"WonderLab.Views.{((NavigationViewItem)e.InvokedItemContainer).Tag ??= string.Empty}")) ?? typeof(HomeView),null, info);

            if (!res) {
                MainWindow.ShowInfoBarAsync("´íÎó", "giao", InfoBarSeverity.Error);
            }
        }

        private void RootNavigationView_DisplayModeChanged(object? sender, NavigationViewDisplayModeChangedEventArgs e)
        {
            NavigationViewDisplayMode = RootNavigationView.DisplayMode;

            Thickness currMargin = AppTitleBar.Margin;
            if (RootNavigationView.DisplayMode == NavigationViewDisplayMode.Minimal)
                AppTitleBar.Margin = new Thickness((RootNavigationView.CompactPaneLength * 2), currMargin.Top, currMargin.Right, currMargin.Bottom);
            else
                AppTitleBar.Margin = new Thickness(RootNavigationView.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);

            if (e.DisplayMode is NavigationViewDisplayMode.Minimal)
            {
                AppTitle.Margin = new(0, 15, 0, 0);
            }
            else if (e.DisplayMode is NavigationViewDisplayMode.Compact)
            {
                AppTitle.Margin = new(24, 15, 0, 0);
            }

            if (RootNavigationView.DisplayMode == NavigationViewDisplayMode.Minimal)
                FrameView.Margin = new Thickness(0, -50, 0, 0);
            else FrameView.Margin = new Thickness(0, -80, 0, 0);
        }

        private void RootNavigationView_BackRequested(object? sender, NavigationViewBackRequestedEventArgs e)
        {
            if (FrameView.BackStack.Count > 0) {
                FrameView.GoBack(new EntranceNavigationTransitionInfo());
                if (FrameView.BackStack.Count == 0) {
                    RootNavigationView.IsBackEnabled = false;
                }
            }
        }
    }

    partial class MainView
    {
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (e.Root is Window b)
            {
                if (!b.IsActive)
                    b.Opened += OnParentWindowOpened;
                else
                    OnParentWindowOpened(b, null);
            }
        }

        private void OnParentWindowOpened(object sender, EventArgs e)
        {
            if (e != null)
                (sender as Window).Opened -= OnParentWindowOpened;

            if (sender is CoreWindow cw)
            {
                var titleBar = cw.TitleBar;
                if (titleBar != null)
                {
                    titleBar.ExtendViewIntoTitleBar = true;

                    if (this.FindControl<Grid>("AppTitleBar") is Grid g) {                    
                        cw.SetTitleBar(g);
                    }
                }
            }
        }

        private void UpdateAppTitleMargin()
        {
            if (RootNavigationView.DisplayMode is NavigationViewDisplayMode.Minimal || RootNavigationView.DisplayMode is NavigationViewDisplayMode.Compact)
            {}            
            else
            {
                if (RootNavigationView.IsPaneOpen)
                    AppTitle.Margin = new(0, 15, 24, 0);
                else
                    AppTitle.Margin = new(24, 15, 0, 0);
            }
        }
    }
}