﻿using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    internal class RoundedButton : Button
    {
        GraphicsPath GetRoundPath(RectangleF rect, int radius)
        {
            var r2 = radius / 2f;
            var graphPath = new GraphicsPath();
            graphPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            graphPath.AddLine(rect.X + r2, rect.Y, rect.Width - r2, rect.Y);
            graphPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            graphPath.AddLine(rect.Width, rect.Y + r2, rect.Width, rect.Height - r2);
            graphPath.AddArc(rect.X + rect.Width - radius,
                             rect.Y + rect.Height - radius, radius, radius, 0, 90);
            graphPath.AddLine(rect.Width - r2, rect.Height, rect.X + r2, rect.Height);
            graphPath.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            graphPath.AddLine(rect.X, rect.Height - r2, rect.X, rect.Y + r2);
            graphPath.CloseFigure();
            return graphPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rect = new RectangleF(0, 0, Width, Height);
            using (var graphPath = GetRoundPath(rect, 8))
            {
                this.Region = new Region(graphPath);
                using (var pen = new Pen(Color.FromArgb(255, 231, 143), 1.55f))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, graphPath);
                }
            }
        }
    }
}