using MinecraftLaunch.Modules.Models.Launch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.ViewModels;

namespace WonderLab.Modules.Models
{
    public class GameCoreViewData<T> : ViewDataBase<T>
    {
        public GameCoreViewData(T data) : base(data)
        {
        }

        public DateTime LastLaunchTime { get; set; }
    }
}
