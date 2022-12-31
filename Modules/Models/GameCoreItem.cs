using MinecraftLaunch.Modules.Models.Install;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Class.Model.Install.Vanilla;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{
    public class GameCoreItem : GameCoreEmtity
    {
        public GameCoreItem(GameCoreEmtity coreManifestItem)
        {
            this.Url = coreManifestItem.Url;
            this.ReleaseTime = coreManifestItem.ReleaseTime;
            this.Time = coreManifestItem.Time;
            this.Id = coreManifestItem.Id;
            this.Type = coreManifestItem.Type;
        }

        [JsonIgnore]
        public GameCore GameCore => new()
        {
            Type = this.Type.ToLower(),
            Id = this.Id,
            Source = this.Id
        };

        public CoreManifestItem GetCoreManifest() => new()        
        {
            Id = this.Id,
            ReleaseTime = this.ReleaseTime.ToString(),
            Time = this.Time.ToString(),
            Type = this.Type,
            Url = this.Url
        };
    }
}
