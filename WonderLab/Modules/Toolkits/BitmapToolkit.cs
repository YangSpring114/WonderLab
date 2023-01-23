using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public static IImage CropImage(Bitmap originImage)
        {
            var v = originImage.CreateScaledBitmap(new(), Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.Default);
            return v;
        }

        public static void RenderToFile(Geometry geometry, Brush brush, string path)
        {
            var control = new DrawingPresenter()
            {
                Drawing = new GeometryDrawing
                {
                    Geometry = geometry,
                    Brush = brush,
                },
                Width = geometry.Bounds.Right,
                Height = geometry.Bounds.Bottom
            };

            RenderToFile(control, path);
        }

        public static void RenderToFile(Control target, string path)
        {
            var pixelSize = new PixelSize((int)target.Width, (int)target.Height);
            var size = new Size(target.Width, target.Height);
            using (RenderTargetBitmap bitmap = new RenderTargetBitmap(pixelSize, new Vector(96, 96)))
            {
                target.Measure(size);
                target.Arrange(new Rect(size));
                bitmap.Render(target);
                bitmap.Save(path);
            }
        }
        /// <summary>
        /// 获取资源图片
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Avalonia.Media.IImage GetAssetsImage(string uri)
        {
            var al = AvaloniaLocator.Current.GetService<IAssetLoader>();
            using (var s = al.Open(new Uri(uri)))
                return new Bitmap(s);
        }
        public static Image<TPixel> ResizeImage<TPixel>(Image<TPixel> image, int w, int h) where TPixel : unmanaged, IPixel<TPixel>
        {
            Image<TPixel> image2 = new(w, h);
            for(int i = 0;i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int realW = image.Width / w * (i + 1);
                    int realH = image.Height / h * (j + 1);
                    image2[i, j] = image[realW, realH];
                }
            }
            return image2;
        }
    }
}
