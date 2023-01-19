using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GithubLib
{
    public class GithubLib
    {
        /// <summary>
        /// 获取release的url
        /// </summary>
        /// <param name="author">作者</param>
        /// <param name="name">存储库名字</param>
        /// <returns>最新release的url</returns>
        public static string GetRepoLatestReleaseUrl(string author, string name)
        {
            string result = "https://api.github.com/repos/" + author + "/" + name + "/releases/latest";
            return result;
        }
        public static Release? GetRepoLatestRelease(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("User-Agent", "JWJUN233233-GithubLib");
            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)wr;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                Release? release = Newtonsoft.Json.JsonConvert.DeserializeObject<Release>(content);
                return release;

            }
        }
        //static void Main()
        //{
        //    string url = GetRepoLatestReleaseUrl("Blessing-Studio", "WonderLab");
        //    Release? release = GetRepoLatestRelease(url);
        //    if(release != null)
        //    {
        //        Console.WriteLine(release.assets[0].name);
        //    }
        //}
    }
    public class User
    {
        public string? login;
        public int? id;
        public string? node_id;
        public string? avatar_url;
        public string? gravatar_id;
        public string? url;
        public string? followers_url;
        public string? following_url;
        public string? gists_url;
        public string? starred_url;
        public string? subscriptions_url;
        public string? organizations_url;
        public string? repos_url;
        public string? events_url;
        public string? received_events_url;
        public string? type;
        public bool? site_admin;

    }
    public class Release
    {
        public string? url;
        public string? assets_url;
        public string? upload_url;
        public string? html_url;
        public int id;
        public User? author;
        public string? node_id;
        public string? tag_name;
        public string? target_commitish;
        public string? name;
        public bool draft;
        public bool prerelease;
        public string? created_at;
        public string? published_at;
        public List<Asset>? assets;
        public string? tarball_url;
        public string? zipball_url;
        public string? body;
    }
    public class Asset
    {
        public string? url;
        public int id;
        public string? node_id;
        public string? name;
        public string? label;
        public User? uploader;
        public string? content_type;
        public string? state;
        public long size;
        public int download_count;
        public string? created_at;
        public string? updated_at;
        public string? browser_download_url;

    }
}
