using Avalonia.Data.Converters;
using Avalonia.Media;
using MinecraftLaunch.Modules.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Toolkits;

namespace WonderLab.Views.Converters
{
    public class ErrorLogConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            IBrush brush = null;
            var res = ((GameLogType)value);

            if (res is GameLogType.Fatal || res is GameLogType.Exception || res is GameLogType.StackTrace || res is GameLogType.Unknown) {
                brush = Brushes.Red;
            }

            return brush;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new();
    }
}
