﻿using DownUnder.UI.Widgets.Actions;
using DownUnder.Utilities;
using DownUnder.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownUnder.UI.Widgets.Behaviors.Examples.RotatableCubeActions
{
    public class SpinCube : WidgetAction
    {
        public Vector3 Direction = new Vector3();
        public InterpolationSettings Interpolation = new InterpolationSettings() { Interpolation = InterpolationType.inverse, TransitionSpeed = 0.6f };

        ChangingValue<Vector3> _inirtia;

        protected override void ConnectEvents()
        {
            Parent.OnUpdate += Update;
        }

        protected override void DisconnectEvents()
        {
            Parent.OnUpdate -= Update;
        }

        protected override void Initialize()
        {
            _inirtia = new ChangingValue<Vector3>(Direction, new Vector3(), Interpolation);
        }

        protected override bool InterferesWith(WidgetAction action)
        {
            return false;
        }

        protected override bool Matches(WidgetAction action)
        {
            return action is SpinCube;
        }

        public override object InitialClone()
        {
            SpinCube c = (SpinCube)base.InitialClone();
            c.Direction = Direction;
            c.Interpolation = Interpolation;
            return c;
        }

        private void Update(object sender, EventArgs args)
        {
            var cube = Parent.Behaviors.GetFirst<RotatableCube>();
            if (cube == null) return;
            cube.Angle += _inirtia.GetCurrent();
            _inirtia.Update(Parent.UpdateData.ElapsedSeconds);
            if (!_inirtia.IsTransitioning) EndAction();
        }
    }
}