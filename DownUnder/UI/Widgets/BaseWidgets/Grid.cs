﻿using DownUnder.UI.Widgets.Interfaces;
using DownUnder.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;

//this needs prefered dimensions?

namespace DownUnder.UI.Widgets.BaseWidgets
{
    /// <summary>
    /// A grid of widgets. Cells are empty Layouts by default.
    /// </summary>
    public class Grid : Widget
    {
        #region Fields

        /// <summary>
        /// A jagged array of all the contained widgets. (Widgets[x][y])
        /// </summary>
        public List<List<Widget>> widgets = new List<List<Widget>>();

        /// <summary>
        /// This is broken for a possibly obvious reason. It might not matter.
        /// </summary>
        private const int _RESIZING_ACCURACY = 1;

        #endregion Fields

        #region Public Properties

        /// <summary>
        /// The number of widgets tall and wide this grid consists of.
        /// </summary>
        public Point Dimensions
        {
            get
            {
                if (widgets.Count == 0)
                {
                    return new Point();
                }

                return new Point(widgets.Count, widgets[0].Count);
            }
        }

        #endregion Public Properties

        #region Constructors

        public Grid(IWidgetParent parent = null)
            : base(parent)
        {
            SetDefaults();
            //if (IsGraphicsInitialized)
            //{
            //    InitializeGraphics();
            //}
        }

        public Grid(IWidgetParent parent, int x_length, int y_length, Widget filler = null)
            : base(parent)
        {
            SetDefaults();
            //if (IsGraphicsInitialized)
            //{
            //    InitializeGraphics();
            //}

            if (filler == null)
            {
                filler = DefaultCell();
            }

            filler.SetOwnership(this);

            CreateWidgetGrid(x_length, y_length, filler);
        }

        private void SetDefaults()
        {
            Size = new Point2(100, 100);
            DrawBackground = false;
            DrawOutline = true;
        }

        #endregion Constructors

        #region Private Methods

        private void ExpandAllWidgets(Size2 modifier)
        {
            RectangleF new_area;
            for (int x = 0; x < widgets.Count; x++)
            {
                for (int y = 0; y < widgets[0].Count; y++)
                {
                    new_area = widgets[x][y].Area;
                    new_area.Width *= modifier.Width;
                    new_area.Height *= modifier.Height;
                    widgets[x][y].Area = new_area;
                }
            }
        }

        protected void CreateWidgetGrid(int x_length, int y_length, Widget filler)
        {
            for (int x = 0; x < x_length; x++)
            {
                widgets.Add(new List<Widget>());
                for (int y = 0; y < y_length; y++)
                {
                    object clone = (Widget)filler.Clone();
                    ((Widget)clone).SetOwnership(this);
                    ((Widget)clone).InitializeGraphics();
                    widgets[x].Add((Widget)clone);
                }
            }
            AlignWidgets();
        }

        public void InsertRow(List<Widget> widgets, int y)
        {
            if (widgets.Count != Dimensions.X)
            {
                throw new Exception("Given list of widgets' length doesn't match the X dimension of this grid.");
            }

            AlignWidgets();
        }

        public void InsertColumn(List<Widget> widgets, int x)
        {
            if (widgets.Count != Dimensions.Y)
            {
                throw new Exception("Given list of widgets' length doesn't match the Y dimension of this grid.");
            }

            AlignWidgets();
        }

        private void AlignWidgets()
        {
            AutoSizeAllWidgets();
            SpaceAllCells();
        }

        /// <summary>
        /// This will find the longest/tallest widget in each row/collumn and make every other element match.
        /// </summary>
        private void AutoSizeAllWidgets()
        {
            for (int x = 0; x < widgets.Count; x++)
            {
                AutoSizeCollumn(x);
            }

            for (int y = 0; y < widgets[0].Count; y++)
            {
                AutoSizeRow(y);
            }
        }

        private void AutoSizeCollumn(int collumn)
        {
            float max_x = 0;
            for (int y = 0; y < widgets[0].Count; y++)
            {
                max_x = MathHelper.Max(max_x, widgets[collumn][y].Width);
            }

            for (int y = 0; y < widgets[0].Count; y++)
            {
                widgets[collumn][y].Width = max_x;
            }
        }

        private void AutoSizeRow(int row)
        {
            float max_y = 0;
            for (int x = 0; x < widgets.Count; x++)
            {
                max_y = MathHelper.Max(max_y, widgets[x][row].Height);
            }

            for (int x = 0; x < widgets.Count; x++)
            {
                widgets[x][row].Height = max_y;
            }
        }

        private void SpaceAllCells()
        {
            Point2 position = new Point();

            for (int x = 0; x < widgets.Count; x++)
            {
                position.Y = 0;
                for (int y = 0; y < widgets[0].Count; y++)
                {
                    widgets[x][y].Position = position;
                    position.Y += widgets[x][y].Height;
                }
                position.X += widgets[x][0].Width;
            }
        }

        /// <summary>
        /// The default cell is a layout
        /// </summary>
        /// <param name="graphics_device"></param>
        /// <returns></returns>
        protected Layout DefaultCell()
        {
            // Create cell
            Layout default_widget = new Layout(this)
            {
                DrawBackground = true,
                DrawOutline = true
            };
            default_widget.SnappingPolicy = DiagonalDirections2D.TopRight_BottomLeft_TopLeft_BottomRight;
            default_widget.OutlineSides = Directions2D.DownRight;
            default_widget.Area = new Rectangle(15, 15, 15, 15);
            default_widget.FitToContentArea = true;
            return default_widget;
        }

        #endregion Private Methods

        #region Public Methods

        public Widget GetCell(int x, int y)
        {
            return widgets[x][y];
        }

        public void SetCell(int x, int y, Widget widget, bool update_parent = false)
        {
            widget.SetOwnership(this);

            widget.Area = widgets[x][y].Area;
            widgets[x][y] = widget;
            UpdateArea(update_parent);
        }

        public void AddToCell(int x, int y, Widget widget, bool update_parent = false)
        {
            if (widget.IsOwned)
            {
                if (widget.Parent != this)
                {
                    throw new Exception($"(Name = {Name}, widget.Name {widget.Name}) Cannot use widget owned by something else.");
                }
            }

            if (!(widgets[x][y] is Layout))
            {
                throw new Exception($"Cannot add widget to cell[{x}][{y}], because cell[{x}][{y}] is not a Layout. widgets[{x}][{y}].GetType() = {widgets[x][y].GetType()}");
            } 
            ((Layout)widgets[x][y]).AddWidget(widget);
            UpdateArea(update_parent);
        }

        #endregion Public Methods

        #region Overrides

        public override RectangleF Area
        {
            get
            {
                if (Dimensions.X == 0 || Dimensions.Y == 0)
                {
                    return new RectangleF();
                }

                Point2 size = new Point();

                for (int x = 0; x < widgets.Count; x++)
                {
                    size.X += widgets[x][0].Width;
                }

                if (widgets.Count != 0)
                {
                    for (int y = 0; y < widgets[0].Count; y++)
                    {
                        size.Y += widgets[0][y].Height;
                    }
                }

                return new RectangleF(base.Area.Position, size);
            }
            set
            {
                base.Area = value;
                RectangleF area = Area;
                if (value == area)
                {
                    return;
                }

                // Resize the grid as many times as _RESIZING_ACCURACY allows.
                Size2 modifier;
                int i = 0;
                while ((Area.ToRectangle().Size != value.Size.ToPoint()) && (i < _RESIZING_ACCURACY))
                {
                    modifier = new Size2(value.Width / area.Width, value.Height / area.Height);
                    ExpandAllWidgets(modifier);
                    i++;
                }

                SpaceAllCells();
                if (Name == "Property grid")
                {
                    Debug.WriteLine($"Property grid result = {Area}");
                }
            }
        }

        protected override void UpdateArea(bool update_parent)
        {
            AlignWidgets();
            base.UpdateArea(update_parent);
        }

        protected override object DerivedClone()
        {
            throw new NotImplementedException();
        }

        public override List<Widget> Children
        {
            get
            {
                List<Widget> children = new List<Widget>();
                Point2 dimensions = Dimensions;
                for (int x = 0; x < dimensions.X; x++)
                {
                    for (int y = 0; y < dimensions.Y; y++)
                    {
                        children.Add(widgets[x][y]);
                    }
                }
                return children;
            }
        }

        #endregion Overrides
    }
}