using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Views.Converters
{
    public class LoaderEnabledConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var Ivalue = value.ToString();
            if (Ivalue.Contains("此加载器与 Forge 和 Opitfine 不兼容") || Ivalue.Contains("此加载器与 Forge 不兼容") ||
                Ivalue.Contains("此加载器与 Forge 和 Opitfine 不兼容") || Ivalue.Contains("此加载器与 OptiFine 不兼容"))//Fabric
                return false;
            else if (Ivalue.Contains("加载中..."))
                return false;
            else if (Ivalue.Contains("此加载器与 Fabric 不兼容"))//Forge and Optifine
                return false;
            else if (Ivalue.Contains("没有可用版本"))
                return false;
            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
