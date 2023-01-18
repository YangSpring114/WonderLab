using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.ViewModels;

namespace WonderLab.Modules.Toolkits
{
    public class WebToolkit : HttpClient
    {
        /// <summary>  
        /// 指定Post地址使用Post方式获取全部字符串  
        /// </summary>  
        /// <param name="url">请求后台地址</param>  
        /// <param name="content">Post提交数据内容(utf-8编码的)</param>  
        /// <returns></returns>  
        public string Post(string url, string content)
        {
            HttpWebRequest req;
            string result = "";
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";
                byte[] data = Encoding.UTF8.GetBytes(content);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容  
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                    stream.Close();
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        ///
        /// Get请求
        /// 
        /// 
        /// 字符串
        public string Get(string url, int Timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public LoginModel.YggdrasilModels YggdrasilLogin(string uri, string UserEmail, string Password)
        {
            string str = this.Post(uri + "/authserver/authenticate", "{\"username\":\"" + UserEmail + "\",\"password\":\"" + Password + "\"}");
            Debug.WriteLine(str);
            BlessingSkin.Root root = new BlessingSkin.Root();
            try
            {
                root = JsonConvert.DeserializeObject<BlessingSkin.Root>(str);
            }
            catch (Exception)
            {
                MainWindow.ShowInfoBarAsync("错误", "账户列表拉取失败，可能是您的某项信息填错了！", FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
            }

            if (root == null)
                MainWindow.ShowInfoBarAsync("错误","账户列表拉取失败，可能是您的某项信息填错了！", FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
            if (root.accessToken == null)
            {
                throw new WebException(Regex.Unescape(JsonConvert.DeserializeObject<BlessingSkinError>(str).errorMessage));
            }
            LoginModel.YggdrasilModels skin = new LoginModel.YggdrasilModels
            {
                Token = root.accessToken,
            };
            //格式：皮肤站地址/玩家名.json 
            List<LoginModel.YggdrasilAttributetype> list = new List<LoginModel.YggdrasilAttributetype>();
            List<Skin> listpro = new();
            foreach (BlessingSkin.AvailableProfilesItem item in root.availableProfiles)
            {
                #region 链接转换逻辑
                char[] ch = uri.ToArray();//把文本框你的内容转换为char类型数组
                Array.Reverse(ch, 0, uri.Length);//使用Array类的Reverse方法颠倒数据
                var str2 = new StringBuilder().Append(ch).ToString();//获取指定数组
                string bbb = str2.Remove(0, 14);
                char[] ch1 = bbb.ToArray();//把文本框你的内容转换为char类型数组
                Array.Reverse(ch1, 0, bbb.Length);//使用Array类的Reverse方法颠倒数据
                var str3 = new StringBuilder().Append(ch1).ToString();//获取指定数组
                #endregion

                LoginModel.YggdrasilAttributetype name = new LoginModel.YggdrasilAttributetype
                {
                    Name = item.name,
                    Uuid = item.id
                };
                LoginModel.YggdrasilAttributetype name1 = new LoginModel.YggdrasilAttributetype
                {
                    Name = item.name,
                    Uuid = item.id,
                    Skinuri = str3 + "/avatar/player/" + item.name
                };

                list.Add(name1);

            }
            skin.UserAccount = list.ToArray();
            return skin;
        }

        public async ValueTask<string> VersionCheckAsync(string id = "f08e3a0d2d8f47d6b5aee68ec2499a21")
        {
            var info = await GetStringAsync($"http://2018k.cn/api/checkVersion?id={id}");
            return info.Split("|")[4];
        }
    }

    internal class MinecraftSkinItem
    {
        public class SKIN
        {
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
        }

        public class Textures
        {
            /// <summary>
            /// 
            /// </summary>
            public SKIN SKIN { get; set; }
        }

        public class Root
        {
            public Textures textures { get; set; }
        }
    }
    internal class MinecraftSkin
    {
        public class PropertiesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public List<PropertiesItem> properties { get; set; }
        }
    }
    public class LoginModel
    {
        public class YggdrasilModels//skin
        {
            /// <summary>
            /// Token
            /// </summary>
            public string Token { get; internal set; }
            /// <summary>
            /// 用户在皮肤站注册的所有游戏账号
            /// </summary>
            public YggdrasilAttributetype[] UserAccount { get; internal set; }
        }
        /// <summary>
        /// 第三方账号属性类型
        /// </summary>
        public class YggdrasilAttributetype//skinname
        {
            /// <summary>
            /// 游戏名
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// 用户的uuid
            /// </summary>
            public string Uuid { get; internal set; }
            /// <summary>
            /// 用户的一些属性
            /// </summary>
            public string Skinuri { get; internal set; }

        }
    }
    internal class BlessingSkinError
    {
        /// <summary>
        /// 
        /// </summary>
        public string error { get; set; }
        /// <summary>
        /// 输入的邮箱与密码不匹配
        /// </summary>
        public string errorMessage { get; set; }
    }
    internal class BlessingSkin
    {
        public class AvailableProfilesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string accessToken { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string clientToken { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<AvailableProfilesItem> availableProfiles { get; set; }
        }
    }
    public class Skin
    {
        public string username { get; set; }
        public SkinPro skins { get; set; }
        public string cape { get; set; }
    }
    public class SkinPro
    {
        public string slim { get; set; }
    }
    public class SKIN
    {
        public string url { get; set; }
    }
    public class Textures
    {
        public SKIN SKIN { get; set; }
    }
    public class Root
    {
        public Textures textures { get; set; }
    }

}
