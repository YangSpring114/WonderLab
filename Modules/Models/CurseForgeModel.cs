using Avalonia.Media.Imaging;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Models.Download;
using Natsurainko.Toolkits.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;

namespace WonderLab.Modules.Models
{
    public class CurseForgeModel : ViewModelBase
    {
        public CurseForgeModel(CurseForgeModpack curseForgeModpack)
        {
            this.Categories = curseForgeModpack.Categories;
            this.Name = curseForgeModpack.Name;
            this.Id = curseForgeModpack.Id;
            this.Description = curseForgeModpack.Description;
            this.Links = curseForgeModpack.Links;
            this.DownloadCount = curseForgeModpack.DownloadCount;
            this.LastUpdateTime = curseForgeModpack.LastUpdateTime;
            this.GamePopularityRank = curseForgeModpack.GamePopularityRank;
            this.LatestFilesIndexes = curseForgeModpack.LatestFilesIndexes;
            this.IconUrl = curseForgeModpack.IconUrl;
            this.Files = curseForgeModpack.Files;
            this.SupportedVersions = curseForgeModpack.SupportedVersions;
            //_ = DownloadImageAsync();
        }

        public CurseForgeModel Current => this;

        public int Id { get; set; }

        public string Name { get => _Name; set => RaiseAndSetIfChanged(ref _Name, value); }

        public string Description { get; set; }

        public Dictionary<string, string> Links { get; set; }

        public int DownloadCount { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int GamePopularityRank { get; set; }

        public List<CurseForgeModpackFileInfo> LatestFilesIndexes { get; set; }

        public string IconUrl { get; set; }

        public Dictionary<string, List<CurseForgeModpackFileInfo>> Files { get; set; } = new();

        public IEnumerable<CurseForgeModpackFileInfo> FileInfos 
        {
            get
            {
                foreach (var item in Files)
                    foreach (var i in item.Value)
                        yield return i;
            }
        }

        public List<CurseForgeModpackCategory> Categories { get; set; }

        public string[] SupportedVersions { get; set; }

        public Bitmap Icon
        {
            get => _Icon;
            set => RaiseAndSetIfChanged(ref _Icon, value);
        }

        public bool Load
        {
            get => _Load;
            set => RaiseAndSetIfChanged(ref _Load, value);
        }

        public bool DownloadButtonEnable
        {
            get => _IsButtonEnble;
            set => RaiseAndSetIfChanged(ref _IsButtonEnble, value);
        }

        public Bitmap _Icon = default;

        public bool _Load = true;

        public string _Name = string.Empty;

        public string _ChineseName = string.Empty;

        public string CurrentVersion
        {
            get => _CurrentVersion;
            set
            {
                if (RaiseAndSetIfChanged(ref _CurrentVersion, value))
                {
                    CurrentFileInfos = Files[CurrentVersion];
                    CurrentFileInfo = null;
                }
            }
        }

        public string _CurrentVersion = string.Empty;

        public bool _IsButtonEnble = false;

        public List<CurseForgeModpackFileInfo> CurrentFileInfos { get => _CurrentFileInfos; set { RaiseAndSetIfChanged(ref _CurrentFileInfos, value);  } }

        public List<CurseForgeModpackFileInfo> _CurrentFileInfos;

        public CurseForgeModpackFileInfo CurrentFileInfo
        {
            get => _CurrentFileInfo;
            set 
            {
                if (RaiseAndSetIfChanged(ref _CurrentFileInfo,value))
                {
                    DownloadButtonEnable = value is not null ? true : false;
                }
            }
        }

        public CurseForgeModpackFileInfo _CurrentFileInfo;

        public string ChineseName
        {
            get
            {
                if (string.IsNullOrEmpty(_ChineseName))
                {
                    _ChineseName = Name;
                    if (Links["websiteUrl"] != null)
                    {
                        string Keyword = Links["websiteUrl"].TrimEnd('/').Split("/").Last();
                        if (InfoConst.ModLangDatas.ContainsKey(Keyword))
                        {
                            var Result = InfoConst.ModLangDatas[Keyword];
                            if (!string.IsNullOrEmpty(Result.Chinese))
                                _ChineseName = Result.Chinese;
                        }
                    }
                }
                return _ChineseName;
            }
            set=> _ChineseName = value;
        }

        public bool IsLoadOK
        {
            set
            {
                if (value)
                {
                    _ = DownloadImageAsync();
                }
            }
        }

        public async ValueTask DownloadImageAsync()
        {
            Load = true;
            var stream = await new HttpClient().GetByteArrayAsync(IconUrl);
            using var ms = new MemoryStream(stream);
            Icon = new Bitmap(ms);
            Load = false;
            //await Task.Run(async delegate
            //{

            //}, default);
        }
    }
}
