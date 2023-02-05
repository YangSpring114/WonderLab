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
            "修复了自动更新的问题",
            "测试了账户管理（千万别用）",
            "综合 Bug 修复"
        };
    }
}
