using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PluginLoader;
using ReactiveUI;
using WonderLab.Modules.Base;

namespace WonderLab.ViewModels
{
	public class PluginItemViewModel : ViewModelBase
	{
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Guid { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}