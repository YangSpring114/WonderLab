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
            "完善了自动更新（目前仅限 Windows）",
            "修复了自动找 Java 出现重复 Java 的情况",
            "修复了一处 UI 显示问题",
            "修复了一处可能导致启动器崩溃的 Bug",
            "优化了启动器性能",
        };
    }
}
