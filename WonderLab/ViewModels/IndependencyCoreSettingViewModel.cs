using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    public partial class IndependencyCoreSettingViewModel : ViewModelBase
    {
        public string WindowWidth
        {
            get => _WindowWidth;
            set
            {
                if (RaiseAndSetIfChanged(ref _WindowWidth, value) && Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$") && !string.IsNullOrEmpty(value))
                {
                    var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore());
                    res.WindowWidth = Convert.ToInt32(value);
                    JsonToolkit.WriteEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), res);
                }
            }
        }
        public string WindowHeight
        {
            get => _WindowHeight;
            set
            {
                if (RaiseAndSetIfChanged(ref _WindowHeight, value) && Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$") && !string.IsNullOrEmpty(value))
                {
                    var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore());
                    res.WindowHeight = Convert.ToInt32(value);
                    JsonToolkit.WriteEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), res);
                }
            }
        }
        public string Jvm
        {
            get => _Jvm;
            set
            {
                if (RaiseAndSetIfChanged(ref _Jvm, value) && !string.IsNullOrEmpty(value))
                {
                    var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore());
                    res.Jvm = value;
                    JsonToolkit.WriteEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), res);
                }
            }
        }
        public bool IsExpand { get => _IsExpand; set => RaiseAndSetIfChanged(ref _IsExpand, value); }
        public bool IsEnableIndependencyCore 
        {
            get => _IsEnableIndependencyCore; 
            set 
            {
                if (RaiseAndSetIfChanged(ref _IsEnableIndependencyCore, value))
                {
                    var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore());
                    if (res != null)
                    {
                        res.IsEnableIndependencyCore = value;
                        JsonToolkit.WriteEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), res);
                    }
                    else
                    {
                        JsonToolkit.CreaftEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), value);
                    }
                    IsExpand = value; 
                }
            }
        }
        public bool IsFullWindow
        {
            get => _IsFullWindow;
            set
            {
                if (RaiseAndSetIfChanged(ref _IsFullWindow, value))
                {
                    var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore());
                    res.IsFullWindows = value;
                    JsonToolkit.WriteEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), res);
                }
            }
        }
        public bool IsOlate
        {
            get => _IsOlate;
            set
            {
                if (RaiseAndSetIfChanged(ref _IsOlate, value))
                {
                    var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore());
                    res.Isolate = value;
                    JsonToolkit.WriteEnableIndependencyCoreInfoJson(App.Data.FooterPath, IndependencyCoreSettingView.GameCore.ToNatsurainkoGameCore(), res);
                }
            }
        }
    }

    partial class IndependencyCoreSettingViewModel
    {
        public string _Jvm = string.Empty;
        public string _WindowHeight = "480";
        public string _WindowWidth = "854";
        public bool _IsEnableIndependencyCore = false;
        public bool _IsExpand = false;
        public bool _IsFullWindow = false;
        public bool _IsOlate = false;
    }
}
