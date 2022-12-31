using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;

namespace WonderLab.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public int AllTaskCount
        {
            get => _allTaskCount;
            set
            {
                if (RaiseAndSetIfChanged(ref _allTaskCount, value))
                {
                    IsBadgeVisible = _allTaskCount <= 0 ? false : true;
                }
            }
        }

        public bool IsBadgeVisible
        {
            get => _IsBadgeVisible;
            set => RaiseAndSetIfChanged(ref _IsBadgeVisible, value);
        }
    }

    partial class MainViewModel
    {
        private bool _IsBadgeVisible = false;
        private int _allTaskCount = 0;
    }

    partial class MainViewModel
    {

    }
}
