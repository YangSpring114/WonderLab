using Microsoft.VisualBasic;
using MinecraftLaunch.Modules.Models.Launch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WonderLab.Modules.Const;

namespace WonderLab.Modules.Toolkits
{
    public static class JavaToolkit
    {
        public static JavaInfo GetJavaInfo(string javapath)
        {
            FileInfo fileInfo = new FileInfo(javapath);
            try
            {
                int? num = null;
                string text = null;
                string text2 = "java version \"\\s*(?<version>\\S+)\\s*\"";
                using Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        Arguments = "-version",
                        FileName = InfoConst.IsWindows && fileInfo.Name.EndsWith(".exe") ? Path.Combine(fileInfo.Directory!.FullName,"java.exe") : fileInfo.FullName,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };
                process.Start();
                process.WaitForExit(8000);
                StreamReader standardError = process.StandardError;
                bool is64Bit = false;
                while (standardError.Peek() != -1)
                {
                    string text3 = standardError.ReadLine()!;
                    if (text3.Contains("java version"))
                    {
                        text = new Regex(text2).Match(text3).Groups["version"].Value;
                    }
                    else if (text3.Contains("openjdk version"))
                    {
                        text2 = text2.Replace("java", "openjdk");
                        text = new Regex(text2).Match(text3).Groups["version"].Value;
                    }
                    else if (text3.Contains("64-Bit"))
                    {
                        is64Bit = true;
                    }
                }


                if (string.IsNullOrEmpty(text)) {
                    using Process process2 = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            Arguments = "-XshowSettings:properties -version",
                            FileName = InfoConst.IsWindows && fileInfo.Name.EndsWith(".exe") ? Path.Combine(fileInfo.Directory!.FullName, "java.exe") : fileInfo.FullName,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true
                        }
                    };

                    process2.Start();
                    process2.WaitForExit(8000);
                    StreamReader standardError1 = process2.StandardError;
                    while (standardError.Peek() != -1)
                    {
                        string text3 = standardError.ReadLine()!;
                        if (text3.Contains("java version"))
                        {
                            text = new Regex(text2).Match(text3).Groups["version"].Value;
                        }
                        else if (text3.Contains("openjdk version"))
                        {
                            text2 = text2.Replace("java", "openjdk");
                            text = new Regex(text2).Match(text3).Groups["version"].Value;
                        }
                        else if (text3.Contains("64-Bit"))
                        {
                            is64Bit = true;
                        }
                    }
                }


                string[] array = text.Split(".");
                if (array.Length != 0)
                {
                    num = ((int.Parse(array[0]) == 1) ? new int?(int.Parse(array[1])) : new int?(int.Parse(array[0])));
                }

                return new JavaInfo
                {
                    Is64Bit = is64Bit,
                    JavaDirectoryPath = fileInfo.Directory!.FullName,
                    JavaSlugVersion = Convert.ToInt32(num),
                    JavaVersion = text,
                    JavaPath = fileInfo.FullName
                };
            }
            catch (Exception)
            {
                return new JavaInfo
                {

                };
            }
        }

        [SupportedOSPlatform("windows")]
        public static List<JavaInfo> GetJavas()
        {
            List<JavaInfo> ret = new();
            try
            {
                string environmentVariable = Environment.GetEnvironmentVariable("Path");
                List<string> JavaPreList = new List<string>();
                string[] array = Strings.Split(environmentVariable.Replace("\\\\", "\\").Replace("/", "\\"), ";");
                string[] array2 = array;
                foreach (string obj in array2)
                {
                    string pie = obj.Trim(" \"".ToCharArray());
                    if (!obj.EndsWith("\\"))
                    {
                        pie += "\\";
                    }

                    if (File.Exists(obj + "javaw.exe"))
                    {
                        JavaPreList.Add(pie);
                    }
                }

                DriveInfo[] drives = DriveInfo.GetDrives();
                for (int j = 0; j < drives.Length; j++)
                {
                    JavaSearchFolder(new DirectoryInfo(drives[j].Name), ref JavaPreList, Source: false);
                }

                JavaSearchFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)), ref JavaPreList, Source: false);
                JavaSearchFolder(new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase), ref JavaPreList, Source: false, IsFullSearch: true);
                List<string> JavaWithoutReparse = new List<string>();
                foreach (string Pair2 in JavaPreList)
                {
                    FileSystemInfo Info = new FileInfo(Pair2.Replace("\\\\", "\\").Replace("/", "\\") + "javaw.exe");
                    do
                    {
                        if (!Info.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        {
                            Info = Info is FileInfo ? ((FileInfo)Info).Directory : ((DirectoryInfo)Info).Parent;
                        }
                    }
                    while (Info != null);
                    JavaWithoutReparse.Add(Pair2);
                }

                if (JavaWithoutReparse.Count > 0)
                {
                    JavaPreList = JavaWithoutReparse;
                }

                List<string> JavaWithoutInherit = new List<string>();
                foreach (string Pair in JavaPreList)
                {
                    if (!Pair.Contains("javapath_target_"))
                    {
                        JavaWithoutInherit.Add(Pair);
                    }
                }

                if (JavaWithoutInherit.Count > 0)
                {
                    JavaPreList = JavaWithoutInherit;
                }

                JavaPreList.Sort((x, s) => x.CompareTo(s));
                foreach (string i in JavaPreList)
                {
                    JavaInfo? res = GetJavaInfo(i);
                    if (res != null)
                    {
                        ret.Add(new JavaInfo
                        {
                            Is64Bit = res.Is64Bit,
                            JavaDirectoryPath = i,
                            JavaSlugVersion = res.JavaSlugVersion,
                            JavaVersion = res.JavaVersion,
                            JavaPath = Path.Combine(i, "javaw.exe")
                        });
                    }
                }
            }
            finally
            {
                GC.Collect();
            }
            return ret;
        }
        public static JavaInfo GetCorrectOfGameJava(IEnumerable<JavaInfo> Javas, GameCore gameCore)
        {
            JavaInfo javaInfo = null;
            foreach (JavaInfo Java in Javas)
            {
                if (Java.JavaSlugVersion == gameCore.JavaVersion && Java.Is64Bit)
                {
                    javaInfo = Java;
                }
            }

            if (javaInfo == null)
            {
                foreach (JavaInfo Java2 in Javas)
                {
                    if (Java2.JavaSlugVersion == gameCore.JavaVersion)
                    {
                        javaInfo = Java2;
                    }
                }

                return javaInfo;
            }

            return javaInfo;
        }

        private static void JavaSearchFolder(DirectoryInfo OriginalPath, ref List<string> Results, bool Source, bool IsFullSearch = false)
        {
            try
            {
                if (!OriginalPath.Exists)
                {
                    return;
                }

                string text = OriginalPath.FullName.Replace("\\\\", "\\");
                if (!text.EndsWith("\\"))
                {
                    text += "\\";
                }

                if (File.Exists(text + "javaw.exe"))
                {
                    Results.Add(text);
                }

                foreach (DirectoryInfo item in OriginalPath.EnumerateDirectories())
                {
                    if (!item.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        string text2 = GetFolderNameFromPath(item.Name).ToLower();
                        if (IsFullSearch || item.Parent!.Name.ToLower() == "users" || text2.Contains("java") || text2.Contains("jdk") || text2.Contains("env") || text2.Contains("环境") || text2.Contains("run") || text2.Contains("软件") || text2.Contains("jre") || text2 == "bin" || text2.Contains("mc") || text2.Contains("software") || text2.Contains("cache") || text2.Contains("temp") || text2.Contains("corretto") || text2.Contains("roaming") || text2.Contains("users") || text2.Contains("craft") || text2.Contains("program") || text2.Contains("世界") || text2.Contains("net") || text2.Contains("游戏") || text2.Contains("oracle") || text2.Contains("game") || text2.Contains("file") || text2.Contains("data") || text2.Contains("jvm") || text2.Contains("服务") || text2.Contains("server") || text2.Contains("客户") || text2.Contains("client") || text2.Contains("整合") || text2.Contains("应用") || text2.Contains("运行") || text2.Contains("前置") || text2.Contains("mojang") || text2.Contains("官启") || text2.Contains("新建文件夹") || text2.Contains("eclipse") || text2.Contains("microsoft") || text2.Contains("hotspot") || text2.Contains("runtime") || text2.Contains("x86") || text2.Contains("x64") || text2.Contains("forge") || text2.Contains("原版") || text2.Contains("optifine") || text2.Contains("官方") || text2.Contains("启动") || text2.Contains("hmcl") || text2.Contains("mod") || text2.Contains("高清") || text2.Contains("download") || text2.Contains("launch") || text2.Contains("程序") || text2.Contains("path") || text2.Contains("国服") || text2.Contains("网易") || text2.Contains("ext") || text2.Contains("netease") || text2.Contains("1.") || text2.Contains("启动"))
                        {
                            JavaSearchFolder(item, ref Results, Source);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (Exception)
            {
            }
        }

        private static string GetFolderNameFromPath(string FolderPath)
        {
            if (FolderPath.EndsWith(":\\") || FolderPath.EndsWith(":\\\\"))
            {
                return FolderPath.Substring(0, 1);
            }

            if (FolderPath.EndsWith("\\") || FolderPath.EndsWith("/"))
            {
                FolderPath = Strings.Left(FolderPath, FolderPath.Length - 1);
            }

            return GetFileNameFromPath(FolderPath);
        }

        private static string GetFileNameFromPath(string FilePath)
        {
            if (FilePath.EndsWith("\\") || FilePath.EndsWith("/"))
            {
                throw new Exception("不包含文件名：" + FilePath);
            }

            if (!FilePath.Contains("\\") && !FilePath.Contains("/"))
            {
                return FilePath;
            }

            if (FilePath.Contains("?"))
            {
                FilePath = Strings.Left(FilePath, FilePath.LastIndexOf("?"));
            }

            return Strings.Mid(FilePath, 0);
        }
    }
}
