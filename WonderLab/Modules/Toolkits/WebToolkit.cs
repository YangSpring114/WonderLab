using Natsurainko.Toolkits.Network;
using Natsurainko.Toolkits.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.ViewModels;

namespace WonderLab.Modules.Toolkits
{
    public class WebToolkit
    {
        /// <summary>
        /// 版本更新检查方法
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<KeyValuePair<bool,UpdateInfo>> VersionCheckAsync()
        {
            var res = await HttpWrapper.HttpGetAsync("https://api.github.com/repos/Blessing-Studio/WonderLab/releases/latest");
            LogToolkit.WriteLine("检查更新步骤1 -获取Json -OK");
            var model = (await res.Content.ReadAsStringAsync()).ToJsonEntity<UpdateInfo>();
            LogToolkit.WriteLine($"Json里的版本为 {model.Version} 启动器实际版本为 {model.Version}");
            if (MainWindow.GetVersion().Replace("Build ", string.Empty) == model.Version)
                return new(false,model);

            return new(true, model);
        }
    }

    public class UpdateAsset
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string Url { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }

    public class UpdateInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tag_name")]
        public string Version { get; set; }

        [JsonPropertyName("prerelease")]
        public bool PreRelease { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime ReleaseTime { get; set; }

        [JsonPropertyName("body")]
        public string Description { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("assets")]
        public List<UpdateAsset> Assets { get; set; }
    }

    public class UpdateChangelog
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("details")]
        public string[] Details { get; set; }
    }

    public class Author
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("followers_url")]
        public string FollowersUrl { get; set; }

        [JsonProperty("following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty("gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty("starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty("subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [JsonProperty("organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public string SiteAdmin { get; set; }
    }
}
