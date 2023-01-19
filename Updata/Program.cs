using System.Diagnostics;
using System.IO.Compression;

namespace Updataer
{
    public class Updataer
    {
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
                string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "tempUnzip");
                string extractPath = Directory.GetCurrentDirectory();
                ZipFile.ExtractToDirectory(zipPath, tempPath);

                //build an array of the unzipped files
                string[] files = Directory.GetFiles(tempPath);

                foreach (string file in files)
                {
                    FileInfo f = new FileInfo(file);
                    //Check if the file exists already, if so delete it and then move the new file to the extract folder
                    if (File.Exists(Path.Combine(extractPath, f.Name)))
                    {
                        File.Delete(Path.Combine(extractPath, f.Name));
                        File.Move(f.FullName, Path.Combine(extractPath, f.Name));
                    }
                    else
                    {
                        File.Move(f.FullName, Path.Combine(extractPath, f.Name));
                    }
                }
                //Delete the temporary directory.
                Directory.Delete(tempPath);
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