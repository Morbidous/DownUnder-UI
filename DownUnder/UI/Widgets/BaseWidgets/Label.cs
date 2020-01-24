﻿using DownUnder.UI.Widgets.DataTypes;
using DownUnder.UI.Widgets.Interfaces;
using DownUnder.UI.Widgets.WidgetControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Todo: add TextArea property

namespace DownUnder.UI.Widgets.BaseWidgets
{
    [DataContract]
    public class Label : Widget
    {
        #region Fields
        
        private string _text_backing = "";
        private bool _editing_enabled_backing = false;
        private TextCursor _text_cursor;

        #endregion Fields

        #region Public Properties

        /// <summary> Set to enable/disable user editing of this text. </summary>
        [DataMember] public virtual bool EditingEnabled
        {
            get => _editing_enabled_backing;
            set
            {
                _text_cursor.Active = value ? false : _text_cursor.Active;
                _editing_enabled_backing = value;
            }
        }

        /// <summary> The displayed text of this widget. </summary>
        [DataMember] public string Text
        {
            get
            {
                if (_text_cursor.Active)
                {
                    return _text_cursor.Text;
                }
                else
                {
                    return _text_backing;
                }
            }
            set
            {
                _text_backing = value;
                UpdateArea(true);
            }
        }

        /// <summary> What kind of text is allowed to be entered in this label. </summary>
        [DataMember] public TextEntryRuleSet TextEntryRules { get; set; } = TextEntryRuleSet.String;

        /// <summary> Whet set to true the area of this label will try to cover any text within. </summary>
        [DataMember] public bool ConstrainAreaToText { get; set; } = false;

        /// <summary> Area of the text within the label. </summary>
        public RectangleF TextArea => IsGraphicsInitialized ? SpriteFont.MeasureString(Text).ToRectSize() : Position.ToRectPosition(1, 1);

        /// <summary> True if the user is editting text. </summary>
        public bool IsBeingEdited => _text_cursor == null ? false : _text_cursor.Active;

        /// <summary>  Palette of the background while editing text. </summary>
        public UIPalette TextEditBackgroundPalette { get; internal set; } = new UIPalette();

        #endregion Public Properties

        #region Constructors

        public Label(IWidgetParent parent = null)
            : base(parent)
        {
            SetDefaults();
        }

        public Label(IWidgetParent parent, string text)
            : base(parent)
        {
            SetDefaults();
            Text = text;
        }

        public Label(IWidgetParent parent, SpriteFont sprite_font, string text = "")
            : base(parent)
        {
            SpriteFont = sprite_font;
            SetDefaults();
            Text = text;
        }

        private void SetDefaults()
        {
            DrawBackground = true;
            EnterConfirms = true;
            BackgroundColor.DefaultColor = Color.White.ShiftBrightness(0.84f);
            BackgroundColor.HoveredColor = Color.White;
            TextEditBackgroundPalette.DefaultColor = Color.White;
            TextEditBackgroundPalette.HoveredColor = Color.White;
            TextEditBackgroundPalette.ForceComplete();
            
            OnDraw += DrawText;
            OnSelectOff += DisableEditing;
            OnUpdate += Update;
            OnConfirm += ConfirmEdit;
            if (IsGraphicsInitialized) InitializeCursor(this, EventArgs.Empty);
            else OnGraphicsInitialized += InitializeCursor;
        }

        #endregion Constructors

        #region Overrides

        /// <summary> When set to true pressing enter while this widget is the primarily selected one will trigger confirmation events. </summary>
        public override bool EnterConfirms
        {
            get => TextEntryRules.IsSingleLine;
            set => TextEntryRules.IsSingleLine = value;
        }

        /// <summary> Minimum size allowed when setting this widget's area. (in terms of pixels on a 1080p monitor) </summary>
        public override Point2 MinimumSize
        {
            get => !ConstrainAreaToText || !IsGraphicsInitialized ? base.MinimumSize : base.MinimumSize.Max(SpriteFont.MeasureString(Text));
            set => base.MinimumSize = value;
        }

        /// <summary> The UIPalette used for the background color. </summary>
        public override UIPalette BackgroundColor
        { 
            get => IsBeingEdited ? TextEditBackgroundPalette : base.BackgroundColor;
            internal set => base.BackgroundColor = value;
        }

        protected override object DerivedClone()
        {
            Label result = new Label();
                
            result.Text = Text;
            result.Name = Name;
            result.TextEntryRules = (TextEntryRuleSet)TextEntryRules.Clone();
            result.EditingEnabled = EditingEnabled;
            result.TextEditBackgroundPalette = (UIPalette)TextEditBackgroundPalette.Clone();
            result.ConstrainAreaToText = ConstrainAreaToText;

            return result;
        }

        public override List<Widget> Children
        {
            get => new List<Widget>();
        }
        
        #endregion Overrides

        #region Event Handlers

        /// <summary>
        /// Invoked when the text has been edited.
        /// </summary>
        public EventHandler<EventArgs> OnTextEdited;

        #endregion

        #region Events

        private void Update(object sender, EventArgs args)
        {
            _text_cursor.Update();
        }

        private void DisableEditing(object sender, EventArgs args)
        {
            _text_cursor.Active = false;
        }

        private void DrawText(object sender, EventArgs args)
        {
            _text_cursor.Draw();
            SpriteBatch.DrawString(SpriteFont, Text, new Vector2(), TextColor.CurrentColor);
        }

        private void ConfirmEdit(object sender, EventArgs args)
        {
            if (IsBeingEdited)
            {
                _text_cursor.ApplyFinalCheck();
                Text = _text_cursor.Text;
                DisableEditing(this, EventArgs.Empty);
                OnTextEdited?.Invoke(this, EventArgs.Empty);
            }
        }

        private void InitializeCursor(object sender, EventArgs args)
        {
            _text_cursor = new TextCursor(this);
        }

        #endregion
    }
}