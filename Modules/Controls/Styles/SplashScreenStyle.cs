using Avalonia.Media;
using Avalonia.Platform;
using Avalonia;
using FluentAvalonia.Core.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using WonderLab.Modules.Toolkits;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using WonderLab.Modules.Models;
using WonderLab.Modules.Const;
using Avalonia.Input.Platform;
using static System.Net.Mime.MediaTypeNames;
using WonderLab.Views;

namespace WonderLab.Modules.Styles
{
    public class SplashScreenStyle : IApplicationSplashScreen
    {
        public SplashScreenStyle()
        {
            var al = AvaloniaLocator.Current.GetService<IAssetLoader>();
            using (var s = al.Open(new Uri("resm:WonderLab.Resources.Icon.ico")))
                AppIcon = new Bitmap(s);
        }

        string IApplicationSplashScreen.AppName { get; }

        public IImage AppIcon { get; }

        object IApplicationSplashScreen.SplashScreenContent { get; }

        int IApplicationSplashScreen.MinimumShowTime => 2000;

        async void IApplicationSplashScreen.RunTasks()
        {

        }
    }
}
