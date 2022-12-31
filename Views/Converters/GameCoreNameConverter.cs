using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;

namespace WonderLab.Views.Converters
{
    public class GameCoreNameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return "未选择任何游戏核心";

            try
            {
                var gamecoreid = ((TransformationModel)value).Id;
                return gamecoreid;
            }
            catch { }

            return "未选择任何游戏核心";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new();
    }
}
