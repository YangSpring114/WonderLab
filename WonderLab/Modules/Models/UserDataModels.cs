using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;

namespace WonderLab.Modules.Models
{
    public class UserDataModels
    {
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string SkinHeadImage { get; set; }
        public string UserUuid { get; set; }
        public string UserAccessToken { get; set; } = "";
        public string UserRefreshToken { get; set; } = "";
        public string AIJvm { get; set; } = "";
    }

    public class UserSkinDemo : UserDataModels
    {
        public async Task<IBitmap> GetSkin()
        {//https://crafatar.com/avatars/95883f77eef84bc6b7274f9c754a5a2c
            return await Task.Run(async () =>
            {
                HttpClient httpClient = new();
                MemoryStream v1 = new(await httpClient.GetByteArrayAsync($"https://crafatar.com/avatars/{UserUuid}"));
                return new Bitmap(v1);
            });
        }
    }
}
