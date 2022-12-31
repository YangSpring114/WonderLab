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
    public class CurseForgeModpackTagConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            try
            {
                var modpack = (CurseForgeModel)value;
                var timeSpan = DateTime.Now - modpack.LastUpdateTime;
                var types = new List<int>();

                modpack.Files.Values.ToList().ForEach(x => x.ForEach(y =>
                {
                    if (y.ModLoaderType != null && !types.Contains((int)y.ModLoaderType))
                        types.Add((int)y.ModLoaderType);
                }));

                var modLoaderTypes = types.Select(x => x switch
                {
                    0 => "All",
                    1 => "Forge",
                    2 => "Cauldron",
                    3 => "LiteLoader",
                    4 => "Fabric",
                    _ => string.Empty
                }).Where(x => !string.IsNullOrEmpty(x));

                var builder = new StringBuilder()
                    .Append(modLoaderTypes.Any() ? $"[{string.Join(',', modLoaderTypes)}]" : string.Empty);

                var timeBuilder = new StringBuilder()
                    .Append(timeSpan.Days != 0 ? $"{timeSpan.Days} 天" : string.Empty)
                    .Append(timeSpan.Hours != 0 ? $" {timeSpan.Hours} 小时" : string.Empty);

                string downloadCount = modpack.DownloadCount > 1000
                    ? $"{modpack.DownloadCount / 1000}k"
                    : modpack.DownloadCount.ToString();

                builder = builder
                    .Append(builder.Length > 0 ? " " : string.Empty)
                    .Append(modpack.SupportedVersions.Any() ? $"[{modpack.SupportedVersions.First()}{(modpack.SupportedVersions.First() == modpack.SupportedVersions.Last() ? string.Empty : $"-{modpack.SupportedVersions.Last()}")}]" : string.Empty)
                    .Append(" ")
                    .Append($"{timeBuilder.ToString()} 前更新，")
                    .Append($"{downloadCount} 次下载");

                return builder.ToString();
            }
            catch
            {
                return null;
            }

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
