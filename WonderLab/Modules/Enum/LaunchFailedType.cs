using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Enum
{
    public enum LaunchFailedType
    {
        /// <summary>
        /// 资源补全时的异常
        /// </summary>
        CompletionedFailed = 1,
        /// <summary>
        /// 启动失败时的异常
        /// </summary>
        LaunchFailed,
        /// <summary>
        /// 游戏崩溃产生的异常
        /// </summary>
        CrashFailed
    }
}
