using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Interactivity;
using Metsys.Bson;
using Avalonia.Layout;
using System.IO;
using System.Net.Http;
using WonderLab.Views;
using WonderLab.Modules.Models;
using System.Diagnostics;
using WonderLab.Modules.Base;
using WonderLab.ViewModels;

namespace WonderLab.Modules.Cards
{
    public class UserCard : TemplatedControl
    {
        public static readonly StyledProperty<string> HeaderProperty =
    AvaloniaProperty.Register<UserCard, string>(nameof(Header));

        public static readonly StyledProperty<string> DescriptionProperty =
            AvaloniaProperty.Register<UserCard, string>(nameof(Description));

        public static readonly StyledProperty<string> HeardImageUriProperty =
            AvaloniaProperty.Register<UserCard, string>(nameof(HeardImageUri));

        public static readonly StyledProperty<IImage> HeardImageProperty =
            AvaloniaProperty.Register<UserCard, IImage>(nameof(HeardImage));

        public static readonly StyledProperty<IControl> ActionButtonProperty =
            AvaloniaProperty.Register<UserCard, IControl>(nameof(ActionButton));

        public static readonly StyledProperty<object> ContentProperty =
            ContentControl.ContentProperty.AddOwner<UserCard>();

        public static readonly StyledProperty<string> UuidProperty =
    AvaloniaProperty.Register<UserCard, string>(nameof(Uuid));

        public static readonly StyledProperty<string> TokenProperty =
    AvaloniaProperty.Register<UserCard, string>(nameof(Token));


        public string Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string Uuid
        {
            get => GetValue(UuidProperty);
            set => SetValue(UuidProperty, value);
        }

        public string Token
        {
            get => GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        public string Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public IImage HeardImage
        {
            get => GetValue(HeardImageProperty);
            set => SetValue(HeardImageProperty, value);
        }

        public string HeardImageUri
        {
            get => GetValue(HeardImageUriProperty);
            set
            {
                try
                {
                    SetValue(HeardImageUriProperty, value);
                    if (HeardImageUri is not null)
                    {
                        TaskBase.InvokeAsync(async () =>
                        {
                            Bitmap bitmap = null;
                            HttpClient hc = new();
                            var v = await hc.GetByteArrayAsync(HeardImageUri);
                            MemoryStream memoryStream = new(v);
                            bitmap = new Bitmap(memoryStream);
                            HeardImage = bitmap;
                        });
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public IControl ActionButton
        {
            get => GetValue(ActionButtonProperty);
            set => SetValue(ActionButtonProperty, value);
        }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _layoutRoot = e.NameScope.Find<Border>("LayoutRoot");        
            _layoutRoot.PointerPressed += OnLayoutRootPointerPressed;
            _layoutRoot.PointerReleased += OnLayoutRootPointerReleased;
            _layoutRoot.PointerCaptureLost += OnLayoutRootPointerCaptureLost;
        }

        private void OnLayoutRootPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                _isPressed = true;
                PseudoClasses.Set(":pressed", true);

                MainWindow.ShowInfoBarAsync("成功：", $"账户 { Header } 将作为启动时使用的账户", FluentAvalonia.UI.Controls.InfoBarSeverity.Success);

                App.Data.SelectedUser = new()
                {
                    UserName = Header,
                    UserAccessToken = Token,
                    UserUuid = Uuid,
                    UserType = Description,
                    SkinHeadImage = HeardImageUri
                };

                HomeView.HomeViewModel.UserInfo = App.Data.SelectedUser;
            }
        }

        private void OnLayoutRootPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            var pt = e.GetCurrentPoint(this);
            if (_isPressed && pt.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                _isPressed = false;

                PseudoClasses.Set(":pressed", false);


            }
        }

        private void OnLayoutRootPointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
        {
            _isPressed = false;
            PseudoClasses.Set(":pressed", false);
        }

        private bool _isPressed;
        private Border _layoutRoot;

    }
}
