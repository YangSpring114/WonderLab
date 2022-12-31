using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;

namespace WonderLab.ViewModels
{
    public class MainPropertyViewModel : ViewModelBase
    {
        public bool _IsHasModLoader = false;
        public string _Id = "";
        public string _Type = "";

        public bool IsHasModLoader
        {
            get => _IsHasModLoader;
            set => RaiseAndSetIfChanged(ref _IsHasModLoader, value);
        }

        public string Type
        {
            get => _Type;
            set=> RaiseAndSetIfChanged(ref _Type, value);
        }

        public string Id
        {
            get => _Id;
            set => RaiseAndSetIfChanged(ref _Id, value);
        }
    }
}
