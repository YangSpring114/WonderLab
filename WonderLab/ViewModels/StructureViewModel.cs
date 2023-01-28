using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;

namespace WonderLab.ViewModels
{
    public partial class StructureViewModel : ViewModelBase
    {
        public string Publisher => "发布人 Xilu";
        public string StructureVersion => $"Build {InfoConst.LauncherVersion} 的更新内容";
        public string StructureId => $"构建Id {Guid.NewGuid()}";
        public string StructureInfo => string.Join("\n", StructureInfoList);
        public List<string> StructureInfoList => new()
        {            
            "添加了此页面",
            "添加了游戏崩溃探测器，但是你们还看不到",
            "添加了材质包管理页面",
            "添加了模组自定义路径安装功能",
            "实装了自动选取适合游戏的 Java 运行时",
            "实装了拖拽安装游戏组件的功能",
            "修改了本地模组列表样式",
            "修改了新闻列表加载逻辑",
            "修改了本地模组列表加载逻辑",
            "修复了游戏列表内无法删除游戏核心的 Bug",
            "综合 Bug 修复",
            "优化了启动器性能",
        };
    }
}
