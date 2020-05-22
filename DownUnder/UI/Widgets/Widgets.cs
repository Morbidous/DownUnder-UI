﻿using DownUnder.UI.Widgets.BaseWidgets;
using DownUnder.UI.Widgets.Interfaces;
using MonoGame.Extended;

namespace DownUnder.UI.Widgets
{
    public static class WidgetStuff {
        public static BorderedContainer MenuBar(IParent parent = null, Widget widget = null) {
            BorderedContainer container = new BorderedContainer(widget, parent);
            container.Size = new Point2(400f, 300f);
            SpacedList menu = new SpacedList(container);
            menu.Height = 30f;
            menu.Add(new Label(null, "File") { DrawOutline = false, DrawBackground = false }); ;
            menu.Add(new Label(null, "Edit") { DrawOutline = false, DrawBackground = false });
            menu.Add(new Label(null, "View") { DrawOutline = false, DrawBackground = false });
            container.Borders.Up.Widget = menu;

            return container;
        }
    }
}