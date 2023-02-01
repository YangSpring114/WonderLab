using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using SkiaSharp;
using System;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class DownSettingView : Page
    {
        public DownSettingView()
        {
            InitializeComponent(true);
            DataContext = new DownSettingViewModel();
        }
        public async void OpenFolderDialog(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                OpenFolderDialog dialog;
                dialog = new OpenFolderDialog
                {
                    Title = "请选择下载路径",
                    DefaultDirectory = Environment.CurrentDirectory
                };

                var result = await dialog.ShowAsync(MainWindow.win);

                if (result is not null)
                {
                    App.Data.CustomDownloadPath = result;
                    ((DownSettingViewModel)DataContext).DownloadPath = result;
                }
                JsonToolkit.JsonWrite();
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误：", $"发生了意想不到的错误：\n{ex}", InfoBarSeverity.Error);
            }
        }
    }
}


    //resm:WonderLab.Resources.HarmonyOS#HarmonyOS Sans