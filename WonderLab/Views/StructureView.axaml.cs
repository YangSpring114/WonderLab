using Avalonia.Controls;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class StructureView : Page
    {
        public StructureViewModel ViewModel { get; set; } = new();
        public StructureView()
        {
            InitializeComponent(true);
            DataContext = ViewModel;
        }
    }
}
