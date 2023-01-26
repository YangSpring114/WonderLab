using Avalonia.Data.Converters;
using MinecraftLaunch.Modules.Models.Launch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab.Views.Converters
{
    public class GameCoreNameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return LanguageToolkit.GetText("NotSelectedGameCore");
            
            try
            {
                var gamecoreid = ((GameCore)value).Id;
                return gamecoreid;
            }
            catch { }

            return LanguageToolkit.GetText("NotSelectedGameCore");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new();
    }
}
