using System.Net;
using System.Text;

namespace GithubLib
{
    public class GithubLib
    {
        /// <summary>
        /// 获取release的url
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="name">存储库名字</param>
        /// <returns>最新release的url</returns>
        public static string GetRepoLatestReleaseUrl(string owner, string name)
        {
            string result = "https://api.github.com/repos/" + owner + "/" + name + "/releases/latest";
            return result;
        }
        /// <summary>
        /// 从url获取release
        /// </summary>
        /// <param name="url">链接</param>
        /// <returns>url的release</returns>
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
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static User? GetUser(string userName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/users/" + userName);
            request.Method = "GET";
            request.Headers.Add("User-Agent", "JWJUN233233-GithubLib");
            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)wr;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                User? user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(content);
                return user;
            }
        }/// <summary>
        /// 获取用户的关注
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>关注者</returns>
        public static List<User>? GetFollowing(string userName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/users/" + userName + "/following");
            request.Method = "GET";
            request.Headers.Add("User-Agent", "JWJUN233233-GithubLib");
            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)wr;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                List<User>? user = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(content);
                return user;
            }
        }
        /// <summary>
        /// 获取仓库贡献人员名单
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="name">存储库名字</param>
        /// <returns>贡献人员名单</returns>
        public static List<User>? GetContributors(string owner, string name)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/" + owner + "/" + name + "/contributors");
            request.Method = "GET";
            request.Headers.Add("User-Agent", "JWJUN233233-GithubLib");
            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)wr;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                List<User>? user = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(content);
                return user;
            }
        }
        /// <summary>
        /// 获取仓库的issue
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="name">存储库名字</param>
        /// <returns>仓库的issue</returns>
        public static Issue[]? GetIssues(string owner, string name)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/" + owner + "/" + name + "/issues");
            request.Method = "GET";
            request.Headers.Add("User-Agent", "JWJUN233233-GithubLib");
            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)wr;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                Issue[] issues = Newtonsoft.Json.JsonConvert.DeserializeObject<Issue[]>(content);
                return issues;
            }
        }
    }
    public class User
    {
        public string? login;
        public long? id;
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
        public long? id;
        public User? author;
        public string? node_id;
        public string? tag_name;
        public string? target_commitish;
        public string? name;
        public bool? draft;
        public bool? prerelease;
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
        public long? id;
        public string? node_id;
        public string? name;
        public string? label;
        public User? uploader;
        public string? content_type;
        public string? state;
        public long? size;
        public int? download_count;
        public string? created_at;
        public string? updated_at;
        public string? browser_download_url;

    }
    public class Issue
    {
        public string? url;
        public string? repository_url;
        public string? labels_url;
        public string? comments_url;
        public string? events_url;
        public string? html_url;
        public long? id;
        public string? node_id;
        public int? number;
        public string? title;
        public User? user;
        public IssueLabel[]? labels;
        public string? state;
        public bool? locked;
        public User[]? assignee;
        public string? milestone;
        public int? comments;
        public string? created_at;
        public string? updated_at;
        public string? closed_at;
        public string? author_association;
        public string? active_lock_reason;
        public string? body;
        public Reactions? reactions;
        public string? timeline_url;
        public int? performed_via_github_app;
        public string? state_reason;

    }
    public class IssueLabel
    {
        public long? id;
        public string? node_id;
        public string? url;
        public string? name;
        public string? color;
        public string? description;
        public bool? @default;
    }
    public class Reactions
    {
        public string? url;
        public int? total_count;
        public int? laugh;
        public int? hooray;
        public int? confused;
        public int? heart;
        public int? rocket;
        public int? eyes;

    }
}
