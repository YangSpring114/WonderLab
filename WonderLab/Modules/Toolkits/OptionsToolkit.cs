using MinecraftLaunch.Modules.Toolkits;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Const;

namespace WonderLab.Modules.Toolkits
{
    public static class OptionsToolkit
    {
        /// <summary>
        /// 游戏语言切换
        /// </summary>
        /// <returns></returns>
        public static async void GameLangChange(string id,int index = 0)
        {
            string langtype = "en_us";

            if (index is 0)
                langtype = "zh_cn";
            else if (index is 2) langtype = "ja_jp";
            else if (index is 3) langtype = "ko_kr";
            //ja_jp
            string path = PathConst.GetOptions(App.Data.FooterPath, id);

            if (!File.Exists(path))
            {
                await File.WriteAllTextAsync(path, $"lang:{langtype}");
                return;
            }

            var allText = await File.ReadAllTextAsync(path);

            foreach (var i in allText.Split("\r\n").AsParallel())
            {
                if (i.Contains("lang:"))
                {
                    LogToolkit.WriteLine("发现语言节点！");
                    allText = allText.Replace(i, $"lang:{langtype}");
                    LogToolkit.WriteLine(allText);
                    await File.WriteAllTextAsync(path, allText);
                    return;
                }
            }
        }
    }
}
