using Avalonia.Data.Converters;
using Avalonia.Media;
using MinecraftLaunch.Modules.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using GameLogType = MinecraftLaunch.Modules.Enum.GameLogType;

namespace WonderLab.Views.Converters
{
    public class LogBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            IBrush brush = null;
            var Tvalue = ((GameLogType)value);
            if (Tvalue is GameLogType.Info)
                brush = Brushes.Green;
            else if (Tvalue is GameLogType.Error)
                brush = Brushes.Orange;
            else if (Tvalue is GameLogType.Warning)
                brush = Brushes.Yellow;
            else if (Tvalue is GameLogType.Debug)
                brush = Brushes.Gray;
            else if (Tvalue is GameLogType.Fatal || Tvalue is GameLogType.Exception || Tvalue is GameLogType.StackTrace || Tvalue is GameLogType.Unknown)
                brush = Brushes.Red;
            return brush;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
