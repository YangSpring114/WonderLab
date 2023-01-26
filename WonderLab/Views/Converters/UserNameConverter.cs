using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab.Views.Converters
{
    /// <summary>
    /// 用户名值转换器
    /// </summary>
    public class UserNameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is null)
                return LanguageToolkit.GetText("NotSelectedUser");

            try
            {
                return ((UserDataModels)value).UserName;
            }
            catch { }

            return LanguageToolkit.GetText("NotSelectedUser");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
