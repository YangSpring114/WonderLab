using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Models;

namespace WonderLab.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<InfoBarModel> InfoBarItems
        {
            get => _InfoBarItems;
            set => RaiseAndSetIfChanged(ref _InfoBarItems, value);
        }

        private ObservableCollection<InfoBarModel> _InfoBarItems = new();

        public static MainWindowViewModel ViewModel;

        public static TitleBar TitleBar { get; set; }

        public Window Window;

        public MainWindowViewModel(Window window)
        {
            Window = window;
            ViewModel = this;
        }

        public void Colse() => TitleBar.OnClose();
        public void MaxWindowSize() => TitleBar.OnRestore();
        public void MiniWindowSize() => TitleBar.OnMinimize();

        /// <summary>
        /// 光影包拖拽安装方法
        /// </summary>
        public void ShaderPacksInstallAction() {
            if (string.IsNullOrEmpty(App.Data.FooterPath) || string.IsNullOrEmpty(App.Data.SelectedGameCore)) {
                MainWindow.win.CloseAnimaction();
                MainWindow.ShowInfoBarAsync("错误", "您没有选择游戏目录或游戏核心，无法继续安装光影包", InfoBarSeverity.Error);
                return;
            }

            var saveDirpath = PathConst.GetShaderPacksFolder(App.Data.FooterPath, App.Data.SelectedGameCore!);
            Trace.WriteLine($"[信息]光影包将被保存的目录为 {saveDirpath}");

            try {
                if (!Directory.Exists(saveDirpath)) {
                    Directory.CreateDirectory(saveDirpath);
                }

                File.Copy(MainWindow.win.FileNames.First(),
                          Path.Combine(saveDirpath, Path.GetFileName(MainWindow.win.FileNames.First())), true);

                MainWindow.ShowInfoBarAsync("成功", $"光影包已成功安装至选择的游戏核心！", InfoBarSeverity.Success);

                if (MainWindow.win.FileNames.Count > 1) {
                    MainWindow.ShowInfoBarAsync("提示", "您拖入了多个文件，WonderLab会选取第一个安装，余下需重复此操作！");
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误", $"WonderLab 在安装光影包时出现了一些不可描述的异常，异常堆栈如下：\n{ex}", InfoBarSeverity.Error);
            }finally {
                MainWindow.win.CloseAnimaction();
            }
        }

        /// <summary>
        /// 材质包拖拽安装方法
        /// </summary>
        public void ResourcePacksInstallAction()
        {
            if (string.IsNullOrEmpty(App.Data.FooterPath) || string.IsNullOrEmpty(App.Data.SelectedGameCore))
            {
                MainWindow.win.CloseAnimaction();
                MainWindow.ShowInfoBarAsync("错误", "您没有选择游戏目录或游戏核心，无法继续安装材质包", InfoBarSeverity.Error);
                return;
            }

            var saveDirpath = PathConst.GetResourcePacksFolder(App.Data.FooterPath, App.Data.SelectedGameCore!);
            Trace.WriteLine($"[信息]材质包将被保存的目录为 {saveDirpath}");

            try
            {
                if (!Directory.Exists(saveDirpath))
                {
                    Directory.CreateDirectory(saveDirpath);
                }

                File.Copy(MainWindow.win.FileNames.First(),
                          Path.Combine(saveDirpath, Path.GetFileName(MainWindow.win.FileNames.First())), true);

                MainWindow.ShowInfoBarAsync("成功", $"材质包已成功安装至选择的游戏核心！", InfoBarSeverity.Success);

                if (MainWindow.win.FileNames.Count > 1)
                {
                    MainWindow.ShowInfoBarAsync("提示", "您拖入了多个文件，WonderLab会选取第一个安装，余下需重复此操作！");
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误", $"WonderLab 在安装材质包时出现了一些不可描述的异常，异常堆栈如下：\n{ex}", InfoBarSeverity.Error);
            }
            finally
            {
                MainWindow.win.CloseAnimaction();
            }
        }

        /// <summary>
        /// 模组包拖拽安装方法
        /// </summary>
        public void ModnstallAction()
        {
            if (string.IsNullOrEmpty(App.Data.FooterPath) || string.IsNullOrEmpty(App.Data.SelectedGameCore))
            {
                MainWindow.win.CloseAnimaction();
                MainWindow.ShowInfoBarAsync("错误", "您没有选择游戏目录或游戏核心，无法继续安装模组", InfoBarSeverity.Error);
                return;
            }

            var saveDirpath = PathConst.GetModsFolder(App.Data.FooterPath, App.Data.SelectedGameCore!);
            Trace.WriteLine($"[信息]模组将被保存的目录为 {saveDirpath}");

            try
            {
                if (!Directory.Exists(saveDirpath))
                {
                    Directory.CreateDirectory(saveDirpath);
                }

                File.Copy(MainWindow.win.FileNames.First(),
                          Path.Combine(saveDirpath, Path.GetFileName(MainWindow.win.FileNames.First())), true);

                MainWindow.ShowInfoBarAsync("成功", $"模组已成功安装至选择的游戏核心！", InfoBarSeverity.Success);

                if (MainWindow.win.FileNames.Count > 1)
                {
                    MainWindow.ShowInfoBarAsync("提示", "您拖入了多个文件，WonderLab会选取第一个安装，余下需重复此操作！");
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误", $"WonderLab 在安装模组时出现了一些不可描述的异常，异常堆栈如下：\n{ex}", InfoBarSeverity.Error);
            }
            finally
            {
                MainWindow.win.CloseAnimaction();
            }
        }
    }
}
