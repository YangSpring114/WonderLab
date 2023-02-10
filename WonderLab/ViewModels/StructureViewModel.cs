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
            "更改了一些字的文本以及图标",
            "修复了切换页面时 UI 显示的问题（应该）",
            "修复了 Linux 的字体报错问题（应该）",
            "添加了网络测试选项的 UI 部分，目前功能处于残废状态",
        };
    }
}
