using FluentAvalonia.UI.Controls;
using MinecaftOAuth.Authenticator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Modules.Base;
using WonderLab.Modules.Const;
using WonderLab.Modules.Models;
using WonderLab.Modules.Toolkits;
using WonderLab.Views;

namespace WonderLab.ViewModels
{
    //Binding
    public partial class UsersViewModel : ViewModelBase
    {
        public string SelectedAuthenticator { get => _SelectedAuthenticator; set => RaiseAndSetIfChanged(ref _SelectedAuthenticator, value); }

        public string UrlTextBoxText { get => _UrlTextBoxText; set => RaiseAndSetIfChanged(ref _UrlTextBoxText, value); }

        public string TextBoxText { get => _TextBoxText; set => RaiseAndSetIfChanged(ref _TextBoxText, value); }

        public string PasswordBoxText { get => _PasswordBoxText; set => RaiseAndSetIfChanged(ref _PasswordBoxText, value); }

        public string LoginDialogTitle { get => _LoginDialogTitle; set => RaiseAndSetIfChanged(ref _LoginDialogTitle, value); }

        public string DeviceInfo { get => _DeviceInfo; set => RaiseAndSetIfChanged(ref _DeviceInfo, value); }

        public string DeviceTips { get => _DeviceTips; set => RaiseAndSetIfChanged(ref _DeviceTips, value); }

        public List<string> Authenticators => new()
        {
            "微软验证",
            "第三方验证",
            "离线验证"
        };

        public List<UserModels> Users { get => _Users; set => RaiseAndSetIfChanged(ref _Users, value); }

        public UserModels CurrentUser { get => _CurrentUser; set { if (RaiseAndSetIfChanged(ref _CurrentUser, value) && _CurrentUser is not null) App.Data.SelectedUser = _CurrentUser.ToUserDataModel(); } }
        
        public bool PasswordBoxVisibility { get => _PasswordBoxVisibility; set => RaiseAndSetIfChanged(ref _PasswordBoxVisibility, value); }

        public bool TextBoxVisibility { get => _TextBoxVisibility; set => RaiseAndSetIfChanged(ref _TextBoxVisibility, value); }

        public bool FirstBoxVisibility { get => _FirstBoxVisibility; set => RaiseAndSetIfChanged(ref _FirstBoxVisibility, value); }

        public bool ProgressBarVisibility { get => _ProgressBarVisibility; set => RaiseAndSetIfChanged(ref _ProgressBarVisibility, value); }
        
        public bool IsCilpboardButtonVisibility { get => _IsCilpboardButtonVisibility; set => RaiseAndSetIfChanged(ref _IsCilpboardButtonVisibility, value); }
    }
    //Methods
    partial class UsersViewModel
    {
        public UsersViewModel() => GetSaveUserInfo();

        public async ValueTask AuthAsync()
        {
            FirstBoxVisibility = false;
            PasswordBoxVisibility = false;
            TextBoxVisibility = false;
            ProgressBarVisibility = false;
            LoginDialogTitle = SelectedAuthenticator;
            UsersView.ShowLoginDialog();
            if (SelectedAuthenticator is "微软验证")
            {
                ProgressBarVisibility = true;
                MicrosoftAuthenticator ma = new()
                {
                    AuthType = MinecaftOAuth.Module.Enum.AuthType.Access,
                    ClientId = "9fd44410-8ed7-4eb3-a160-9f1cc62c824c"
                };
                var code = await ma.GetDeviceInfo();
                DeviceInfo = $"使用一次性验证代码 {code.UserCode} 登录您的账户";
                IsCilpboardButtonVisibility = true;
                Process.Start(new ProcessStartInfo(code.VerificationUrl)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
                _DeviceCode = code.UserCode;
                DeviceTips = $"请使用浏览器访问 {code.VerificationUrl} 并输入代码 {code.UserCode} 以完成登录";
                try
                {
                    await ma.GetTokenResponse(code);
                    var v = await ma.AuthAsync((a) =>
                    {
                        Debug.WriteLine(a);
                    });

                    var user = new UserDataModels()
                    {
                        UserAccessToken = v.AccessToken,
                        UserRefreshToken = v.RefreshToken,
                        UserName = v.Name,
                        UserUuid = v.Uuid.ToString(),
                        UserType = "微软账户"
                    };

                    App.Data.UserList.Add(user);
                    MainWindow.ShowInfoBarAsync("添加账户成功：", $"{user.UserType} {user.UserName} 欢迎回来,{user.UserName}", InfoBarSeverity.Success);
                    GetSaveUserInfo();
                    UsersView.CloseDialog();
                }
                catch (Exception ex)
                {
                    MainWindow.ShowInfoBarAsync("添加账户失败：", ex.ToString(), InfoBarSeverity.Error);
                    UsersView.CloseDialog();
                    Debug.WriteLine(ex);
                }
            }
            else if(SelectedAuthenticator is "第三方验证")
            {
                FirstBoxVisibility = true;
                PasswordBoxVisibility = true;
                TextBoxVisibility = true;
            }
            else if (SelectedAuthenticator is "离线验证")
                TextBoxVisibility = true;
            else
                MainWindow.ShowInfoBarAsync("错误", "您选择了未知的验证器！", InfoBarSeverity.Error);
        }

        public async void FindllyAuth()
        {
            if (SelectedAuthenticator is "第三方验证")
            {
                try
                {
                    YggdrasilAuthenticator yggdrasilAuthenticator = new(UrlTextBoxText, TextBoxText, PasswordBoxText);
                    var res = await yggdrasilAuthenticator.AuthAsync(x => Debug.WriteLine(x));
                    res.ToList().ForEach(x =>
                    {// /api/yggdrasil
                        string p = new FileInfo(PathConst.MainDirectory).FullName + @"\" + "authlib-injector.jar";
                        string ag = $"-javaagent:{p}={x.YggdrasilServerUrl}";
                        Debug.WriteLine(ag);
                        var str = x.YggdrasilServerUrl.Replace("/api/yggdrasil", string.Empty);
                        DeviceTips = "";
                        var user = new UserDataModels()
                        {
                            UserAccessToken = x.AccessToken,
                            UserName = x.Name,
                            UserUuid = x.Uuid.ToString(),
                            SkinHeadImage = $"{str}/avatar/player/{x.Name}",
                            UserType = "第三方账户",
                            AIJvm = ag,
                        };
                        App.Data.UserList.Add(user);
                        MainWindow.ShowInfoBarAsync("添加账户成功：", $"{user.UserType} {user.UserName} 欢迎回来,{user.UserName}", InfoBarSeverity.Success);
                    });
                    UsersView.CloseDialog();
                }
                catch (Exception ex)
                {
                    UsersView.CloseDialog();
                    MainWindow.ShowInfoBarAsync("错误：", $"WonderLab 遇到了不可描述的异常！\n最后的异常堆栈信息：\n{ex}", InfoBarSeverity.Error);
                }
            }
            else if (SelectedAuthenticator is "离线验证")
            {
                OfflineAuthenticator offlineAuthenticator = new(TextBoxText);
                var v = await offlineAuthenticator.AuthAsync();
                DeviceTips = "";
                var user = new UserDataModels()
                {
                    UserAccessToken = v.AccessToken,
                    UserName = v.Name,
                    UserUuid = v.Uuid.ToString(),
                    UserType = "离线账户"
                };
                App.Data.UserList.Add(user);
                MainWindow.ShowInfoBarAsync("添加账户成功：", $"{user.UserType} {user.UserName} 欢迎回来,{user.UserName}", InfoBarSeverity.Success);
                UsersView.CloseDialog();
            }
            GetSaveUserInfo();
            StringsRefresh();
        }

        public void StringsRefresh()
        {
            SelectedAuthenticator = "微软验证";
            UrlTextBoxText = "";
            TextBoxText = "";
            PasswordBoxText = "";
            DeviceInfo = "";
            DeviceTips = "";
        }

        public async void GetSaveUserInfo()
        {
            BackgroundWorker worker = new();
            worker.DoWork += (_, _) =>
            {
                Users = new(App.Data.UserList.Select(x => new UserModels(x)));
            };

            worker.RunWorkerAsync();
            //CurrentUser = Users.GetUserInIndex(App.Data.SelectedUser.UserName);
        }

        public async void CopyCodeAsync()
        {
            if (!string.IsNullOrEmpty(_DeviceCode))
                await ClipboardToolkt.CopyToClipboard(_DeviceCode);
        }
    }
    //Models
    partial class UsersViewModel
    {
        public bool _TextBoxVisibility = false;
        public bool _IsCilpboardButtonVisibility = false;
        public bool _PasswordBoxVisibility = false;
        public bool _ProgressBarVisibility = false;
        public bool _FirstBoxVisibility = false;
        public string _SelectedAuthenticator = "微软验证";
        public string _UrlTextBoxText = string.Empty;
        public string _TextBoxText = string.Empty;
        public string _PasswordBoxText = string.Empty;
        public string _DeviceInfo = string.Empty;
        public string _DeviceTips = string.Empty;
        public string _DeviceCode = string.Empty;
        public string _LoginDialogTitle = string.Empty; 
        public List<UserModels> _Users = new();
        public UserModels _CurrentUser = new(App.Data.SelectedUser);
        //public string _PasswordBoxText = string.Empty;
    }
}
