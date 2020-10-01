﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace DownUnder.UI.Widgets.Behaviors.Visual
{
    class BGEffectTest : WidgetBehavior
    {
        public override string[] BehaviorIDs { get; protected set; } = new string[] { DownUnderBehaviorIDs.COSMETIC_HIGH_PERFORMANCE };
        Effect effect;

        protected override void Initialize()
        {
            //Parent.DrawingMode = Widget.DrawingModeType.use_render_target;
            if (Parent.ParentWindow != null) LoadEffect(this, EventArgs.Empty);
        }

        protected override void ConnectEvents()
        {
            Parent.OnDrawBackgroundEffects += ApplyEffect;
            Parent.OnParentWindowSet += LoadEffect;
        }

        protected override void DisconnectEvents()
        {
            Parent.OnDrawBackgroundEffects -= ApplyEffect;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        private void LoadEffect(object sender, EventArgs args)
        {
            Parent.ParentWidget.DrawingMode = Widget.DrawingModeType.use_render_target;
            effect = Parent.ParentWindow.ParentGame.Content.Load<Effect>("DownUnder Native Content/Effects/BGEffectTest");
        }

        private void ApplyEffect(object sender, DrawBGEffectsArgs args)
        {
            effect.CurrentTechnique.Passes[0].Apply();
            Parent.SpriteBatch.FillRectangle(new RectangleF(20,20,20,20), Color.Red);
            Parent.SpriteBatch.Draw(args.ParentRender, args.ChildAreaInRender.ToRectangle(), new Rectangle(0,0,500,500), Color.White);
            Parent.SpriteBatch.DrawString(Parent.WindowFont, args.ChildAreaInRender.ToString(), new Vector2(), Color.White);
        }
    }
}