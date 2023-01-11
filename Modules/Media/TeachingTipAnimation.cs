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
    /// 教学提示控件动画类
    /// </summary>
    public class TeachingTipAnimation
    {
        public bool IsReversed { get; set; }

        public TeachingTipAnimation(bool isReversed = false) => IsReversed = isReversed;

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
                            new Setter(Visual.OpacityProperty, 0.0),
                            new Setter(ScaleTransform.ScaleXProperty, IsReversed ? 1.0 : 0.0),
                            new Setter(ScaleTransform.ScaleYProperty, IsReversed ? 1.0 : 0.0)
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(Visual.OpacityProperty, 1.0),
                            new Setter(ScaleTransform.ScaleXProperty, IsReversed ? 0 : 1.0),
                            new Setter(ScaleTransform.ScaleYProperty, IsReversed ? 0 : 1.0)
                        },
                        Cue = new Cue(1d)
                    }
                },
                Duration = TimeSpan.FromSeconds(IsReversed ? 0.35 : 0.75),
                FillMode = FillMode.Forward
            };

            await animation.RunAsync(ctrl, null);

            (ctrl as IVisual).Opacity = 1;
        }
    }
}
