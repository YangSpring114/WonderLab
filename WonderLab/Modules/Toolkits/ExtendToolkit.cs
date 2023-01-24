using MinecraftLaunch.Modules.Models.Auth;
using MinecraftLaunch.Modules.Models.Install;
using MinecraftLaunch.Modules.Models.Launch;
using Natsurainko.FluentCore.Class.Model.Launch;
using Natsurainko.FluentCore.Module.Launcher;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Models;
using GameCore = MinecraftLaunch.Modules.Models.Launch.GameCore;

namespace WonderLab.Modules.Toolkits
{
    public static class ExtendToolkit
    {
        public static UserModels ToUserModel(this UserDataModels user) => new(user);

        public static Dictionary<string, DateTime?> GetGameCoresLastLaunchTime(this IEnumerable<GameCore> cores)
        {
            var gameCoresLastLaunchTime = new Dictionary<string, DateTime?>();
            foreach (var i in cores)
            {
                var res = JsonToolkit.GetTimeInfoJsons(App.Data.FooterPath, new GameCoreLocator(App.Data.FooterPath).GetGameCore(i.Id));
                gameCoresLastLaunchTime.Add(res.Item1 is null ? string.Empty : res.Item1, res.Item2);
            }
            return gameCoresLastLaunchTime;
        }

        public static string ToVersionType(this string raw)
        {
            string type = string.Empty;
            if (raw is "release")
                type = "正式版";
            else if (raw is "snapshot")
                type = "快照版";
            else if (raw.Contains("old_alpha"))
                type = "远古版";
            return type;
        }

        public static string ToType(this GameCore core)
        {
            string type = string.Empty;
            if (core.Type is "release")
                type = "正式版";
            else if (core.Type is "snapshot")
                type = "快照版";
            else if (core.Type.Contains("old_alpha"))
                type = "远古版";
            Debug.WriteLine("Type:"+type);
            var res = core.HasModLoader ? $"{type} 继承自 {core.Source}" : $"{type} {core.Source}";
            Debug.WriteLine(res);
            return res;
        }

        public static Natsurainko.FluentCore.Class.Model.Launch.GameCore ToNatsurainkoGameCore(this GameCore core) =>
            new GameCoreLocator(App.Data.FooterPath).GetGameCore(core.Id);

        public static GameCore GetGameCoreInIndex(this List<GameCore> cores ,string id) {        
            return cores.Where(x => x.Id == id).FirstOrDefault()!;
        }

        public static UserModels GetUserInIndex(this List<UserModels> users, string id)
        {
            foreach (var i in users)
                if (i.Name == id)
                    return i;

            return null;
        }

        public static string GetVersionType(string raw)
        {
            string type = string.Empty;
            if (raw is "release")
                type = "正式版";
            else if (raw is "snapshot")
                type = "快照版";
            else if (raw.Contains("old_alpha"))
                type = "远古版";
            return type;
        }

        public static Account ToAccount(this UserDataModels raw)
        {
            if (raw.UserType.Contains("微软")) {            
                return new MicrosoftAccount()
                {
                    Name = raw.UserName,
                    AccessToken = raw.UserAccessToken,
                    Uuid = Guid.Parse(raw.UserUuid),
                    RefreshToken = raw.UserRefreshToken,
                };
            }
            else if (raw.UserType.Contains("离线")) {
                return new OfflineAccount()
                {
                    Name = raw.UserName,
                    AccessToken = raw.UserAccessToken,
                    Uuid = Guid.Parse(raw.UserUuid),
                };
            } else {
                return new YggdrasilAccount()
                {
                    Name = raw.UserName,
                    AccessToken = raw.UserAccessToken,
                    Uuid = Guid.Parse(raw.UserUuid),
                };
            }
        }

        public static LogModels ToOutput(this GameLogAnalyseResponse response)
        {
            return new LogModels()
            {
                Log = response.Log,
                LogLevel = response.LogType,
                Source = response.Source,
                Time = response.Time,
            };
        }
    }
}
