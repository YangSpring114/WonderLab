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
using WonderLab.Modules.Const;

namespace WonderLab.Modules.Toolkits
{
    public class WebToolkit
    {
        const string API = $"http://api.2018k.cn/checkVersion?id=<id>&version={InfoConst.LauncherVersion}";

        /// <summary>
        /// 版本更新检查方法
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<KeyValuePair<bool, string>> VersionCheckAsync()
        {
            try
            {
                int secondTolast = 0;
                int lastVersion = 0;
                string url = string.Empty;

                if (App.Data.CurrentBranch is 0) {
                    var res = await HttpWrapper.HttpGetAsync(API.Replace("<id>", "f08e3a0d2d8f47d6b5aee68ec2499a21"));
                    var content = await res.Content.ReadAsStringAsync();
                    lastVersion = Convert.ToInt32(content.Split('|').Last().Split('.').Last());
                    secondTolast= Convert.ToInt32(content.Split("|").Last().Split('.')[2]);
                    url = content.Split('|')[3].Trim();
                } else {                    
                    var res = await HttpWrapper.HttpGetAsync(API.Replace("<id>", "d743cd5cbfaf46dea31e0730c5af0e85"));
                    var content = await res.Content.ReadAsStringAsync();
                    secondTolast = Convert.ToInt32(content.Split("|").Last().Split('.')[2]);
                    lastVersion = Convert.ToInt32(content.Split('|').Last().Split('.').Last());
                    url = content.Split('|')[3].Trim();
                }
                
                Trace.WriteLine($"[信息] 发行分支为 {App.Data.CurrentBranch}");
                Trace.WriteLine($"[信息] 倒数第二个版本为 {secondTolast}");
                Trace.WriteLine($"[信息] 尾版本为 {lastVersion}");
                
                if (Convert.ToInt32(InfoConst.LauncherVersion.Split('.').Last()) < lastVersion ||
                    Convert.ToInt32(InfoConst.LauncherVersion.Split('.')[2]) < secondTolast) {
                    return new(true, url);
                } else {
                    return new(false, string.Empty);
                }
            }
            catch (Exception)
            {
                return new(false, string.Empty);
            }
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
        }
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

    public class Author
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

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
    }

    public class UpdateAsset
    {
        [JsonProperty("browser_download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class UpdateInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("target_commitish")]
        public string TargetCommitish { get; set; }

        [JsonProperty("prerelease")]
        public string PreRelease { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("assets")]
        public List<UpdateAsset> Assets { get; set; }
    }
}
