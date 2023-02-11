using Avalonia.Input.Platform;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 剪贴板操作工具类
    /// </summary>
    public class ClipboardToolkt
    {
        public static async ValueTask CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            await ((IClipboard)AvaloniaLocator.Current.GetRequiredService(typeof(IClipboard))).SetTextAsync(text);
        }

        public static async ValueTask ClearClipboard()
        {
            await ((IClipboard)AvaloniaLocator.Current.GetRequiredService(typeof(IClipboard))).ClearAsync();
        }
    }
}
