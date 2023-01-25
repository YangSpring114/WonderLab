using Avalonia;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using GithubLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using WonderLab.Modules.Toolkits;

namespace WonderLab
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                AppBuilder builder = BuildAvaloniaApp();
                builder.StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                JsonToolkit.JsonWrite();
                Trace.WriteLine(ex.ToString());
                MainWindow.ShowInfoBarAsync("错误：",$"WonderLab 在使用中遇到了不可描述的异常,这可能会影响您的使用体验！\n异常堆栈： {ex}");
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),$"崩溃日志-{ex.GetType().Name}.txt"), ex.ToString());
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PluginLoader.PluginLoader.LoadAllFromPlugin();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new Win32PlatformOptions
                {
                    UseWindowsUIComposition = true,
                })
                .UseReactiveUI();
    }
}
