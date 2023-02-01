using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using Avalonia.Media.Imaging;
using PluginLoader;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using WonderLab.Modules.Base;
using WonderLab.PluginAPI;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
	public class PluginViewModel : ViewModelBase
    {
        public PluginViewModel()
        {
            var tmp = new ObservableCollection<PluginItemViewModel>();
            foreach (var plugin in PluginLoader.PluginLoader.Plugins)
            {
                byte[]? icon = null;
                Bitmap Pic;
                if (plugin.GetPluginInfo().Icon != null)
                {
                    icon = Convert.FromBase64String(plugin.GetPluginInfo().Icon);
                }
                using Stream stream = new MemoryStream();
                Image image = new Image<Rgba32>(64, 64);
                if (icon != null)
                {
                    image = Image.Load(icon);
                }
                stream.Position = 0;
                image.Save(stream, new PngEncoder());
                stream.Position = 0;
                Pic = new Bitmap(stream);
                tmp.Add(new PluginItemViewModel
                {
                    Name = plugin.GetPluginInfo().Name,
                    Description = plugin.GetPluginInfo().Description,
                    Guid = plugin.GetPluginInfo().Guid,
                    Version = plugin.GetPluginInfo().Version,
                    Author = plugin.GetPluginInfo().Author,
                    Icon = Pic
                });
            }
            Plugins = tmp;
            HasPlugin = PluginLoader.PluginLoader.GetPlugins().Length != 0;
        }
        private bool _HasPlugin;
        public bool HasPlugin
        {
            get => _HasPlugin;
            set => this.RaiseAndSetIfChanged(ref _HasPlugin, value);
        }
        public bool UnHasPlugin
        {
            get => !HasPlugin;
        }
        private ObservableCollection<PluginItemViewModel> _Plugins = new();
        public ObservableCollection<PluginItemViewModel> Plugins
        {
            get => _Plugins;
            set => this.RaiseAndSetIfChanged(ref _Plugins, value);
        }
        public void SettingButtonClick(Avalonia.Interactivity.RoutedEventArgs e)
        {
            //var plugin = PluginLoader.PluginLoader.GetPlugin(pluginItem.Name);
            //if(plugin is WonderLabPlugin)
            //{
            //    ((WonderLabPlugin)plugin).OnSettingButtonClick();
            //}
        }
    }
}