using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;

namespace WonderLab.Views
{
    public partial class NewItemView : Page
    {
        public NewItemView()
        {
            InitializeComponent();
        }

        public NewItemView(string uri,string title,string message,string data,string uri1)
        {
            InitializeComponent();
            LoadImage(uri);
            mTitle.Text = title;
            Message.Text= message;
            link.Content = uri1;
            IsOk.Header = title;
            link.NavigateUri = new(uri1);
            Date.Text= string.Format("{0}{1}","发布日期：",data);
        }

        async void LoadImage(string uri)
        {
            try
            {
                using HttpClient hc = new();
                var newsimage = await Task.Run(async () =>
                {
                    var stream1 = await hc.GetByteArrayAsync("https://launchercontent.mojang.com/" + uri);
                    MemoryStream stream = new(stream1);
                    return stream;
                });
                Bitmap bitmap = new(newsimage);
                image.Source = bitmap;
            }
            catch (Exception)
            {

            }
        }
    }
}
