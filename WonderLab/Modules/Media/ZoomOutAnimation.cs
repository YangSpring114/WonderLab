using Avalonia.Animation.Easings;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Media
{
    /// <summary>
    /// 放大缩小动画
    /// </summary>
    public class ZoomOutAnimation
    {
        public bool IsReversed { get; set; }

        public ZoomOutAnimation(bool isReversed = false) => IsReversed = isReversed;

        /// <summary>
        /// 启动动画
        /// </summary>
        /// <param name="ctrl"></param>
        public async void RunAsync(Animatable ctrl)
        {
            var animation = new Animation
            {
                Easing = new SplineEasing(0.1, 0.9, 0.2, 1.0),
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(Visual.OpacityProperty, 1.0),
                            new Setter(ScaleTransform.ScaleXProperty, IsReversed ? 1.3 : 1.0),
                            new Setter(ScaleTransform.ScaleYProperty, IsReversed ? 1.3 : 1.0)
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(Visual.OpacityProperty, 1.0),
                            new Setter(ScaleTransform.ScaleXProperty, IsReversed ? 1.0 : 1.3),
                            new Setter(ScaleTransform.ScaleYProperty, IsReversed ? 1.0 : 1.3)
                        },
                        Cue = new Cue(1d)
                    }
                },
                Duration = TimeSpan.FromSeconds(0.85),
                FillMode = FillMode.Both
            };

            await animation.RunAsync(ctrl, null);

            (ctrl as IVisual).Opacity = 1;
        }
    }
}
