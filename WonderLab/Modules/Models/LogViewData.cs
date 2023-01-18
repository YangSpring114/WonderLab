using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.ViewModels;

namespace WonderLab.Modules.Models
{
    public class LogViewData : ViewDataBase<string>
    {
        public LogViewData(string data) : base(data)
        {

        }
    }
}
