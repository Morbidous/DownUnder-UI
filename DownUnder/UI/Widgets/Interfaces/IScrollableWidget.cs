﻿using DownUnder.UI.Widgets.WidgetElements;
using MonoGame.Extended;

namespace DownUnder.UI.Widgets.Interfaces {
    /// <summary> Widgets that have an inner area. </summary>
    public interface IScrollableWidget {
        Scroll ScrollBars { get; }
        RectangleF ContentArea { get; }
        Point2 Scroll { get; }
        bool FitToContentArea { get; set; }
    }
}