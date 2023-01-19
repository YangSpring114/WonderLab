using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Toolkits;
using GithubLib;
using System.Net;
using System.IO;
using System.IO.Compression;
using Downloader;
using WonderLab.Views;
using Natsurainko.Toolkits.Network.Model;
using FluentAvalonia.UI.Controls;

namespace WonderLab.ViewModels
{
    public partial class OtherViewModel : ViewModelBase
    {
        public static Release? Updata()
        {
            string releaseUrl = GithubLib.GithubLib.GetRepoLatestReleaseUrl("Blessing-Studio", "WonderLab");
            Release? release = GithubLib.GithubLib.GetRepoLatestRelease(releaseUrl);
            return release;
        }
        public async Task Check()
        {
            var res = Updata();
            if (ButtonContent == "检查更新")
            {
                MainWindow.ShowInfoBarAsync("提示：", "开始检查更新");
                if (res != null)
                {
                    if (res.name != MainWindow.GetVersion())
                    {
                        MainWindow.ShowInfoBarAsync("提示：", $"有可用更新，版本号  {res.name}", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
                        ButtonContent = "立即更新";
                        return;
                    }
                    else
                    {
                        MainWindow.ShowInfoBarAsync("提示：", "检查更新完成，无可用更新！", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);
                        return;
                    }
                }
                else
                {
                    MainWindow.ShowInfoBarAsync("错误：", "检查更新出错", FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
                }
            }
            else
            {
                var button = new HyperlinkButton()
                {
                    Content = "转至 祝福终端>任务中心",
                };
                MainWindow.ShowInfoBarAsync("提示：", $"开始下载更新  更新内容:\n {res.body} \n\n推送者{res.author.login} \n 可前往任务中心查看进度", InfoBarSeverity.Informational, 8000, button);
                string save = @"updata.zip";
                File.Delete(Path.Combine(save, "updata-cache"));
                string url = null;
                foreach(var asset in res.assets)
                {
                    if(asset.name == "Results.zip")
                    {
                        url = asset.browser_download_url;
                    }
                }
                HttpDownloadRequest httpDownload = new HttpDownloadRequest();
                httpDownload.Url = url;
                httpDownload.FileName = save;
                httpDownload.Directory = new DirectoryInfo("updata-cache");
                DownItemView downItemView = new DownItemView(httpDownload, $"更新  {res.name} 下载");
                TaskView.Add(downItemView);
            }
        }
        /// <summary>
        /// 解压Zip文件到指定目录
        /// </summary>
        /// <param name="zipPath">zip地址</param>
        /// <param name="folderPath">文件夹地址</param>
        public static void DecompressZip(string zipPath, string folderPath)
        {
            DirectoryInfo directoryInfo = new(folderPath);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            ZipFile.ExtractToDirectory(zipPath, folderPath);
        }
    }

    partial class OtherViewModel
    {
        public string Version
        {
            get => _Version;
            set => RaiseAndSetIfChanged(ref _Version, value);
        }
        public string ButtonContent
        {
            get => _ButtonContent;
            set => RaiseAndSetIfChanged(ref _ButtonContent, value);
        }

    }

    partial class OtherViewModel
    {
        public string _Version = MainWindow.GetVersion(); 
        public string _ButtonContent = "检查更新";
    }
}
