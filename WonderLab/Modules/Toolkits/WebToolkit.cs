using Natsurainko.Toolkits.Network;
using Natsurainko.Toolkits.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
            if ("".Replace("Build ", string.Empty) == model.Version)
                return new(false,model);

            return new(true, model);
        }

        /// <summary>
        /// 获取正版皮肤 Url
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static async ValueTask<string> GetUserSkinUrl(string uuid)
        {
            var res = await HttpWrapper.HttpGetAsync($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");
            var json = await res.Content.ReadAsStringAsync();
            Trace.WriteLine($"[信息] 返回的 Json 信息如下：{json}");

            var skinjson = Encoding.UTF8.GetString(Convert.FromBase64String(json.ToJsonEntity<UserSkinInfo>().Properties.First().Value));
            Trace.WriteLine($"[信息] 皮肤 Base64 解码的 Json 信息如下：{skinjson}");

            var url = skinjson.ToJsonEntity<SkinMoreInfo>().Textures.Skin.Url;
            Trace.WriteLine($"[信息] 皮肤的链接如下：{url}");
            return url;
            //JObject jobject = new(json);
            //var base64 = (jobject["properties"] as JArray)[0]["value"];
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

    public class SkinInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class UserSkinInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("properties")]
        public List<SkinInfo> Properties { get; set; }
    }

    public class SKIN
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class CAPE
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Textures
    {
        [JsonProperty("SKIN")]
        public SKIN Skin { get; set; }

        [JsonProperty("CAPE")]
        public CAPE Cape { get; set; }
    }

    public class SkinMoreInfo
    {
        [JsonProperty("timestamp")]
        public Int64 TimeStamp { get; set; }

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("profileName")]
        public string ProfileName { get; set; }

        [JsonProperty("textures")]
        public Textures Textures { get; set; }
    }
}
