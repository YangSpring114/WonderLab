using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Toolkits
{
    public static class McVersionLib
    {
        public static VersionInfo GetVersionInfo(string version)
        {
            int headVersion;
            int midVersion;
            int? lastVersion = null;
            var tmp = version.Split('.');
            if(tmp.Length != 2 && tmp.Length != 3)
            {
                throw new Exception("版本格式不正确");
            }
            else
            {
                headVersion = int.Parse(tmp[0]);
                midVersion = int.Parse(tmp[1]);
                if (tmp.Length == 3)
                {
                    lastVersion = int.Parse(tmp[2]);
                }
            }
            return new VersionInfo
            {
                FullVersion = version,
                HeadVersion = headVersion,
                MidVersion = midVersion,
                LastVersion = lastVersion
            };
        }
    }
    public class VersionInfo
    {
        public string FullVersion = string.Empty;
        public int HeadVersion = 1;
        public int MidVersion = 0;
        public int? LastVersion = null;
    }
}
