using Avalonia.Data.Converters;
using Natsurainko.FluentCore.Class.Model.Install;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.ViewModels;

namespace WonderLab.Views.Converters
{
    public class LoaderConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            object? result = null;
            if (value is null)
                return "未选择任何版本";

            var res = (ModLoaderInformationViewData)value;

            if (res.Data.CanSelected is 0)
                result = res.Data.Version;
            else if(res.Data.CanSelected is 1)
                result = "此加载器无法与OptiFine或Forge同时安装";
            else
                result = "此加载器无法与Fabric同时安装";

            return result;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
