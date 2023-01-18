using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PluginLoader
{
    public static class util
    {
        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="Path">
        /// 文件路径
        /// </param>
        /// <returns>
        /// 文件名
        /// </returns>
        [Obsolete]
        public static string GetFileName(string Path)
        {
            string[] tmp = Path.Split('\\');
            return tmp[tmp.Length - 1];
        }
        /// <summary>
        /// 获取无后缀名文件名
        /// </summary>
        /// <param name="FileName">
        /// 文件名
        /// </param>
        /// <returns>
        /// 无后缀名文件名
        /// </returns>
        [Obsolete]
        public static string FileName(string FileName)
        {
            string[] tmp = FileName.Split('.');
            string tmp2 = "";
            for(int i = 0; i < tmp.Length - 1; i++)
            {
                tmp2 += tmp[i];
            }
            return tmp2;
        }
        public static bool IsGuidByReg(string strSrc)
        {
            Regex reg = new Regex("^{[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}}$", RegexOptions.Compiled);
            return reg.IsMatch(strSrc);
        }
    }
}
