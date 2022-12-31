using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Views;

namespace WonderLab.Modules.Controls
{
    /// <summary>
    /// 滚动条显示修改版，此类将不再被重写
    /// </summary>
    public sealed class ScrollContentPresenterX : ScrollContentPresenter
    {
        /// <summary>
        /// 此方法将不再被重写
        /// </summary>
        /// <param name="e"></param>
        protected sealed override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            //base.OnPointerWheelChanged(e);
            //The Extent is scrollviewer Maximum of the contents and the Viewport is Container Control Maximum of the contents
            //Debug.WriteLine($"Extent Height：{Extent.Height} Extent Width:{Extent.Width}");
            //Debug.WriteLine($"Viewport Height：{Viewport.Height} Viewport Width:{Viewport.Width}");
            if (Extent.Height > Viewport.Height || Extent.Width > Viewport.Width)
            {
                var scrollable = Child as ILogicalScrollable;
                var isLogical = scrollable?.IsLogicalScrollEnabled == true;

                var x = Offset.X;
                var y = Offset.Y;
                var delta = e.Delta;

                // KeyModifiers.Shift should scroll in horizontal direction. This does not work on every platform. 
                // If Shift-Key is pressed and X is close to 0 we swap the Vector.
                if (MathUtilities.IsZero(delta.X))
                {
                    delta = new Vector(delta.Y, delta.X);
                }

                if (782 > Viewport.Width)
                {
                    double width = isLogical ? scrollable!.ScrollSize.Width : 50;
                    x += -delta.X * width;
                    x = Math.Max(x, 0);
                    x = Math.Min(x, Extent.Width - Viewport.Width);
                }

                Vector newOffset = new Vector(x, y);
                bool offsetChanged = newOffset != Offset;
                Offset = newOffset;

                e.Handled = offsetChanged;
            }
        }
    }
}
