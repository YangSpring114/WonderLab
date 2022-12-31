using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class OtherView : Page
    {
        public OtherView()
        {
            InitializeComponent(true);
            DataContext = new OtherViewModel();
        }
    }
}
