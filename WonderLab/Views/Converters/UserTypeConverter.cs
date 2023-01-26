using Avalonia.Data.Converters;
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
    /// <summary>
    /// 用户类型转换器
    /// </summary>
    public class UserTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            try
            {
                var res = (((UserDataModels)value).UserType.ToUserTypeText());

                if (!string.IsNullOrEmpty(res)) {
                    return res;
                }
            }
            catch { }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException();

    }
}
