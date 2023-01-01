using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Enum;
using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;

namespace WonderLab.ViewModels
{
    public partial class DownModViewModel : ViewModelBase
    {
        public void Filter()
        {
            if (FilterOpacity is 0)
            {//45,20,45,0
                FilterHeight = double.NaN;
                FilterMargin = new(45, 20, 45, 0);
                FilterOpacity = 1;
            }
            else
            {//45,10,45,0
                FilterMargin = new(45, 0, 45, 0);
                FilterOpacity = 0;
                FilterHeight = 0;
            }
        }

        public async void GetHotModListAsync()
        {//分片加载
            CurseForgeToolkit curseForgeModpackFinder = new(InfoConst.CForgeToken);
            var mods = await curseForgeModpackFinder.GetFeaturedModpacksAsync();
            //ModList = new(mods.Select(x => new CurseForgeModel(x)));
            await Dispatcher.UIThread.InvokeAsync(() => ModList = mods.Select(x => new CurseForgeModel(x)).ToList());
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                IsLoadedOver = true;
                IsModsLoadOK = true;
                IsLoaded = false;
            });
            await Task.Delay(1000);
            await Dispatcher.UIThread.InvokeAsync(async () => await Task.Run(() => ModList.ForEach(x => x.IsLoadOK = true)));
        }

        public ModLoaderType ToModLoaderType()
        {
            switch (SelectModLoaderFilter)
            {
                case "Forge":
                    return ModLoaderType.Forge;
                case "Fabric":
                    return ModLoaderType.Fabric;
                case "LiteLoader":
                    return ModLoaderType.LiteLoader;
                default:
                    return ModLoaderType.Any;
                    break;
            }
        }

        public async void SearchModpacksAsync()
        {
            IsModsLoadOK = false;
            IsLoaded = true;
            CurseForgeToolkit curseForgeModpackFinder = new(InfoConst.CForgeToken);
            var filtertemp = SearchFilter;
            var res = StringToolkit.ModNameSearchCheck(filtertemp);//中文检查
            var searchFilter = res is null ? SearchFilter : res.CurseForgeId;
            var mods = await curseForgeModpackFinder.SearchModpacksAsync(searchFilter, this.ToModLoaderType(), SelectVersionFilter, SelectCategoriesFilter.Value.Value);
            Dispatcher.UIThread.Post(() => ModList = mods.Select(x => new CurseForgeModel(x)).ToList());
            Dispatcher.UIThread.Post(() =>
            {
                IsModsLoadOK = true;
                IsLoaded = false;
            });
            await Task.Delay(1000);
            Dispatcher.UIThread.Post(async () => await Task.Run(() => ModList.ForEach(x => x.IsLoadOK = true)));            
        }

        public void SearchFilterClear()
        {
            SearchFilter = string.Empty;
            SelectModLoaderFilter = string.Empty;
            SelectVersionFilter = string.Empty;
            SelectCategoriesFilter = Categories.First();
        }

        [Obsolete] public void NavigationToModInfo()
        {
            Debug.WriteLine("NB");
        }
    }

    partial class DownModViewModel
    {
        public DownModViewModel()
        {
            IsLoaded = true;
            IsLoadedOver = false;
            GetHotModListAsync();
        }
    }

    partial class DownModViewModel
    {
        public double FilterOpacity
        {
            get => _FilterOpacity;
            set => RaiseAndSetIfChanged(ref _FilterOpacity, value);
        }
        
        public Thickness FilterMargin
        {
            get => _FilterMargin;
            set => RaiseAndSetIfChanged(ref _FilterMargin, value);
        }

        public Thickness Margins
        {
            get => _FilterMargin;
            set => RaiseAndSetIfChanged(ref _FilterMargin, value);
        }

        public double FilterHeight
        {
            get => _FilterHeight;
            set => RaiseAndSetIfChanged(ref _FilterHeight, value);
        }

        public bool FilterVisible
        {
            get => _FilterVisible;
            set => RaiseAndSetIfChanged(ref _FilterVisible, value);
        }

        public bool IsLoaded
        {
            get => _IsLoaded;
            set => RaiseAndSetIfChanged(ref _IsLoaded, value);
        }

        public bool IsLoadedOver
        {
            get => _IsLoadedOver;
            set => RaiseAndSetIfChanged(ref _IsLoadedOver, value);
        }

        public bool IsModsLoadOK
        {
            get => _IsModsLoadOK;
            set => RaiseAndSetIfChanged(ref _IsModsLoadOK, value);
        }

        public List<string> VersionFilters => new()
        {
            "1.19.3",
            "1.19.2", "1.19",
            "1.18.2", "1.18",
            "1.17.1", "1.17",
            "1.16.5", "1.16",
            "1.15.2", "1.15",
            "1.14.4", "1.14",
            "1.13.2", "1.13",
            "1.12.2", "1.12",
            "1.11.2", "1.11",
            "1.10.2", "1.10",
            "1.9",   "1.8.9",            
            "1.7.10","1.6.4",
            
        };
        
        public List<string> ModLoaderFilters => new()
        {
            "Forge","Fabric","LiteLoader"
        };

        public List<CurseForgeModel> ModList
        {
            get => _ModList;
            set => RaiseAndSetIfChanged(ref _ModList, value);
        }
        
        public CurseForgeModel SelectMod
        {
            get => _SelectMod;
            set => RaiseAndSetIfChanged(ref _SelectMod, value);
        }

        public string SearchFilter
        {
            get => _SearchFilter;
            set => RaiseAndSetIfChanged(ref _SearchFilter, value);
        }

        public string SelectModLoaderFilter
        {
            get => _SelectModLoaderFilter;
            set => RaiseAndSetIfChanged(ref _SelectModLoaderFilter, value);
        }

        public string SelectVersionFilter
        {
            get => _SelectVersionFilter;
            set => RaiseAndSetIfChanged(ref _SelectVersionFilter, value);
        }

        public KeyValuePair<string,int>? SelectCategoriesFilter
        {
            get => _SelectCategoriesFilter;
            set => RaiseAndSetIfChanged(ref _SelectCategoriesFilter, value);
        }

        public Dictionary<string, int> Categories => new()
        {
            { "全部", -1 },
            { "世界生成", 406 },
            { "生物群系", 407 },
            { "天然结构", 409 },
            { "维度", 410 },
            { "生物", 411 },
            { "科技", 412 },
            { "交通与移动", 414 },
            { "管道与物流", 415 },
            { "能源", 417  },
            { "红石", 4558 },
            { "仓储", 420  },
            { "农业", 416  },
            { "食物", 436  },
            { "魔法", 419  },
            { "装备与工具", 434 },
            { "冒险与探索", 422 },
            { "建筑与装饰", 424 },
            { "信息显示", 423 },
            { "杂项", 425 },
            { "支持库", 421 },
            { "服务器", 435 },
        };

        public List<ListBoxItem> DemoList => new List<ListBoxItem>()
        {
            new ListBoxItem(),
            new ListBoxItem(),
            new ListBoxItem(),
            new ListBoxItem(),
            new ListBoxItem(),
            new ListBoxItem(),

        };
    }

    partial class DownModViewModel
    {
        public string _SearchFilter = string.Empty;
        public string _SelectVersionFilter = string.Empty;
        public string _SelectModLoaderFilter;
        public KeyValuePair<string, int>? _SelectCategoriesFilter = new("全部", -1);
        public List<CurseForgeModel> _ModList = new();
        public CurseForgeModel _SelectMod;
        public double _FilterOpacity = 0;
        public double _Margins = 0;
        public bool _FilterVisible = false;        
        public bool _IsLoaded = true;        
        public bool _IsLoadedOver = false;
        public bool _IsModsLoadOK = false;
        public double _FilterHeight = 0;
        public Thickness _FilterMargin = new(45,0,45,0);
    }
}