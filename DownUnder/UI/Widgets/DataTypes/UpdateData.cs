﻿using DownUnder.UI.Widgets.DataTypes;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace DownUnder.UI.Widgets {
    [DataContract] public class UpdateData {
        public UIInputState UIInputState { get; set; }
        public GameTime GameTime { get; internal set; }
        /// <summary> Slightly faster version of <see cref="GameTime.GetElapsedSeconds()"/> </summary>
        public float ElapsedSeconds { get; internal set; }
        /// <summary> Multiply timing values by this value to account for higher refresh rates monitors. Smaller on lower fps displays. (<see cref="ElapsedSeconds"/> * 60) </summary>
        public float SpeedModifier { get; internal set; }
    }
}
