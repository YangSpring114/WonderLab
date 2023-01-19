using Avalonia;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using GithubLib;
using System;
using System.Text;

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
            string releaseUrl = GithubLib.GithubLib.GetRepoLatestReleaseUrl("Blessing-Studio", "WonderLab");
            Release? release = null;
            try
            {
                 release = GithubLib.GithubLib.GetRepoLatestRelease(releaseUrl);
            }
            catch (Exception _)
            {

            }
            if(release != null)
            {

            }
            PluginLoader.PluginLoader.LoadAllFromPlugin();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace(LogEventLevel.Error, LogArea.Property, LogArea.Layout)
                .With(new Win32PlatformOptions
                {
                     UseWindowsUIComposition = true,
                })
                .UseReactiveUI();
    }
}
