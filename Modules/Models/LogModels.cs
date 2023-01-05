using Avalonia.Media;
using MinecraftLaunch.Modules.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{
    public class LogModels
    {
        public string LogType => LogLevel.ToString();
        public string Time { get; set; }
        public string Source { get; set; }
        public string Log { get; set; }
        public MinecraftLaunch.Modules.Enum.GameLogType LogLevel { get; set; }
    }
}
