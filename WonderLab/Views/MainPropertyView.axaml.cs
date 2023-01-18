using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Diagnostics;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class MainPropertyView : Page
    {
        public static GameCore SelectGameCore { get; set; }

        public static MainPropertyViewModel ViewModel { get; set; } = new();

        public MainPropertyView()
        {
            InitializeComponent(true);
            DataContext = ViewModel;
            contentFrame.Navigated += ContentFrame_Navigated;
            NavigationView.ItemInvoked += NavigationView_ItemInvoked;
        }

        public override void OnNavigatedTo()
        {
            PropertyViewInitialize(this);
            ViewModel.Id = SelectGameCore.Id;
            ViewModel.Type = SelectGameCore.Type;//SelectGameCore.HasModLoader is true ? $"{SelectGameCore.Type} ¼Ì³Ð×Ô {SelectGameCore.Source}" : $"{SelectGameCore.Type} {SelectGameCore.Source}";
            ViewModel.IsHasModLoader = SelectGameCore.HasModLoader;
        }

        private void NavigationView_ItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            try
            {
                if (((NavigationViewItem)e.InvokedItemContainer).Tag.ToString() == "ModPropertyView")
                    ModPropertyViewModel.SelectedGameCore = SelectGameCore ??= GameCoreToolkit.GetGameCore(App.Data.FooterPath, ViewModel.Id);
                else if (((NavigationViewItem)e.InvokedItemContainer).Tag.ToString() == "IndependencyCoreSettingView")
                    IndependencyCoreSettingView.GameCore = SelectGameCore ??= GameCoreToolkit.GetGameCore(App.Data.FooterPath, ViewModel.Id);

                contentFrame.Navigate((Page)contentFrame.Content, (Type.GetType($"WonderLab.Views.{((NavigationViewItem)e.InvokedItemContainer).Tag ??= string.Empty}")), null, null);
            }
            catch (Exception)
            {

            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (NavigationViewItem item in NavigationView.MenuItems)
            {
                if ((string)item.Tag == e.SourcePageType.Name)
                {
                    NavigationView.SelectedItem = item;
                    item.IsSelected = true;
                    contentFrame.NavigateTo((Page)e.Content);
                    return;
                }
            }
        }
    }
}
