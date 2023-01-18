using Avalonia.Controls;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;

namespace WonderLab.Modules.Base
{
    /// <summary>
    /// 异步操作基类
    /// </summary>
    public class TaskBase : Page
    {
        /// <summary>
        /// 将代码调到非ui线程
        /// </summary>
        /// <param name="action"></param>
        public static async void RunAsync(Action action)=>
            await Task.Run(action);
        /// <summary>
        /// 将代码调到非ui线程（可访问ui线程）
        /// </summary>
        /// <param name="action"></param>
        public static async void InvokeAsync(Action action) =>
            await Task.Run(async () => await Dispatcher.UIThread.InvokeAsync(action));

        //public static async Task<T> InvokeAsync<T>(Func<T> action)
        //{
        //    return await Task.Run<T>(async () => { return await Dispatcher.UIThread.InvokeAsync(action); });
        //}
    }
}
