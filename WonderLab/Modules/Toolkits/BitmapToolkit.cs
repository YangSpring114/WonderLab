using Avalonia;
using Avalonia.Platform;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.IO;
using IImage = Avalonia.Media.IImage;
using Avalonia.Media.Imaging;
using System;
using SixLabors.ImageSharp.Processing;
using WonderLab.ViewModels;
using SixLabors.ImageSharp.Advanced;
using System.Threading.Tasks;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 图片操作工具类
    /// </summary>
    public class BitmapToolkit
    {
        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="originImage">原图片</param>
        /// <param name="region">裁剪的方形区域</param>
        /// <returns>裁剪后图片</returns>
        public static async ValueTask<Stream> CropImage(string stream)
        {
            Image<Rgba32> head = (Image<Rgba32>)Image.Load(stream);
            head.Mutate(x => x.Crop(Rectangle.FromLTRB(8, 8, 16, 16)));

            Image<Rgba32> hat = (Image<Rgba32>)Image.Load(stream);
            hat.Mutate(x => x.Crop(Rectangle.FromLTRB(40, 8, 48, 16)));

            Image<Rgba32> endImage = new Image<Rgba32>(8, 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    endImage[i, j] = head[i, j];
                    if (hat[i, j].A == 255)
                    {
                        endImage[i, j] = hat[i, j];
                    }
                }
            }

            endImage.Mutate(x => x.Resize(128, 128));

            //缓存图片
            await endImage.SaveAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),(stream.Length.ToString() + ".jpg")));
            return null;
        }

        /// <summary>
        /// 获取资源图片
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static IImage GetAssetsImage(string uri)
        {
            var al = AvaloniaLocator.Current.GetService<IAssetLoader>();
            using (var s = al.Open(new Uri(uri)))
                return new Bitmap(s);
        }
    }
}
