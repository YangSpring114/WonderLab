using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using PluginLoader;
using ReactiveUI;
using WonderLab.Modules.Base;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
	public class PluginItemViewModel : ViewModelBase
	{
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
    }
}