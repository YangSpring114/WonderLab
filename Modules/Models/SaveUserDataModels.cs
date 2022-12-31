using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{
    public class SaveUserDataModels : UserDataModels
    {
        public string SkinHeadUri { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string UserUuid { get; set; }
        public string UserToken { get; set; }
    }
}
