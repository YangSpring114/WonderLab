using PluginLoader;
using WonderLab.PluginAPI;

namespace CustomValue
{
    [PluginHandler("CustomValue", "这是个测试插件", "1.0.0.12", "{EDE94F4B-3E3F-4380-A0A7-CBFEF1781B16}", "Ddggdd135", Icon)]
    public class CustomValuePlugin : Plugin
    {
        public const string Icon = "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAPqSURBVHhe7ZvHilRBGIXHgDliziimhYIB0b1rX8qVr6I+ghsRRBHRMecsiOJCMSui5yu7oBmmb/1V9VfPwvvBYbpm0TP33HMq3J6Z6Onp6enp6enp6fk/mTX4muKEdFhaEEbdXJXOSR/DyI9j0nFpaRh180I6K70PIwdOSrzZH4POSPukOZIXG6VTkvV3OC1tlZLMHnxN8Ub6+e9lkrUSP3x+GPmwS8LUhWHUzQfprvQtjBJYDXgg3ZO+h1E3eyTqsiiMfNgs7ZAspr6Snks/wiiB1YCn0jXpSxh1s17ibq2TvGqwTFouWd7vvnRLck3AO+mlZK0BF79N8qgB/be+V4w/KfjFN1JYDfgq8cYzUYOc/t+Rbkj8viasBsATaSZqYO0/d/+yhAGWmxTIMSC3Bosla29HQfz3S6wsqfch9nSfpfI337CQY0BuDZZIGDA3jMrIiT+T323JNPlFcgyAnBp4zAPEf7tkiT83h4SaJr9IrgE5NWAeIL6l8wDxt84jWWv/MLkGUAMqYK1Bza6Q+GOgNf7mtX+YXAPgsTSOGjSPP5QYkFODDVJpDTj1rZCaxR9KDBhHDTDOuvvjnFIUfygxAKjBdcmy46IGh6ScGlj7XxV/KDUg1sASu5IaWPtP/J9JRfGHUgNyN0VcvLUGGMbyZ9n9VcUfSg2AnBqwLWZSs/y8GP9UZarjDzUG5NSASHNhlmeK1sPPa6kq/lBjQE4NrPPAWOMPNQYANZiULDXg4rdI88JoesYaf6g1IKcGcTlkPhjFJikn/taj+UhqDYg1YC9eWwMmSRKySrLG35K8TmoNgJzVoKsGnB5ZKrsqAjH+fPhRFX/wMCCnBnulUTUgIZbtr1v8wcMAToUeNbD23y3+4GEA1K4G0ZjU8kf8WXZd4g9eBuQckeOucPhCd0qW5Y/48yGNS/zBywBqwDN5y6aIuz91V2g9/LjGH7wMAGsNpsadseXZn3v8wdOAnBpwsXHJs8b/inRe+hRGTngaQA34JS8NXncxvCu0HH64+7z3Tcly/DbjaQDwsdQFKXWXhj/x4bnfSqkr/q5r/zDeBuRMhtTgqMSEmNr9PZS4+26TX8TbALBOhnT+oHRg8HoUbie/6WhhgHUy5I8emAhThx+XBx+jaGFA3BqnasDkRwKYD7o+QG0Wf2hhADySUjVgN0gCVkujEtA0/tDKgJw9QRdN4w+tDIg1sJwQu2gaf2hlAFhq0EXc+jaLP7Q0oLYG8eTXLP7Q0oDaGjSPP7Q0AEprwB9aN48/tDbgrVRSg+azf6S1AaU1GEv8wfr/AjWskY5Iu6XUE58ItbkofQ6jnp6eniZMTPwFOhtISb2EIh4AAAAASUVORK5CYII=";
        public ConfigManager Config;
        public static CustomValuePlugin Plugin;
        public void OnPluginLoad()
        {
            Config = new ConfigManager(this);
            try
            {
                if (Config.GetBool("Value"))
                {

                }
            }
            catch
            {
                Config.SetBool("Value", false);
            }
            Plugin = this;
        }
        public void OnPluginUnLoad()
        {
            Config.SaveConfig();
        }
    }
    public class GetValueEvent : Event
    {
        public override string Name
        {
            get
            {
                return "GetValueEvent";
            }
        }

        public override bool Do()
        {
            return CustomValuePlugin.Plugin.Config.GetBool("Value");
        }
    }
}