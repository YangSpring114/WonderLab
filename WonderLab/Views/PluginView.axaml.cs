using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PluginLoader;
using System.Collections.Generic;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class PluginView : Page
    {
        public static PluginViewModel ViewModel { get; } = new();
        public PluginView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
