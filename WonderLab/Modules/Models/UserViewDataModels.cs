using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using Flurl;
using MinecaftOAuth.Authenticator;
using Natsurainko.Toolkits.Network;
using ReactiveUI.Fody.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SkiaSharp;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

namespace WonderLab.Modules.Models
{
    public class UserModels : ViewModelBase
    {
        public UserModels(UserDataModels user)
        {
            if (user is not null)
            {
                Name = user.UserName;
                Type = user.UserType;
                RefreshToken = user.UserRefreshToken;
                AccessToken = user.UserAccessToken;
                Uuid = user.UserUuid;
                Jvm = user.AIJvm;
                SkinLink = user.SkinHeadImage;
                _ = DownloadImageAsync();
            }
        }

        public UserModels Current => this;
        public string _Name;
        public string Name { get => _Name; set => _Name = value; }

        public string Jvm { get; set; }

        public string Type { get; set; }

        public string Uuid { get; set; }

        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }

        public string AuthState
        {
            get => _AuthState;
            set => RaiseAndSetIfChanged(ref _AuthState, value);
        }

        public string SkinLink
        {
            get => _link;
            set => RaiseAndSetIfChanged(ref _link, value);
        }

        public bool Loading
        {
            get => _load;
            set => RaiseAndSetIfChanged(ref _load, value);
        }

        public byte[] SkinBytes { get; set; }

        public Bitmap? Icon
        {
            get => _SkinBitmapIcon;
            set => RaiseAndSetIfChanged(ref _SkinBitmapIcon, value);
        }

        public Bitmap? _SkinBitmapIcon = null;

        public string _AuthState = "";

        public string _link = "";

        public bool _load = true;

        public async void Auth()
        {
            try
            {
                AuthState = "即将开始刷新验证";

                switch (Type)
                {
                    case "离线账户":
                        OfflineAuthenticator offlineAuthenticator = new(Name);
                        await offlineAuthenticator.AuthAsync();
                        AuthState = "当前状态：准备就绪";
                        break;
                    case "第三方账户":
                        AuthState = "当前状态：准备就绪";
                        break;
                    case "微软账户":
                        MicrosoftAuthenticator microsoftAuthenticator = new()
                        {
                            RefreshToken = RefreshToken,
                            AuthType = MinecaftOAuth.Module.Enum.AuthType.Refresh,
                            ClientId = InfoConst.ClientId,
                        };
                        await microsoftAuthenticator.AuthAsync((w) =>
                        {
                            AuthState = "当前状态：" + w;
                            if (w is "微软登录（刷新验证）完成")
                                AuthState = "当前状态：准备就绪";
                        });
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowInfoBarAsync("错误，验证刷新失败：", ex.ToString(), InfoBarSeverity.Error);
            }
        }

        public async ValueTask DownloadImageAsync()
        {
            await Task.Run(async() =>
            {
                try
                {
                    if (Type.Contains("离线账户"))
                    {
                        var al = AvaloniaLocator.Current.GetService<IAssetLoader>();
                        var stream = al.Open(new Uri("resm:WonderLab.Resources.sdf.png"));
                        
                        Icon = new Bitmap(stream);
                    }
                    else if (Type.Contains("微软"))
                    {
                        var url = await WebToolkit.GetUserSkinUrl(Uuid);
                        var btyes = await (await HttpWrapper.HttpGetAsync(url)).Content.ReadAsByteArrayAsync();
                        SkinBytes= btyes;
                        var Image = await BitmapToolkit.CropSkinHeadImage(btyes);

                        Icon = BitmapToolkit.ResizeImage(Image, 120, 120).ToBitmap();
                    }
                    else
                    {
                        var stream = await new HttpClient().GetByteArrayAsync(SkinLink);
                        SkinBytes = stream;
                        using var savestream = new MemoryStream();
                        BitmapToolkit.ResizeImage((Image<Rgba32>)Image.Load(stream), 512, 512).Save(savestream, new PngEncoder());
                        savestream.Position = 0;

                        Icon = new Bitmap(savestream);
                    }
                }
                catch (Exception)
                {

                }
                Loading = false;
            }, default);
        }

        public UserDataModels ToUserDataModel()
        {
            return new UserDataModels()
            {
                AIJvm = Jvm,
                UserRefreshToken = RefreshToken,
                UserName = Name,
                UserType = Type,
                UserAccessToken = AccessToken,
                UserUuid = Uuid,
            };
        }
    }
}