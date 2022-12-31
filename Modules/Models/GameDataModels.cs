using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{
    /// <summary>
    /// 单一游戏核心数据保存模型
    /// </summary>
    public class GameDataModels
    {
        public string LastLaunchTime { get; set; }
        public DateTime LastLaunchTimeVM { get; set; }
        public bool IsEnableIndependencyCore { get; set; } = false;

        public string Jvm { get; set; } = string.Empty;
        public string GameLang { get; set; } = "zh";
        public bool Isolate { get; set; } = false;
        public bool IsFullWindows { get; set; } = false;
        public int WindowHeight { get; set; } = 480;
        public int WindowWidth { get; set; } = 854;
    }
}
