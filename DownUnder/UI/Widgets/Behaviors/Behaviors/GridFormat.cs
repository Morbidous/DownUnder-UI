﻿using DownUnder.UI.Widgets.DataTypes;
using DownUnder.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace DownUnder.UI.Widgets.Behaviors
{
    /// <summary> A behavior that keeps children in a grid formation. </summary>
    public class GridFormat : WidgetBehavior
    {
        private bool _enable_internal_align = true;
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Point Dimensions => new Point(Width, Height);

        public Widget Filler { get; set; }
        public bool DisposeOldOnSet { get; set; } = true;

        public GridFormat(Point dimensions, Widget filler = null)
        {
            Width = dimensions.X;
            Height = dimensions.Y;
            Filler = (Widget)filler?.Clone();
        }
        public GridFormat(int width, int height, Widget filler = null)
        {
            Width = width;
            Height = height;
            Filler = (Widget)filler?.Clone();
        }

        protected override void ConnectToParent()
        {
            if (Filler == null) Filler = DefaultCell();
            GridWriter.InsertFiller(Parent, Width, Height, Filler);
            foreach (Widget child in Parent.Children) child.OnAreaChangePriority += InternalAlign;
            Align(this, EventArgs.Empty);
            Parent.EmbedChildren = false;
            Parent.OnResize += Align;
            Parent.OnAddChild += AddInternalAlign;
            Parent.OnRemoveChild += RemoveInternalAlign;
        }

        protected override void DisconnectFromParent()
        {
            foreach (Widget child in Parent.Children) child.OnAreaChangePriority -= InternalAlign;
            Parent.OnResize -= Align;
            Parent.OnAddChild -= AddInternalAlign;
            Parent.OnRemoveChild -= RemoveInternalAlign;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        private Widget DefaultCell() =>
            new Widget() {
                OutlineSides = Directions2D.DR,
                FitToContentArea = true,
                SnappingPolicy = DiagonalDirections2D.None
            };

        public WidgetList GetRow(int y_row) => GridReader.GetRow(Parent.Children, Width, y_row);
        public WidgetList GetColumn(int x_column) => GridReader.GetColumn(Parent.Children, Width, Height, x_column);
        public Widget this[int x, int y] {
            get => Parent.Children[y * Width + x];
            set
            {
                value.Parent = Parent;
                if (DisposeOldOnSet) Parent.Children[y * Width + x].Dispose();
                Parent.Children[y * Width + x] = value;
            }
        }
        public Point IndexOf(Widget widget) => GridReader.IndexOf(Width, Parent.Children.IndexOf(widget));

        private void Align(object sender, EventArgs args)
        {
            _enable_internal_align = false;
            GridWriter.Align(Parent.Children, Width, Height, Parent.Area.SizeOnly());
            _enable_internal_align = true;
        }

        // Adds and removes InternalAlign to/from child widgets
        private void AddInternalAlign(object sender, EventArgs args) {
            ((Widget)sender).LastAddedWidget.OnAreaChangePriority += InternalAlign;
        }
        private void RemoveInternalAlign(object sender, EventArgs args) {
            ((Widget)sender).LastRemovedWidget.OnAreaChangePriority -= InternalAlign;
        }

        // As usual this will be moved to GridWriter
        /// <summary> Respond to an inner <see cref="Widget"/>'s resizing by resizing the surrounding rows and columns. </summary>
        private void InternalAlign(object sender, RectangleFSetOverrideArgs args)
        {
            if (!_enable_internal_align) return;
            _enable_internal_align = false;
            Widget widget = (Widget)sender;
            Point index = IndexOf(widget);
            if (index == new Point(-1, -1)) throw new Exception($"Given {nameof(Widget)} does not belong to this {nameof(GridFormat)}");

            var difference = widget.Area.Difference(args.PreviousArea);
            args.Override = widget.Area;

            WidgetList current_row = GetRow(index.Y);
            Point2 current_row_minimum_size = current_row.MinimumWidgetSize;
            WidgetList current_column = GetColumn(index.X);
            Point2 current_column_minimum_size = current_column.MinimumWidgetSize;

            if (difference.Top != 0) { // Resizing a widget's top
                if (index.Y == 0) args.Override = args.Override.Value.ResizedBy(difference.Top, Directions2D.U);
                else {
                    WidgetList above_row = GetRow(index.Y - 1); // Resize above row first
                    above_row.ResizeBy(difference.Top, Directions2D.D, true);
                    float above_row_bottom = this[index.X, index.Y - 1].Area.Bottom;

                    foreach (Widget widget_ in current_row) { // Resize current row to match one above (A more convoluted current_row.ResizeBy as to not resize the working widget directly)
                        if (widget_ != widget) widget_.Area = widget_.Area.ResizedBy(widget_.Area.Top - above_row_bottom, Directions2D.U, current_row_minimum_size);
                    }
                    args.Override = args.Override.Value.ResizedBy(args.Override.Value.Top - above_row_bottom, Directions2D.U, current_row_minimum_size);
                }
            }

            if (difference.Left != 0) // mostly same as difference.Top
            {
                if (index.X == 0) args.Override = args.Override.Value.ResizedBy(difference.Left, Directions2D.L);
                else {
                    WidgetList previous_column = GetColumn(index.X - 1);
                    previous_column.ResizeBy(difference.Left, Directions2D.R, true);
                    float previous_column_right = this[index.X - 1, index.Y].Area.Right;

                    foreach (Widget widget_ in current_column) {
                        if (widget_ != widget) widget_.Area = widget_.Area.ResizedBy(widget_.Area.Left - previous_column_right, Directions2D.L, current_column_minimum_size);
                    }
                    args.Override = args.Override.Value.ResizedBy(args.Override.Value.Left - previous_column_right, Directions2D.L, current_column_minimum_size);
                }
            }

            if (difference.Right != 0) {
                if (index.X == Width - 1) args.Override = args.Override.Value.ResizedBy(difference.Right, Directions2D.R);
                else {
                    WidgetList next_column = GetColumn(index.X + 1);
                    next_column.ResizeBy(-difference.Right, Directions2D.L, true);
                    float next_column_left = this[index.X + 1, index.Y].Area.Left;

                    foreach (Widget widget_ in current_column) {
                        if (widget_ != widget) widget_.Area = widget_.Area.ResizedBy(-(widget_.Area.Right - next_column_left), Directions2D.R, current_column_minimum_size);
                    }
                    args.Override = args.Override.Value.ResizedBy(-(args.Override.Value.Right - next_column_left), Directions2D.R, current_column_minimum_size);
                }
            }

            if (difference.Bottom != 0)
            {
                if (index.Y == Height - 1) args.Override = args.Override.Value.ResizedBy(difference.Bottom, Directions2D.D);
                else
                {
                    WidgetList next_row = GetRow(index.Y + 1);
                    next_row.ResizeBy(-difference.Bottom, Directions2D.U, true);
                    float next_row_top = this[index.X, index.Y + 1].Area.Top;

                    foreach (Widget widget_ in current_row)
                    {
                        if (widget_ != widget) widget_.Area = widget_.Area.ResizedBy(-(widget_.Area.Bottom - next_row_top), Directions2D.D, current_row_minimum_size);
                    }
                    args.Override = args.Override.Value.ResizedBy(-(args.Override.Value.Bottom - next_row_top), Directions2D.D, current_row_minimum_size);
                }
            }

            _enable_internal_align = true;
        }
    }
}
