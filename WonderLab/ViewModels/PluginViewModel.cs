using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PluginLoader;
using ReactiveUI;
using WonderLab.Modules.Base;
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
                tmp.Add(new PluginItemViewModel
                {
                    Name = plugin.GetPluginInfo().Name,
                    Description = plugin.GetPluginInfo().Description,
                    Guid = plugin.GetPluginInfo().Guid,
                    Version = plugin.GetPluginInfo().Version,
                    Author = plugin.GetPluginInfo().Author
                }) ;
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
    }
}