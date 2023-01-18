using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Toolkits;

namespace WonderLab.Views.Converters
{
    public class ModIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            using var hc = new HttpClient();
            var stream = hc.GetByteArrayAsync(value.ToString()).Result;
            var memoryStream = new MemoryStream(stream);
            return new Bitmap(memoryStream);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
