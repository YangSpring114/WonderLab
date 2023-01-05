using Avalonia.Data.Converters;
using MinecraftLaunch.Modules.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Views.Converters
{
    public class PrefixConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var res = (GameLogType)value;
            if (res is GameLogType.Exception || res is GameLogType.StackTrace || res is GameLogType.Unknown)
                return false;

            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
