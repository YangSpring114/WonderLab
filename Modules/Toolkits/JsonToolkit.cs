using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using MinecraftLaunch.Modules.Toolkits;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.Toolkits.Text;
using Newtonsoft.Json;
using WonderLab.Modules.Const;
using WonderLab.Modules.Models;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// json操作工具类
    /// </summary>
    public class JsonToolkit
    {
        /// <summary>
        /// 写入json(数据保存用)
        /// </summary>
        /// <returns>如果返回值为 true 则为写入，如果为 false 则自动读取</returns>
        public static bool JsonAllWrite()
        {
            if (!File.Exists(PathConst.SettingJsonPtah))
            {
                //w
                if (!Directory.Exists(PathConst.MainDirectory))
                    Directory.CreateDirectory(PathConst.MainDirectory);

                if (!Directory.Exists(PathConst.SkinHeadDirectory))
                    Directory.CreateDirectory(PathConst.SkinHeadDirectory);

                File.WriteAllText(PathConst.SettingJsonPtah, Newtonsoft.Json.JsonConvert.SerializeObject(new DataModels()));
                return true;
            }
            else
            {
                //r
                var Temp = JsonConvert.DeserializeObject<DataModels>(File.ReadAllText(PathConst.SettingJsonPtah));
                App.Data = Temp;
                return false;
            }
        }

        public static void JsonWrite()
        {
            //App.Data.UserList = 
            File.WriteAllText(PathConst.SettingJsonPtah, Newtonsoft.Json.JsonConvert.SerializeObject(App.Data));
        }

        public static void CreaftGameInfoJson(string root, GameCore core)
        {
            var v = PathConst.GetVersionFolder(root, core.Id) + $"\\info.json";
            var data = new GameDataModels()
            {
                LastLaunchTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                LastLaunchTimeVM = DateTime.Now
            };
            File.WriteAllText(v, data.ToJson());
        }

        public static void ChangeEnableIndependencyCoreInfoJsonTime(string root,GameCore core, GameDataModels gameData = null)
        {
            FileInfo info = new(Path.Combine(PathConst.GetVersionFolder(root, core.Id), "info.json"));
            if (info.Exists)
            {
                gameData.LastLaunchTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                gameData.LastLaunchTimeVM = DateTime.Now;
                File.WriteAllText(info.FullName,gameData.ToJson());
                return;
            }
            CreaftGameInfoJson(root, core);
        }

        public static GameDataModels CreaftEnableIndependencyCoreInfoJson(string root, GameCore core,bool IsEnableIndependencycore = true)
        {
            var v = PathConst.GetVersionFolder(root, core.Id) + $"\\info.json";
            var data = new GameDataModels()
            {
                IsEnableIndependencyCore = IsEnableIndependencycore
            };
            File.WriteAllText(v, data.ToJson());
            return data;
        }

        public static void WriteEnableIndependencyCoreInfoJson(string root, GameCore core, GameDataModels models)
        {
            var v = PathConst.GetVersionFolder(root, core.Id) + $"\\info.json";
            File.WriteAllText(v, models.ToJson());
        }

        public static string GetTimeInfoJson(string root, GameCore core)
        {
            var jsonpath = PathConst.GetVersionFolder(root, core.Id) + $"\\info.json";
            if (File.Exists(jsonpath))
            {
                var json = File.ReadAllText(jsonpath);
                var v = Newtonsoft.Json.JsonConvert.DeserializeObject<GameDataModels>(json);
                return v.LastLaunchTime;
            }
            return "";
        }

        public static GameDataModels GetEnableIndependencyCoreData(string root, GameCore core)
        {
            var jsonpath = PathConst.GetVersionFolder(root, core.Id) + $"\\info.json";
            if (File.Exists(jsonpath))
            {
                var json = File.ReadAllText(jsonpath);
                var v = Newtonsoft.Json.JsonConvert.DeserializeObject<GameDataModels>(json);
                return v;
            }
            return null;
        }

        public static (string,DateTime) GetTimeInfoJsons(string root, GameCore core)
        {
            var jsonpath = PathConst.GetVersionFolder(root, core.Id) + $"\\info.json";
            if (File.Exists(jsonpath))
            {
                var json = File.ReadAllText(jsonpath);
                var v = Newtonsoft.Json.JsonConvert.DeserializeObject<GameDataModels>(json);
                (string, DateTime) t = new(v.LastLaunchTime, v.LastLaunchTimeVM);
                return t;
            }
            return new();
        }

        public static string JsonConverts<T>(T data)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var i in data.ToString().ToArray())
            {
                Debug.WriteLine(i);
                stringBuilder.Append(i);
            }
            Debug.WriteLine(stringBuilder);
            return stringBuilder.ToString();
        }
    } 
}
