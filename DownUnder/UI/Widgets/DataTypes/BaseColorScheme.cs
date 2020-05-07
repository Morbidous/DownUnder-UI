﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DownUnder.UI.Widgets.DataTypes {
    [DataContract] public class BaseColorScheme : ICloneable {
        /// <summary> Defines the behavior of the theme when updating as well as what colors a widget accesses in the theme. </summary>
        public enum PaletteCategory {
            default_widget = 0,
            text_widget = 1,
            text_edit_widget = 2,
            header_widget = 3
        }

        /// <summary> A reference to the widget that owns this palette. </summary>
        public Widget Parent { get; set; }

        [DataMember] public List<ElementColors> BackGroundColors { get; set; } = new List<ElementColors>();
        [DataMember] public List<ElementColors> TextColors { get; set; } = new List<ElementColors>();
        [DataMember] public List<ElementColors> OutlineColors { get; set; } = new List<ElementColors>();
        [DataMember] public ElementColors InnerScrollBar { get; set; } = new ElementColors();
        [DataMember] public ElementColors OuterScrollBar { get; set; } = new ElementColors();
        
        public BaseColorScheme()=> SetDefaults();
        
        private void SetDefaults() {
            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory))) {
                BackGroundColors.Add(new ElementColors(Color.LimeGreen));
                TextColors.Add(new ElementColors());
                OutlineColors.Add(new ElementColors());
            }
        }

        public ElementColors BackgroundColor => GetBackground(Parent.PaletteUsage);
        public ElementColors GetBackground(PaletteCategory category) => BackGroundColors[(int)category];
        public ElementColors TextColor => GetText(Parent.PaletteUsage);
        public ElementColors GetText(PaletteCategory category) => TextColors[(int)category];
        public ElementColors OutlineColor => GetOutline(Parent.PaletteUsage);
        public ElementColors GetOutline(PaletteCategory category) => OutlineColors[(int)category];
         
        public void Update(GameTime game_time) {
            GetBackground(Parent.PaletteUsage).Update(game_time.GetElapsedSeconds());
            GetText(Parent.PaletteUsage).Update(game_time.GetElapsedSeconds());
            GetOutline(Parent.PaletteUsage).Update(game_time.GetElapsedSeconds());
            if (Parent.ChangeColorOnMouseOver) SetHovered(Parent.IsPrimaryHovered);
        }

        private void SetHovered(bool hovered)
        {
            GetBackground(Parent.PaletteUsage).Hovered = hovered;
            GetText(Parent.PaletteUsage).Hovered = hovered;
            GetOutline(Parent.PaletteUsage).Hovered = hovered;
        }

        public void ForceComplete() {
            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory))) {
                BackGroundColors[(int)category].ForceComplete();
                TextColors[(int)category].ForceComplete();
                OutlineColors[(int)category].ForceComplete();
            }
        }

        public object Clone() {
            BaseColorScheme c = new BaseColorScheme();
            c.BackGroundColors.Clear();
            c.OutlineColors.Clear();
            c.TextColors.Clear();

            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory))) {
                c.BackGroundColors.Add((ElementColors)BackGroundColors[(int)category].Clone());
                c.TextColors.Add((ElementColors)TextColors[(int)category].Clone());
                c.OutlineColors.Add((ElementColors)OutlineColors[(int)category].Clone());
            }

            c.InnerScrollBar = (ElementColors)InnerScrollBar.Clone();
            c.OuterScrollBar = (ElementColors)OuterScrollBar.Clone();

            return c;
        }

        /// <summary> The basic MonoGame-esque CornFlowerBlue theme. </summary>
        public static BaseColorScheme Default {
            get {
                BaseColorScheme r = new BaseColorScheme();

                // default_widget theme
                r.GetOutline(PaletteCategory.default_widget).DefaultColor = Color.Black;
                r.GetOutline(PaletteCategory.default_widget).SpecialColor = Color.Black;
                r.GetOutline(PaletteCategory.default_widget).HoveredColor = Color.Black.ShiftBrightness(1.1f);
                r.GetBackground(PaletteCategory.default_widget).DefaultColor = Color.CornflowerBlue;
                r.GetBackground(PaletteCategory.default_widget).SpecialColor = Color.Yellow;
                r.GetBackground(PaletteCategory.default_widget).HoveredColor = Color.CornflowerBlue.ShiftBrightness(1.1f);
                r.GetText(PaletteCategory.default_widget).DefaultColor = Color.Black;
                r.GetText(PaletteCategory.default_widget).SpecialColor = Color.White;
                r.GetText(PaletteCategory.default_widget).HoveredColor = Color.Black;

                // text_widget theme
                r.GetOutline(PaletteCategory.text_widget).DefaultColor = Color.Black;
                r.GetOutline(PaletteCategory.text_widget).SpecialColor = Color.Black;
                r.GetOutline(PaletteCategory.text_widget).HoveredColor = Color.Black.ShiftBrightness(1.1f);
                r.GetBackground(PaletteCategory.text_widget).DefaultColor = Color.White.ShiftBrightness(0.8f);
                r.GetBackground(PaletteCategory.text_widget).SpecialColor = Color.White;
                r.GetBackground(PaletteCategory.text_widget).HoveredColor = Color.White;
                r.GetText(PaletteCategory.text_widget).DefaultColor = Color.Black;
                r.GetText(PaletteCategory.text_widget).SpecialColor = Color.White;
                r.GetText(PaletteCategory.text_widget).HoveredColor = Color.Black;

                // text_edit_widget theme
                r.GetOutline(PaletteCategory.text_edit_widget).DefaultColor = Color.Black;
                r.GetOutline(PaletteCategory.text_edit_widget).SpecialColor = Color.Black;
                r.GetOutline(PaletteCategory.text_edit_widget).HoveredColor = Color.Black.ShiftBrightness(1.1f);
                r.GetBackground(PaletteCategory.text_edit_widget).DefaultColor = Color.White;
                r.GetBackground(PaletteCategory.text_edit_widget).SpecialColor = Color.White;
                r.GetBackground(PaletteCategory.text_edit_widget).HoveredColor = Color.White;
                r.GetText(PaletteCategory.text_edit_widget).DefaultColor = Color.Black;
                r.GetText(PaletteCategory.text_edit_widget).SpecialColor = Color.White;
                r.GetText(PaletteCategory.text_edit_widget).HoveredColor = Color.Black;

                // header_widget theme
                r.GetOutline(PaletteCategory.header_widget).DefaultColor = Color.Black.ShiftBrightness(1.3f);
                r.GetOutline(PaletteCategory.header_widget).SpecialColor = Color.Black;
                r.GetOutline(PaletteCategory.header_widget).HoveredColor = Color.Black.ShiftBrightness(1.35f);
                r.GetBackground(PaletteCategory.header_widget).DefaultColor = Color.Black.ShiftBrightness(1.1f);
                r.GetBackground(PaletteCategory.header_widget).SpecialColor = Color.Blue.ShiftBrightness(0.5f);
                r.GetBackground(PaletteCategory.header_widget).HoveredColor = Color.Black.ShiftBrightness(1.2f);
                r.GetText(PaletteCategory.header_widget).DefaultColor = Color.White.ShiftBrightness(0.8f);
                r.GetText(PaletteCategory.header_widget).SpecialColor = Color.White;
                r.GetText(PaletteCategory.header_widget).HoveredColor = Color.White;
                
                return r;
            }
        }
        
        /// <summary> A black theme. </summary>
        public static BaseColorScheme Dark {
            get {
                BaseColorScheme r = new BaseColorScheme();

                // default_widget theme
                ElementColors e = r.GetOutline(PaletteCategory.default_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.35f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.2f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.6f);
                e = r.GetBackground(PaletteCategory.default_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.07f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.4f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.15f);
                e = r.GetText(PaletteCategory.default_widget);
                e.DefaultColor = Color.White.ShiftBrightness(0.75f);
                e.SpecialColor = Color.White;
                e.HoveredColor = Color.White;

                // text_widget theme
                e = r.GetOutline(PaletteCategory.text_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.35f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.2f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.5f);
                e = r.GetBackground(PaletteCategory.text_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.1f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.4f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.15f);
                e = r.GetText(PaletteCategory.text_widget);
                e.DefaultColor = Color.White.ShiftBrightness(0.75f);
                e.SpecialColor = Color.White;
                e.HoveredColor = Color.White;

                // text_edit_widget theme
                e = r.GetOutline(PaletteCategory.text_edit_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.6f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.2f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.6f);
                e = r.GetBackground(PaletteCategory.text_edit_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.15f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.4f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.15f);
                e = r.GetText(PaletteCategory.text_edit_widget);
                e.DefaultColor = Color.White.ShiftBrightness(0.75f);
                e.SpecialColor = Color.White;
                e.HoveredColor = Color.White;

                // header_widget theme
                e = r.GetOutline(PaletteCategory.header_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.35f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.2f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.6f);
                e = r.GetBackground(PaletteCategory.header_widget);
                e.DefaultColor = Color.Black.ShiftBrightness(1.07f);
                e.SpecialColor = Color.Red.ShiftBrightness(0.4f);
                e.HoveredColor = Color.Black.ShiftBrightness(1.15f);
                e = r.GetText(PaletteCategory.header_widget);
                e.DefaultColor = Color.White.ShiftBrightness(0.75f);
                e.SpecialColor = Color.White;
                e.HoveredColor = Color.White;

                r.OuterScrollBar.DefaultColor = new Color(0, 0, 0, 0);
                r.OuterScrollBar.SpecialColor = new Color(0, 0, 0, 0);
                r.OuterScrollBar.HoveredColor = new Color(0, 0, 0, 70);

                r.InnerScrollBar.DefaultColor = new Color(100, 100, 100, 100);
                r.InnerScrollBar.SpecialColor = new Color(255, 255, 255, 255);
                r.InnerScrollBar.HoveredColor = new Color(200, 200, 200, 100);

                return r;
            }
        }
    }
}
