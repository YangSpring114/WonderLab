using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Natsurainko.Toolkits.Network.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;
using WonderLab.ViewModels;
using WonderLab.Views;

namespace WonderLab.Modules.Toolkits
{
    /// <summary>
    /// 任务操作工具类
    /// </summary>
    public class TasksTooklit : ViewModelBase
    {
        /// <summary>
        /// 创建模组下载任务
        /// </summary>
        /// <param name="curseForgeModpack"></param>
        /// <param name="control"></param>
        public static void CreateModDownloadTask(CurseForgeModel curseForgeModpack, Control control)
        {
            control.IsEnabled = false;

            if (string.IsNullOrEmpty(App.Data.FooterPath))
                return;

            var folder = App.CurrentGameCore != null
                ? PathConst.GetVersionModsFolder(App.Data.FooterPath, App.CurrentGameCore.Id)
                : Path.Combine(App.Data.FooterPath, "mods");
            Debug.WriteLine(folder);
            var downloaderProcess = new HttpDownloadRequest
            {
                Directory = new DirectoryInfo(folder),
                FileName = curseForgeModpack.CurrentFileInfo.FileName,
                Url = curseForgeModpack.CurrentFileInfo.DownloadUrl
            };

            //CacheResources.DownloaderProcesses.Insert(0, downloaderProcessViewData);
            DownItemView downItemView1 = new(downloaderProcess);
            TaskView.Add(downItemView1);

            var hyperlinkButton = new HyperlinkButton { Content = "转至 祝福终端>任务中心"  };
            hyperlinkButton.Click += (_, _) => Page.NavigatedToTaskView();

            MainWindow.ShowInfoBarAsync("成功",$"已将 \"{curseForgeModpack.CurrentFileInfo.FileName}\" 添加至下载队列",button: hyperlinkButton);     
            MainView.ViewModel.AllTaskCount++;
            control.IsEnabled = true;
        }

        /// <summary>
        /// 创建 Java 安装任务
        /// </summary>
        /// <param name="s"></param>
        /// <param name="control"></param>
        public static void CreateJavaInstallTask(string s, Control control)
        {
            control.IsEnabled = false;

            if (string.IsNullOrEmpty(App.Data.FooterPath))
                return;

            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Java");
            Debug.WriteLine(folder);
            DownItemView downItemView1 = new(s);
            TaskView.Add(downItemView1);

            var hyperlinkButton = new HyperlinkButton { Content = "转至 祝福终端>任务中心" };
            hyperlinkButton.Click += (_, _) => Page.NavigatedToTaskView();

            MainWindow.ShowInfoBarAsync("成功", $"已将 \"{Path.GetFileName(s)}\" 添加至下载队列", button: hyperlinkButton);
            MainView.ViewModel.AllTaskCount++;
            control.IsEnabled = true;
        }
    }
}
