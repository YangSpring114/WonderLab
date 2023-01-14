using Avalonia.Controls;
using MinecraftLaunch.Modules.Toolkits;
using System.Threading.Tasks;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

namespace WonderLab.Views
{
    public partial class ModPropertyView : Page
    {
        public static ModPropertyViewModel ViewModel { get; protected set; } = new();
        public ModPropertyView()
        {
            InitializeComponent(true);
            DataContext = ViewModel;
        }

        private void Hyperlink_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            NavigatedToDownView();
        }

        public override async void OnNavigatedTo()
        {
            await Task.Run(() =>
            {
                var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, ModPropertyViewModel.SelectedGameCore.ToNatsurainkoGameCore());
                bool isolate = App.Data.Isolate;
                if (res != null && res.IsEnableIndependencyCore)
                {
                    isolate = res.Isolate;
                }
                //(res is not null && !res.Isolate) || !App.Data.Isolate
                if (!isolate)
                {
                    ViewModel.Isolate = true;
                }
                else ViewModel.Isolate = false;

                ViewModel.Toolkit = new(ModPropertyViewModel.SelectedGameCore, false, isolate, App.Data.FooterPath);
                ModDataModel.SetToolkit(ViewModel.Toolkit);
                ViewModel.LoadModList();
            });
        }
    }
}
