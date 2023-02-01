using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using PluginLoader;
using ReactiveUI;
using WonderLab.Modules.Base;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
	public class PluginItemViewModel : ViewModelBase
	{
        public PluginItemViewModel()
        {
            thread.Start(this);
        }
        private Thread thread = new(new ParameterizedThreadStart(ChangeWidth));
        public static async void ChangeWidth(object? PluginItemViewModel)
        {
            var obj = (PluginItemViewModel)PluginItemViewModel;
            while (true)
            {
                if (stop)
                {
                    break;
                }
                int tmp = 0;
                await Dispatcher.UIThread.InvokeAsync(() => (tmp = (int)(MainWindow.win.Width)));
                if(tmp > 1000)
                {
                    obj.Width = tmp - 300;
                }
                else if(tmp > 900)
                {
                    obj.Width = tmp - 250;
                }
                else if (tmp > 800)
                {
                    obj.Width = tmp - 200;
                }
                else if (tmp > 700)
                {
                    obj.Width = tmp - 150;
                }
                else if (tmp > 600)
                {
                    obj.Width = tmp - 100;
                }
                else if (tmp > 500)
                {
                    obj.Width = tmp - 80;
                }
                else
                {
                    obj.Width = tmp - 50;
                }
                Thread.Sleep(50);
            }
        }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Guid { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public Bitmap? Icon { get; set; }
        private int _Width = (int) (MainWindow.win.Width - 200);
        public int Width
        {
            get => _Width;
            set
            {
                if (RaiseAndSetIfChanged(ref _Width, value))
                {
                    //Debug.WriteLine(value.Count);
                }
            }
        }
        public static bool stop = false;
    }
}