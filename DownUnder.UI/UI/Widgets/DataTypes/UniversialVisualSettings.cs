﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static DownUnder.UI.Widgets.DataTypes.GeneralVisualSettings;

namespace DownUnder.UI.Widgets.DataTypes
{
    public class UniversialVisualSettings
    {
        /// <summary> What this <see cref="Widget"/> should be regarded as regarding several <see cref="WidgetBehavior"/>. </summary>
        [DataMember]
        public VisualRoleType Role { get; set; } = VisualRoleType.default_widget;

        /// <summary> The color palette of this <see cref="Widget"/>. </summary>
        [DataMember]
        public BaseColorScheme Theme { get; set; } = BaseColorScheme.Dark;

        public Color TextColor => Theme.GetText(Role).CurrentColor;
        public Color BackgroundColor => Theme.GetBackground(Role).CurrentColor;
        public Color OutlineColor => Theme.GetOutline(Role).CurrentColor;

        public void Update(float step, bool change_color_on_mouseover, bool is_hovered)
        {
            Theme.Update(Role, step, change_color_on_mouseover, is_hovered);
        }

        public object Clone()
        {
            UniversialVisualSettings result = new UniversialVisualSettings();
            result.Role = Role;
            result.Theme = (BaseColorScheme)Theme.Clone();
            return result;
        }
    }
}
