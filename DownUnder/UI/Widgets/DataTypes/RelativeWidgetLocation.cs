﻿using MonoGame.Extended;
using System;

namespace DownUnder.UI.Widgets.DataTypes
{
    public abstract class RelativeWidgetLocation : ICloneable
    {
        public abstract RectangleF GetLocation(Widget spawner, Widget widget);
        public abstract object Clone();

        public void ApplyLocation(Widget spawner, Widget widget)
        {
            widget.Area = GetLocation(spawner, widget);
        }
    }
}
