using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Views;

namespace WonderLab.Modules.Controls
{
    /// <summary>
    /// 导航页
    /// </summary>
    public class Page : UserControl
    {
        /// <summary>
        /// 导航至方法
        /// </summary>
        public virtual void OnNavigatedTo() => Debug.WriteLine("This is a not ovveride Page!");

        #region BlessingView
        public static void NavigatedToTaskView()
        {
            MainView.mv.FrameView.Navigate(typeof(BlessingView));
            foreach (NavigationViewItem i in BlessingView.view.RootNavigationView.MenuItems)
            {
                i.IsSelected = false;
                if (i.Content is "任务中心")
                {
                    i.IsSelected = true;
                    continue;
                }
            }
            BlessingView.view.FrameView.Navigate(typeof(TaskView), null, new SlideNavigationTransitionInfo());
        }

        public static void NavigatedToDownView()
        {
            try
            {
                MainView.mv.FrameView.Navigate(typeof(BlessingView));
                foreach (NavigationViewItem i in BlessingView.view.RootNavigationView.MenuItems)
                {
                    i.IsSelected = false;
                    if (i.Content is "下载")
                    {
                        i.IsSelected = true;
                        continue;
                    }
                }
                BlessingView.view.FrameView.Navigate(typeof(DownView));
            }
            catch (Exception)
            {

            }
        }

        public static void NavigatedToNewsView()
        {
            try
            {
                MainView.mv.FrameView.Navigate(typeof(BlessingView));
                foreach (NavigationViewItem i in BlessingView.view.RootNavigationView.MenuItems)
                {
                    i.IsSelected = false;
                    if (i.Content is "新闻")
                    {
                        i.IsSelected = true;
                        continue;
                    }
                }
                BlessingView.view.FrameView.Navigate(typeof(NewsView));
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region PropertyView
        public static void PropertyViewInitialize(MainPropertyView view)
        {
            foreach (NavigationViewItem i in view.NavigationView.MenuItems)
            {
                i.IsSelected = false;
                if (i.Content is "详细信息")
                {
                    i.IsSelected = true;
                    continue;
                }
            }
            PropertyView.GameCore = MainPropertyView.SelectGameCore;
            view.contentFrame.Navigate(typeof(PropertyView));
        }
        #endregion

        #region SettingsView
        public static void NavigatedToGameSettingView()
        {
            //MainView.mv.FrameView.Navigate(typeof(SettingView));
            foreach (NavigationViewItem i in SettingView.sv.RootNavigationView.MenuItems)
            {
                i.IsSelected = false;
                if (i.Content is "启动设置")
                {
                    i.IsSelected = true;
                    continue;
                }
            }
            GameSettingView gsv = new();
            SettingView.sv.FrameView.Navigate(gsv.GetType());
        }
        #endregion
    }
}
