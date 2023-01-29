using MinecraftLaunch.Modules.Models.Download;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;
using Avalonia.Media.Imaging;

namespace WonderLab.ViewModels
{
    public partial class ResourceViewModel : ViewModelBase
    {
        public ObservableCollection<ResourcePackViewData<ResourcePack>> DisbaledResourcePacks
        {
            get => _DisbaledResourcePacks;
            set => RaiseAndSetIfChanged(ref _DisbaledResourcePacks, value);
        }

        public ObservableCollection<ResourcePackViewData<ResourcePack>> EnabledResourcePacks
        {
            get => _EnabledResourcePacks;
            set => RaiseAndSetIfChanged(ref _EnabledResourcePacks, value);
        }
    }

    partial class ResourceViewModel
    {
        /// <summary>
        /// 加载所有资源包
        /// </summary>
        public async void LoadAllPacksAction()
        {
            var res = JsonToolkit.GetEnableIndependencyCoreData(App.Data.FooterPath, PropertyView.GameCore.ToNatsurainkoGameCore());
            bool isolate = App.Data.Isolate;
            if (res != null && res.IsEnableIndependencyCore)
            {
                isolate = res.Isolate;
            }

            if (!isolate)
            {
                //ViewModel.Isolate = true;
            }
            //else ViewModel.Isolate = false;

            ResourcePackToolkit toolkit = new(PropertyView.GameCore, false, isolate, App.Data.FooterPath);
            var AllPack = await toolkit.LoadAllAsync();

            EnabledResourcePacks = AllPack.Where(x => x.IsEnabled).Select(x => x.CreateViewData<ResourcePack, ResourcePackViewData<ResourcePack>>()).BuildObservableCollection();
            DisbaledResourcePacks = AllPack.Where(x => !x.IsEnabled).Select(x => x.CreateViewData<ResourcePack, ResourcePackViewData<ResourcePack>>()).BuildObservableCollection();

            foreach (var i in EnabledResourcePacks) {
                i.PackLogo = GetImage(i.Data.Path);
            }

            foreach (var i in DisbaledResourcePacks) {
                i.PackLogo = GetImage(i.Data.Path);
            }
        }

        Bitmap GetImage(string file)
        {
            string fileName = Path.GetFileName(file);
            bool flag = file.EndsWith(".zip");
            MemoryStream memoryStream2 = new MemoryStream();
            if (flag)
            {
                using ZipArchive zipArchive = ZipFile.OpenRead(file);

                ZipArchiveEntry entry2 = zipArchive.GetEntry("pack.png");
                if (entry2 != null)
                {
                    using Stream stream2 = entry2.Open();
                    stream2.CopyTo(memoryStream2);

                    if (memoryStream2.Length > 0) {
                        memoryStream2.Position= 0;
                        return new(memoryStream2);
                    }
                }
            }
            else
            {
                return new(Path.Combine(file, "pack.png"));
            }

            return null;
        }

        private void ResourceViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Trace.WriteLine($"[信息] 改变的属性名：{e.PropertyName}");
        }
    }

    partial class ResourceViewModel
    {
        public ResourceViewModel() => PropertyChanged += ResourceViewModel_PropertyChanged;        
        public ObservableCollection<ResourcePackViewData<ResourcePack>> _DisbaledResourcePacks = null;
        public ObservableCollection<ResourcePackViewData<ResourcePack>> _EnabledResourcePacks = null;
    }
}
