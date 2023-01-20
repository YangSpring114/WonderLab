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
            var model = JsonSerializer.Deserialize<UpdateInfo>(await res.Content.ReadAsStringAsync());
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
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }

        [JsonPropertyName("followers_url")]
        public string FollowersUrl { get; set; }

        [JsonPropertyName("following_url")]
        public string FollowingUrl { get; set; }

        [JsonPropertyName("gists_url")]
        public string GistsUrl { get; set; }

        [JsonPropertyName("starred_url")]
        public string StarredUrl { get; set; }

        [JsonPropertyName("subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [JsonPropertyName("organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonPropertyName("repos_url")]
        public string ReposUrl { get; set; }

        [JsonPropertyName("events_url")]
        public string EventsUrl { get; set; }

        [JsonPropertyName("received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("site_admin")]
        public string SiteAdmin { get; set; }
    }
}
