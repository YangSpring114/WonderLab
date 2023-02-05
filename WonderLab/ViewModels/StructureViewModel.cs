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
            "添加了开始动画",
            "添加了一些 UI 细节",
            "优化了启动器性能",
            "修复了几处导致崩溃的 Bug"
        };
    }
}
