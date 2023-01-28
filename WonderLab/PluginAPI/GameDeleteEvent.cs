using MinecraftLaunch.Modules.Models.Launch;
using MinecraftLaunch.Modules.Toolkits;
using PluginLoader;
using System;
using WonderLab.Views;

namespace WonderLab.PluginAPI
{
    public class GameDeleteEvent : Event, ICancellable
    {
        public GameCore Core;
        public GameDeleteEvent(GameCore core)
        {
            Core = core;
        }
        public bool IsCanceled { get; set; }

        public override string Name => throw new NotImplementedException();

        public override bool Do()
        {
            try
            {
                GameCoreToolkit gameCoreLocator = new(Core.Root);
                gameCoreLocator.Delete(Core.Id);
                GameView.ViewModel.GameSearchAsync();
                MainView.mv.FrameView.Navigate(typeof(GameView));
                return true;
            }
            catch { }
            return false;
        }
    }
}
