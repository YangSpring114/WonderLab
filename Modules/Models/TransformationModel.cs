using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{
    /// <summary>
    /// 转换模型
    /// </summary>
    public class TransformationModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public bool HasModLoader { get; set; }
    }
}
