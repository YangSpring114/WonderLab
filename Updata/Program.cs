using System.Diagnostics;
using System.IO.Compression;


namespace Updataer
{
    public class Updataer
    {
        public static void deleteUploadOaFolder(string folderPath)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                DirectoryInfo fileInfo = new DirectoryInfo(folderPath);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                File.SetAttributes(folderPath, FileAttributes.Normal);  //去除文件的只读属性
                if (Directory.Exists(folderPath))
                {
                    foreach (string f in Directory.GetFileSystemEntries(folderPath))
                    {
                        if (File.Exists(f))   //判断文件夹是否还存在
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine("DeleteDir: " + f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            deleteUploadOaFolder(f);
                        }
                    }
                    //删除空根文件夹。即传来一个文件夹如：/upload/kahnFolder/,则最后将根文件夹kahnFolder也删除掉
                    Directory.Delete(folderPath);
                }
            }
            catch (Exception ex)  // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());   // 异常信息
            }
        }
        /// <summary>
        /// 实现复制整个文件夹到另一个路径，如果存在此文件夹，便覆盖
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                //string[] fileList = Directory.GetFiles(srcPath);// 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);// 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                for (int i = 0; i < fileList.Length; i++)
                {
                    Trace.WriteLine(fileList[i].ToString());
                }

                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 解压Zip文件到指定目录
        /// </summary>
        /// <param name="zipPath">zip地址</param>
        /// <param name="folderPath">文件夹地址</param>
        public static void DecompressZip(string zipPath, string folderPath)
        {
            DirectoryInfo directoryInfo = new(folderPath);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            try
            {
                //Declare a temporary path to unzip your files
                string tempPath = Path.Combine(Path.GetTempPath(), "tempUnzip");
                string extractPath = folderPath;
                ZipFile.ExtractToDirectory(zipPath, tempPath);
                deleteUploadOaFolder(folderPath);
                CopyDir(tempPath, folderPath);
                deleteUploadOaFolder(tempPath);
            }
            catch (Exception)
            {

            }
        }
        public static void Main(string[] args)
        {
            Thread.Sleep(1000);
            if(args.Length == 0)
            {
                if(File.Exists(Path.Combine(Path.GetTempPath(), "WonderLab-updata.exe")))
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), "WonderLab-updata.exe"));
                }
                File.Copy("Updata.exe", Path.Combine(Path.GetTempPath(), "WonderLab-updata.exe"));
                if (File.Exists(Path.Combine(Path.GetTempPath(), "Updata.dll")))
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), "Updata.dll"));
                }
                File.Copy("Updata.dll", Path.Combine(Path.GetTempPath(), "Updata.dll"));
                if (File.Exists(Path.Combine(Path.GetTempPath(), "Updata.deps.json")))
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), "Updata.deps.json"));
                }
                File.Copy("Updata.deps.json", Path.Combine(Path.GetTempPath(), "Updata.deps.json"));
                if (File.Exists(Path.Combine(Path.GetTempPath(), "Updata.runtimeconfig.json")))
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), "Updata.runtimeconfig.json"));
                }
                File.Copy("Updata.runtimeconfig.json", Path.Combine(Path.GetTempPath(), "Updata.runtimeconfig.json"));
                Process pro = new Process();
                pro.StartInfo.FileName = Path.Combine(Path.GetTempPath(), "WonderLab-updata.exe");
                pro.StartInfo.Arguments = "-updata " + Path.Combine("updata-cache", "updata.zip") + " " + Path.Combine(Environment.CurrentDirectory) + " " + Path.Combine(System.Environment.CurrentDirectory,"WonderLab.exe");
                File.Delete(Path.Combine("updata-cache", "UpdataNextTime"));
                pro.Start();
                return;
            }
            else if(args.Length == 4)
            {
                if(args[0] == "-updata")
                {
                    DecompressZip(args[1], args[2]);
                    Process pro = new Process();
                    pro.StartInfo.FileName = args[3];
                    pro.Start();
                }
            }
        }
    }
}