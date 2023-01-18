using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media;
using FluentAvalonia.UI.Navigation;
using FluentAvalonia.UI.Media.Animation;
using WonderLab.Modules.Controls;
using System;
using WonderLab.Modules.Media;

namespace WonderLab.Views
{
    public partial class BlessingView : Page
    {
        [Obsolete] public static bool IsDown = false;
        [Obsolete] public static bool IsTask = false;
        public static BlessingView view;
        public BlessingView()
        {
            InitializeComponent();
            view = this;
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
            RootNavigationView.ItemInvoked += RootNavigationView_ItemInvoked;
        }

        private void RootNavigationView_ItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            FrameView.Navigate((Type.GetType($"WonderLab.Views.{((NavigationViewItem)e.InvokedItemContainer).Tag ??= string.Empty}")) ?? typeof(NewsView), null, null);
        }
    }
}
