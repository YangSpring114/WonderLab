using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;

namespace WonderLab.Views
{
    public partial class NewsView : Page
    {
        //https://launchercontent.mojang.com/news.json
        public NewsView()
        {
            InitializeComponent();
            LoadNews();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            newslist = this.Find<StackPanel>("newslist");
            Isok = this.Find<StackPanel>("Isok");
        }
        List<NewItemView> newItemViews = new List<NewItemView>();
        public async void LoadNews()
        {
            using HttpClient hc = new();
            try
            {
                var json = await hc.GetStringAsync("https://launchercontent.mojang.com/news.json");
                var v = JsonConvert.DeserializeObject<NewsModels>(json);
                foreach (var i in v.entries)
                {
                    NewItemView newItem = new(i.newsPageImage.url, i.title, i.text, i.date, i.readMoreLink);
                    newItem.IsOk.Description = i.tag + " 快讯";
                    newItemViews.Add(newItem);
                    if (newItemViews.Count is 20)
                        break;
                }
            }
            catch (System.Exception)
            {

            }
            finally
            {
                Isok.IsVisible = false;
                hc.Dispose();
                await Task.Run(async() =>
                {
                    foreach (var i in newItemViews)
                        await Dispatcher.UIThread.InvokeAsync(() => { newslist.Children.Add(i); }, DispatcherPriority.Background);
                });
            }
        }
    }
}
