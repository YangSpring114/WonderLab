using Avalonia.Controls;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class ExplosionRadioView : Page
    {
        public ExplosionRadioView() {
       
            InitializeComponent();
            DataContext = new ExplosionRadioViewModel();
        }
    }
}
