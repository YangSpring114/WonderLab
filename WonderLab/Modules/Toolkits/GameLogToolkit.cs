using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.Modules.Models;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 游戏日志操作工具类
    /// </summary>
    public class GameLogToolkit
    {
        public static GameLogType GameLogParSing(string S)
        {
            var regex = new Regex($"FATAL|ERROR|WARN|INFO|DEBUG");
            var regex1 = new Regex($": .*?\\s\\S.*");
            var regex2 = new Regex(@"\[*(?<time>[\s\S]*?)\]");

            return new GameLogType
            {
                Log = regex1.Match(S).Value.Replace(": ", ""),
                LogTime = regex2.Match(S).Groups["time"].Value,
                LogType = regex.Match(S).Value switch
                {
                    "FATAL" => LogType.Fatal,
                    "ERROR" => LogType.Error,
                    "WARN" => LogType.Warning,
                    "INFO" => LogType.Info,
                    "DEBUG" => LogType.Debug,
                    _ => LogType.Unknown
                },
            };

        }
    }
}
