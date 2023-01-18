using Avalonia.Controls;
using Avalonia.Media.Imaging;
using FluentAvalonia.UI.Controls;
using MinecaftOAuth;
using MinecaftOAuth.Authenticator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Toolkits;
using WonderLab.ViewModels;

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
                Auth();
                _ = DownloadImageAsync();
            }
        }
        public UserModels Current => this;

        public string Name { get; set; }

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

        public Bitmap Icon
        {
            get => _SkinBitmapIcon;
            set => RaiseAndSetIfChanged(ref _SkinBitmapIcon, value);
        }

        public Bitmap _SkinBitmapIcon = null;

        public string _AuthState = "";

        public string _link = "";

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
            await Task.Run(async delegate
            {
                if (Type is not "离线账户")
                {
                     var stream = await new HttpClient().GetByteArrayAsync(SkinLink);
                     using var ms = new MemoryStream(stream);
                     Icon = new Bitmap(ms);
                }
                else
                {
                    Icon = (BitmapToolkit.GetAssetsImage("resm:WonderLab.Resources.sdf.png") as Bitmap);
                }
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
                SkinHeadImage = $"https://crafatar.com/avatars/{Uuid}"
            };
        }
    }
}
