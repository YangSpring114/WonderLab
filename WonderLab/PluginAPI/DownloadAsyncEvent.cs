using Natsurainko.Toolkits.Network.Model;
using PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Enum;
using WonderLab.ViewModels;
using WonderLab.Views;

namespace WonderLab.PluginAPI
{
    public enum DownloadType
    {
        Game,
        Http,
        ModLoaders,
        ModLoader
    }
    public abstract class DownloadAsyncEvent : Event, ICancellable
    {
        public abstract DownloadType DownloadType { get; }
        public bool IsCanceled { get; set; }
    }
    public class GameDownloadEvent : DownloadAsyncEvent
    {
        public string id;
        public DownType downType;
        public GameDownloadEvent(string Id, DownType DownType)
        {
            id = Id;
            downType = DownType;
        }
        public override DownloadType DownloadType
        {
            get
            {
                return DownloadType.Game;
            }
        }

        public override string Name { get { return "GameDownloadEvent"; } }

        public override bool Do()
        {
            if (!IsCanceled)
            {
                DownItemView downItemView = new DownItemView(id, downType);
                TaskView.Add(downItemView);
            }
            return true;
        }
    }
    public class HttpDownloadEvent : DownloadAsyncEvent
    {
        public HttpDownloadRequest http;
        public string taskName;
        public AfterDo? afterDo;
        public HttpDownloadEvent(HttpDownloadRequest Http, string TaskName = "", AfterDo? AfterDo = null)
        {
            http = Http;
            taskName = TaskName;
            afterDo = AfterDo;
        }
        public override DownloadType DownloadType
        {
            get
            {
                return DownloadType.Http;
            }
        }

        public override string Name { get { return "HttpDownloadEvent"; } }

        public override bool Do()
        {
            if (!IsCanceled)
            {
                DownItemView downItemView;
                if (afterDo == null)
                {
                    downItemView = new DownItemView(http, taskName);
                }
                else
                {
                    downItemView = new DownItemView(http, taskName, afterDo);
                }
                TaskView.Add(downItemView);
            }
            return true;
        }
    }
    public class ModLoaderDownloadEvent : DownloadAsyncEvent
    {
        public ModLoaderInformationViewData id;
        public ModLoaderDownloadEvent(ModLoaderInformationViewData Id)
        {
            id = Id;
        }
        public override DownloadType DownloadType
        {
            get
            {
                return DownloadType.ModLoader;
            }
        }

        public override string Name { get { return "ModLoadersDownloadEvent"; } }

        public override bool Do()
        {
            if (!IsCanceled)
            {
                DownItemView downItemView = new DownItemView(id);
                TaskView.Add(downItemView);
            }
            return true;
        }
    }
    public class ModLoadersDownloadEvent : DownloadAsyncEvent
    {
        public List<ModLoaderInformationViewData> ids;
        public ModLoadersDownloadEvent(List<ModLoaderInformationViewData> Ids)
        {
            ids = Ids;
        }
        public override DownloadType DownloadType
        {
            get
            {
                return DownloadType.ModLoaders;
            }
        }

        public override string Name { get { return "ModLoadersDownloadEvent"; } }

        public override bool Do()
        {
            if (!IsCanceled)
            {
                DownItemView downItemView = new DownItemView(ids);
                TaskView.Add(downItemView);
            }
            return true;
        }
    }
}
