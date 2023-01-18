using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Media
{
    public sealed class DrillInNavigationTransitionInfoX : NavigationTransitionInfo
    {
        public bool IsReversed { get; set; } = false;
        public async override void RunAnimation(Animatable ctrl)
        {
            var animation = new Avalonia.Animation.Animation
            {
                Easing = new SplineEasing(0.1, 0.9, 0.2, 1.0),
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(Visual.OpacityProperty, 0.0),
                            //new Setter(ScaleTransform.ScaleXProperty, IsReversed ? 1.5 : 0.0),
                            new Setter(ScaleTransform.ScaleYProperty, IsReversed ? 1.5 : 0.0)
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(Visual.OpacityProperty, 1.0),
                            new Setter(ScaleTransform.ScaleXProperty, IsReversed ? 1.0 : 1.0),
                            new Setter(ScaleTransform.ScaleYProperty, IsReversed ? 1.0 : 1.0)
                        },
                        Cue = new Cue(1d)
                    }
                },
                Duration = TimeSpan.FromSeconds(0.35),
                FillMode = FillMode.Forward
            };

            await animation.RunAsync(ctrl, null);

            (ctrl as IVisual).Opacity = 1;
        }
    }
}
