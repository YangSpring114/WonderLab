using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WonderLab.Modules.Controls;
using WonderLab.Modules.Interface;

namespace WonderLab.Views
{
    public partial class TaskView : Page
    {
        public static List<ITask> itemView = new();
        public static TaskView task;
        public TaskView()
        {
            InitializeComponent(true);
            task = this;
            Debug.WriteLine(itemView.Count);
            if (itemView.Count != 0)
            {
                foreach (var i in itemView)
                    infopanel.Children.Add(i);
                nullText.IsVisible = false;
            }
            else
                nullText.IsVisible = true;
        }

        public void AddItem(ITask view)
        {
            itemView.Add(view);
            foreach (var i in itemView)
                infopanel.Children.Add(i);
            nullText.IsVisible = false;
        }
        /// <summary>
        /// 添加任务方法
        /// </summary>
        /// <param name="view"></param>
        public static void Add(ITask view)
        {
            if (itemView.Count is not 0 && task is not null) {           
                task.infopanel.Children.Add(view);
                task.nullText.IsVisible = false;
            }
            else if (itemView.Count is not 0 && task is null)
                itemView.Add(view);
            else if (itemView.Count is 0 && task is null)
                itemView.Add(view);
            else if (itemView.Count is 0 && task is not null)
                task.AddItem(view);
        }
        /// <summary>
        /// 移除任务方法
        /// </summary>
        /// <param name="page"></param>
        public static void Remove(Page page)
        {
            task.infopanel.Children.Remove(page);
            if (task.infopanel.Children.Count is 0)
            {
                task.nullText.IsVisible = true;
            }            
        }
    }
}
