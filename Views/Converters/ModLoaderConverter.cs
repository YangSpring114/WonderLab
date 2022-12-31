using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.ViewModels;

namespace WonderLab.Views.Converters
{
    public class ModLoaderConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            try
            {
                var con = (ModLoaderInformationViewData)value!;
                StringBuilder stringBuilder = new();
                stringBuilder.Append($"版本号：{con.Data.Version} - {con.Data.McVersion}");
                stringBuilder.Append("发布于 ");
                stringBuilder.Append(con.Data.ReleaseTime.ToString() + ", ");
                //stringBuilder.Append("种类：installer");
                Debug.WriteLine(stringBuilder.ToString());
                return stringBuilder.ToString();
            }
            catch
            {

            }
            return "666";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
