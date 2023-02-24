using System.Diagnostics;
using System.Text.RegularExpressions;

namespace javaInfo;

/// <summary>
/// # Java，包含 Path 和 Version 属性
/// </summary>
public class Java
{
    public string Path { get; set; }
    public string Version { get; set; }

    public Java(string path)
    {
        Path = path;
        Version = string.Empty;
    }

    public Java(string path, string version)
    {
        Path = path;
        Version = version;
    }
}

/// <summary>
/// # JavaInfo 用于获得Java信息
/// </summary>
public class JavaInfo
{
    /// <summary>
    /// ## 在终端执行 shell arg
    /// </summary>
    /// <param name="shell"></param>
    /// <param name="arg"></param>
    /// <returns></returns>
    private static HashSet<Java> RunProcess(string shell, string arg)
    {
        var startInfo = new ProcessStartInfo(shell, arg)
        {
            WorkingDirectory = Directory.GetCurrentDirectory(),
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };
        var cmd = new Process
        {
            StartInfo = startInfo
        };
        cmd.Start();
        if (cmd.HasExited)
        {
            cmd.Close();
            return new HashSet<Java>();
        }

        var reader = cmd.StandardOutput.ReadToEnd();
        var javas = new HashSet<Java>();
        foreach (var i in reader.Split('\n'))
        {
            if (i.Replace("\r", string.Empty) == string.Empty)
                continue;
            javas.Add(new Java(i.Replace("\r", string.Empty)));
        }

        cmd.Close();
        return javas;
    }

    /// <summary>
    /// ## 寻找已安装的Java **多平台处理**
    /// </summary>
    /// <returns></returns>
    private static HashSet<Java> SearchJava()
    {
        if (OperatingSystem.IsWindows())
            return RunProcess("where.exe", "javaw.exe");

        if (OperatingSystem.IsMacOS())
            return MacJavas();

        if (OperatingSystem.IsLinux())
            return LinuxJavas();
        //Unix系，仅返回环境变量的java路径
        return RunProcess("which", "java");
    }

    /// <summary>
    /// ## MacOS 的 Java查询
    /// </summary>
    /// <returns></returns>
    private static HashSet<Java> MacJavas()
    {
        const string javaHomePath =  "/Library/Java/JavaVirtualMachines";
        var javas = new HashSet<Java>();
        foreach (var i in Directory.GetDirectories(javaHomePath))
        {
            if (!Directory.Exists(i + "/Contents/Home/bin"))
                continue;
            javas.Add(new Java(i + "/Contents/Home/bin/java"));
        }
        return javas;
    }
    
    /// <summary>
    /// ## Linux 的 Java查询 **处理不同的发行版**
    /// </summary>
    /// <returns></returns>
    private static HashSet<Java> LinuxJavas()
    {
        const string binPath = "/usr/bin/";
        var jvmDirectory = string.Empty;
        if (Directory.Exists("/usr/lib/jvm"))
            jvmDirectory = "/usr/lib/jvm";
        else if (Directory.Exists("/usr/lib64/jvm"))
            jvmDirectory = "/usr/lib64/jvm";

        if (jvmDirectory == string.Empty)
            return new HashSet<Java>();
        //Arch系 包管理器为 pacman
        if (File.Exists(binPath + "pacman"))
            return ArchJavas(Directory.GetDirectories(jvmDirectory));
        //Debian系 包管理器为 apt
        if (File.Exists(binPath + "apt"))
            return DebianJavas(Directory.GetDirectories(jvmDirectory));
        //Red hat系 包管理器为 yum/dnf
        if (File.Exists(binPath + "yum") || File.Exists(binPath + "dnf"))
            return RedHatJavas(Directory.GetDirectories(jvmDirectory));
        //opensuse系 包管理器为 zypper
        if (File.Exists(binPath + "zypper"))
            return OpensuseJavas(Directory.GetDirectories(jvmDirectory));
        //其他系，难精确查找，仅返回环境变量中的java路径
        return RunProcess("which", "java");
    }
    
    /// <summary>
    /// ### Arch系 Linux 的Java查询
    /// </summary>
    /// <param name="javaRootPaths"></param>
    /// <returns></returns>
    private static HashSet<Java> ArchJavas(string[] javaRootPaths)
    {
        var javas = new HashSet<Java>();
        foreach (var i in javaRootPaths)
        {
            if (i.Contains("default"))
                continue;
            if (Directory.Exists(i+"/bin"))
                javas.Add(new Java(i + "/bin/java"));
        }
        return javas;
    }
    
    /// <summary>
    /// ### Debian系 Linux 的Java查询
    /// </summary>
    /// <param name="javaRootPaths"></param>
    /// <returns></returns>
    private static HashSet<Java> DebianJavas(string[] javaRootPaths)
    {
        var javas = new HashSet<Java>();
        foreach (var i in javaRootPaths)
        {
            if (!Directory.Exists(i+"/bin"))
                continue;
            javas.Add(new Java(i + "/bin/java"));
        }

        return javas;
    }

    /// <summary>
    /// ### 红帽系 Linux 的Java查询
    /// </summary>
    /// <param name="javaRootPaths"></param>
    /// <returns></returns>
    private static HashSet<Java> RedHatJavas(string[] javaRootPaths)
    {
        var javas = new HashSet<Java>();
        foreach (var i in javaRootPaths)
        {
            if (i.Contains("jre"))
                continue;
            if (Directory.Exists(i+"/bin"))
                javas.Add(new Java(i + "/bin/java"));
        }
        return javas;
    }
    
    /// <summary>
    /// ### OpenSuse Linux 的Java查询
    /// </summary>
    /// <param name="javaRootPaths"></param>
    /// <returns></returns>
    private static HashSet<Java> OpensuseJavas(string[] javaRootPaths)
    {
        var javas = new HashSet<Java>();
        foreach (var i in javaRootPaths)
        {
            if (i.Contains("jre"))
                continue;
            if (Directory.Exists(i+"/bin"))
                javas.Add(new Java(i + "/bin/java"));
        }
        return javas;
    }

    /// <summary>
    /// ## 通过读 java 根目录下的 release 文件来获得版本信息
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string FindJavaVersion(string path)
    {
        if (!File.Exists(Path.GetFullPath(path)))
            //不存在release文件时，使用 `java -version`命令查看版本
            return FindJavaVersionInCmd(path);
        try
        {
            var sr = new StreamReader(Path.GetFullPath(path));
            return ReadRelease(sr,path);
        }
        catch (Exception)
        {
            return "Cannot Open Release File!";
        }
    }

    /// <summary>
    /// ## 通过 java -version 来获得版本信息
    /// 注意！如果用 `cmd.StandardOutput.ReadToEnd()` 则会返回空字符串
    /// 因此！应该使用 `cmd.StandardError.ReadToEnd()`
    /// 奇怪的特性
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string FindJavaVersionInCmd(string path)
    {
        string javaPath;
        if (OperatingSystem.IsWindows())
            javaPath = path.Replace("release",@"bin\java.exe");
        else
            javaPath = path.Replace("release", "bin/java");
        var cmd = RunJavaVersionCommand(javaPath);
        cmd.Start();
        var versionText = cmd.StandardError.ReadToEnd();
        cmd.Close();
        //使用正则表达式来提取 “” 中的内容，并且删除双引号
        return Regex.Match(versionText, "[\"].*?[\"]").ToString().Replace("\"",string.Empty);
    }

    /// <summary>
    /// 运行 java -version 命令并返回结果
    /// </summary>
    /// <param name="javaPath"></param>
    /// <returns></returns>
    private static Process RunJavaVersionCommand(string javaPath)
    {
        var startInfo = new ProcessStartInfo(javaPath, "-version")
        {
            WorkingDirectory = Directory.GetCurrentDirectory(),
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };
        return new Process{
            StartInfo = startInfo
        };
    }

    /// <summary>
    /// ## 读取 Release 文件
    /// 若读取错误则使用java -version命令读取
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string ReadRelease(StreamReader sr, string path)
    {
        while (!sr.EndOfStream)
        {
            var versionText = sr.ReadLine();
            if (versionText == null)
            {
                sr.Close();
                return "Unknow Version";
            }

            if (versionText.Contains("JAVA_VERSION="))
            {
                sr.Close();
                return versionText.Replace("JAVA_VERSION=", string.Empty).Replace("\"", string.Empty);
            }
        }
        //读取错误时
        sr.Close();
        return FindJavaVersionInCmd(path);
    }
    
    /// <summary>
    /// ## 寻找并设置好现有 java 的版本
    /// </summary>
    /// <param name="javas"></param>
    /// <returns></returns>
    private static HashSet<Java> SetJavaVersion(HashSet<Java> javas)
    {
        var res = new HashSet<Java>();
        foreach (var i in javas)
        {
            string javaReleasePath;
            if (OperatingSystem.IsWindows())
                javaReleasePath = i.Path.Replace(@"bin\javaw.exe", "release");
            else
                javaReleasePath = i.Path.Replace("bin/java", "release");
            res.Add(new Java(i.Path, FindJavaVersion(javaReleasePath)));
        }

        return res;
    }

    /// <summary>
    /// # 寻找 Java及其版本
    /// </summary>
    /// <returns></returns>
    public static HashSet<Java> FindJava()
    {
        return SetJavaVersion(SearchJava());
    }
} 