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
using System.Diagnostics;
using ReactiveUI;

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

        public override void OnNavigatedTo()
        {
            FrameView.Navigate((Type.GetType($"WonderLab.Views.DownView")) ?? typeof(NewsView));
        }

        private void BlessingView_Initialized(object? sender, EventArgs e)
        {
            FrameView.Navigate((Type.GetType($"WonderLab.Views.DownView")) ?? typeof(NewsView));
        }

        private void InitializeComponent()
        {
            InitializeComponent(true);
            FrameView.Navigated += FrameView_Navigated;
            RootNavigationView.ItemInvoked += RootNavigationView_ItemInvoked;
        }

        private void FrameView_Navigated(object sender, NavigationEventArgs e)
        {
            try {
                foreach (NavigationViewItem item in RootNavigationView.MenuItems) {                
                    if ((string)item.Tag! == e.SourcePageType.Name) {                    
                        item.IsSelected = true;
                    }
                }
            }
            catch (Exception ex) {
                Trace.WriteLine(ex.ToString());
            }
        }

        private void RootNavigationView_ItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            FrameView.Navigate((Type.GetType($"WonderLab.Views.{((NavigationViewItem)e.InvokedItemContainer).Tag ??= string.Empty}")) ?? typeof(NewsView));
        }
    }
}
