using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WonderLab.Modules.Base;
using WonderLab.Modules.Controls;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class GameItemView : Page
    {
        public GameItemView(GameItemViewModel model) => InitializeComponent(model);
        public GameItemView() => InitializeComponent();

        private void InitializeComponent(GameItemViewModel model)
        {
            InitializeComponent(true);
            this.DataContext = model;
            PointerEnter += model.MoveInAction;
            PointerLeave += model.MoveOutAction;
        }
    }
}
