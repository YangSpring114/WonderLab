using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WonderLab.Modules.Controls
{
    public class FrontalSkinX : TemplatedControl
    {
        public Bitmap HeadSource
        {
            get => GetValue(HeadSourceProperty);
            set => SetValue(HeadSourceProperty, value);
        }

        public Bitmap BodySource
        {
            get => GetValue(BodySourceProperty);
            set => SetValue(BodySourceProperty, value);
        }

        public Bitmap LeftArmSource
        {
            get => GetValue(LeftArmSourceProperty);
            set => SetValue(LeftArmSourceProperty, value);
        }

        public Bitmap RightArmSource
        {
            get => GetValue(RightArmSourceProperty);
            set => SetValue(RightArmSourceProperty, value);
        }

        public Bitmap LeftLegSource
        {
            get => GetValue(LeftLegSourceProperty);
            set => SetValue(LeftLegSourceProperty, value);
        }

        public Bitmap RightLegSource
        {
            get => GetValue(RightLegSourceProperty);
            set => SetValue(RightLegSourceProperty, value);
        }
        

        public static readonly StyledProperty<Bitmap> HeadSourceProperty =
            AvaloniaProperty.Register<FrontalSkinX, Bitmap>(nameof(HeadSource));

        public static readonly StyledProperty<Bitmap> BodySourceProperty =
            AvaloniaProperty.Register<FrontalSkinX, Bitmap>(nameof(BodySource));

        public static readonly StyledProperty<Bitmap> LeftArmSourceProperty =
            AvaloniaProperty.Register<FrontalSkinX, Bitmap>(nameof(LeftArmSource));

        public static readonly StyledProperty<Bitmap> RightArmSourceProperty =
            AvaloniaProperty.Register<FrontalSkinX, Bitmap>(nameof(RightArmSource));

        public static readonly StyledProperty<Bitmap> LeftLegSourceProperty =
            AvaloniaProperty.Register<FrontalSkinX, Bitmap>(nameof(LeftLegSource));

        public static readonly StyledProperty<Bitmap> RightLegSourceProperty =
            AvaloniaProperty.Register<FrontalSkinX, Bitmap>(nameof(RightLegSource));

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
        }
    }
}
