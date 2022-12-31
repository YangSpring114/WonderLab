using Avalonia.Collections;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Threading;
using Avalonia;
using DynamicData;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace WonderLab.Modules.Controls
{
    public partial class FrameX : Frame
    {
        public void NavigateTo(Page page)
        {
            page.OnNavigatedTo();
        }

        public bool Navigate(Page sourcePageType) => Navigate(sourcePageType, sourcePageType.GetType(), null, null);

        public bool Navigate(Page sourcePageType, object parameter) => Navigate(sourcePageType, sourcePageType.GetType(),parameter, null);

        public bool Navigate(Page sourcePageType, Type type, object parameter, NavigationTransitionInfo infoOverride)
        {
            //sourcePageType.OnNavigatedTo();
            var v = Navigate(type, parameter, infoOverride);
            return v;
        }
    }
}