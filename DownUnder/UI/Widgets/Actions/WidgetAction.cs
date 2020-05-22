﻿using DownUnder.UI.Widgets.Interfaces;
using DownUnder.UI.Widgets.Behaviors;
using System;

namespace DownUnder.UI.Widgets.Actions {
    /// <summary> A <see cref="WidgetAction"/> acts as a plugin for a <see cref="Widget"/>. Adds additional behaviors to the <see cref="Widget"/>'s <see cref="EventHandler"/>s. Differs from <see cref="WidgetBehavior"/> as this deletes itself on finishing execution. </summary>
    public abstract class WidgetAction : INeedsWidgetParent {
        Widget _parent_backing;
        public enum DuplicatePolicyType {
            /// <summary> Override any existing duplicate <see cref="WidgetAction"/>. </summary>
            override_,
            /// <summary> Execute alongside any duplicate <see cref="WidgetAction"/>. </summary>
            parallel,
            /// <summary> Execute after any duplicate <see cref="WidgetAction"/> are done. </summary>
            wait,
            /// <summary> Do not execute if a duplicate <see cref="WidgetAction"/> is being executed. </summary>
            cancel
        }

        public enum DuplicateDefinitionType
        {
            /// <summary> The most strict standard for catching duplicate actions. Defines a duplicate where the existing <see cref="WidgetAction"/> does a similar task. </summary>
            interferes_with,
            /// <summary> Defines a duplicate where the existing <see cref="WidgetAction"/> is attempting to reach the same end result. </summary>
            matches_result
        }

        /// <summary> How this <see cref="WidgetAction"/> will execute if a duplicate action is already being executed. </summary>
        public DuplicatePolicyType DuplicatePolicy { get; set; } = DuplicatePolicyType.wait;

        /// <summary> How this <see cref="WidgetAction"/> will determine if an existing <see cref="WidgetAction"/> is a duplicate. </summary>
        public DuplicateDefinitionType DuplicateDefinition { get; set; } = DuplicateDefinitionType.interferes_with;

        public Widget Parent {
            get => _parent_backing;
            set {
                if (_parent_backing != null) {
                    if (_parent_backing == value) return;
                    throw new Exception($"{nameof(WidgetAction)}s cannot be reused. Call {nameof(InitialClone)} to create a copy.");
                }
                _parent_backing = value;
                ConnectToParent();
            }
        }

        public bool HasParent => Parent != null;
        public bool IsCompleted { get; protected set; } = false;

        public bool IsDuplicate(WidgetAction action)
        {
            if (DuplicateDefinition == DuplicateDefinitionType.interferes_with) return InterferesWith(action);
            if (DuplicateDefinition == DuplicateDefinitionType.matches_result) return Matches(action);
            throw new Exception();
        }

        protected abstract void ConnectToParent();
        protected abstract void DisconnectFromParent();
        protected abstract bool InterferesWith(WidgetAction action);
        protected abstract bool Matches(WidgetAction action);

        public abstract object InitialClone();

        protected void EndAction() {
            DisconnectFromParent();
            _parent_backing.Actions.Remove(this);
            IsCompleted = true;
        }
    }
}