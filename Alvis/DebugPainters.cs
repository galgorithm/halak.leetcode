using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using NUnit.Framework;

namespace Alvis
{
    public enum DebugColor
    {
        Black,
        Red,
        Green,
        Blue,
        Yellow,
    }

    public interface IDebugPainter
    {
        void DrawPoint(double x, double y, DebugColor color);

        void DrawLine(double x1, double y1, double x2, double y2, DebugColor color);

        /// <summary>
        /// 2차 베지어 곡선을 그립니다.
        /// 겹친 선분들을 표현할 때 유용합니다.
        /// `d`는 선분의 중점과 베지어 곡선 조절점 간의 거리 스케일을 의미합니다.
        /// </summary>
        void DrawLine(double x1, double y1, double x2, double y2, double d, DebugColor color);

        void DrawRectangle(double x, double y, double width, double height, DebugColor color);
    }

    public static class DebugPainters
    {
        private static readonly ConcurrentDictionary<string, int> savedImages = new ConcurrentDictionary<string, int>();

        public static void Draw(Action<IDebugPainter> handle)
        {
            var currentTest = TestContext.CurrentContext.Test;

            var boundsPainter = new BoundsPainter();
            handle(boundsPainter);

            var bounds = boundsPainter.PaddedBounds;
            var width = 0.0;
            var height = 0.0;
            if (bounds.Width < bounds.Height)
            {
                width = Math.Max(Math.Floor(bounds.Width), 512.0);
                height = width * bounds.Height / bounds.Width;
            }
            else
            {
                height = Math.Max(Math.Floor(bounds.Height), 512.0);
                width = height * bounds.Width / bounds.Height;
            }

            using (var canvas = new Bitmap((int)Math.Ceiling(height), (int)Math.Ceiling(width)))
            using (var g = Graphics.FromImage(canvas))
            {
                g.Clear(Color.White);

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingMode = CompositingMode.SourceOver;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.None;

                var scale = (float)Math.Min(Math.Ceiling(width) / bounds.Width, Math.Ceiling(height) / bounds.Height);
                g.ResetTransform();
                g.TranslateTransform(-bounds.Left, -bounds.Top, MatrixOrder.Append);
                g.ScaleTransform(scale, scale, MatrixOrder.Append);

                handle(new GraphicsPainter(g, 1.0f / scale));

                var ordinal = savedImages.AddOrUpdate(currentTest.FullName, 1, (_, value) => value + 1);
                var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "leetcode", currentTest.MethodName);
                if (Directory.Exists(basePath) == false)
                    Directory.CreateDirectory(basePath);
                canvas.Save(Path.Combine(basePath, ordinal.ToString("0000") + ".png"));
            }
        }

        private static RectangleF MakeRect(double left, double top, double right, double bottom)
            => RectangleF.FromLTRB((float)left, (float)top, (float)right, (float)bottom);

        private sealed class BoundsPainter : IDebugPainter
        {
            private RectangleF mergedBounds;

            public RectangleF PaddedBounds
            {
                get
                {
                    var bounds = mergedBounds;
                    bounds.Inflate(bounds.Width / 10.0f, bounds.Height / 10.0f);
                    return bounds;
                }
            }

            public BoundsPainter()
            {
                mergedBounds = Rectangle.Empty;
            }

            public void DrawPoint(double x, double y, DebugColor color)
                => Merge(MakeRect(x, y, x, y));
            public void DrawLine(double x1, double y1, double x2, double y2, DebugColor color)
                => Merge(MakeRect(Math.Min(x1, x2), Math.Min(y1, y2), Math.Max(x1, x2), Math.Max(y1, y2)));
            public void DrawLine(double x1, double y1, double x2, double y2, double d, DebugColor color)
                => Merge(MakeRect(Math.Min(x1, x2), Math.Min(y1, y2), Math.Max(x1, x2), Math.Max(y1, y2)));
            public void DrawRectangle(double x, double y, double width, double height, DebugColor color)
                => Merge(MakeRect(x, y, x + width, y + height));

            private void Merge(RectangleF localBounds)
            {
                if (mergedBounds.IsEmpty)
                    mergedBounds = localBounds;
                else
                    mergedBounds = RectangleF.Union(mergedBounds, localBounds);
            }
        }

        private sealed class GraphicsPainter : IDebugPainter
        {
            private readonly Graphics graphics;
            private readonly float unit;
            private readonly Pen sharedPen;

            public GraphicsPainter(Graphics graphics, float unit)
            {
                this.graphics = graphics;
                this.unit = unit;
                this.sharedPen = new Pen(Brushes.Black, unit * 2.0f)
                {
                    LineJoin = LineJoin.Round,
                };
            }

            public void DrawPoint(double x, double y, DebugColor color)
                => graphics.FillEllipse(ToBrush(color), MakeRect(x - (unit * 4), y - (unit * 4), x + (unit * 4), y + (unit * 4)));
            public void DrawLine(double x1, double y1, double x2, double y2, DebugColor color)
                => graphics.DrawLine(GetColoredPen(color), (float)x1, (float)y1, (float)x2, (float)y2);
            public void DrawLine(double x1, double y1, double x2, double y2, double d, DebugColor color)
            {
                if (d != 0.0)
                {
                    var dx = x2 - x1;
                    var dy = y2 - y1;
                    var distance = Math.Sqrt(dx * dx + dy * dy);
                    if (distance > 0.0)
                    {
                        var xx = (x1 + (x2 - x1) * 0.5) + (-dy * (distance * d)) / distance;
                        var yy = (y1 + (y2 - y1) * 0.5) + (+dx * (distance * d)) / distance;
                        graphics.DrawBezier(GetColoredPen(color), (float)x1, (float)y1, (float)xx, (float)yy, (float)xx, (float)yy, (float)x2, (float)y2);
                    }
                }
                else
                    DrawLine(x1, y1, x2, y2, color);
            }
            public void DrawRectangle(double x, double y, double width, double height, DebugColor color)
                => graphics.DrawRectangle(GetColoredPen(color), (float)x, (float)y, (float)width, (float)height);

            private Pen GetColoredPen(DebugColor color)
            {
                sharedPen.Brush = ToBrush(color);
                return sharedPen;
            }

            static Brush ToBrush(DebugColor color)
            {
                switch (color)
                {
                    case DebugColor.Black:
                        return Brushes.Black;
                    case DebugColor.Red:
                        return Brushes.Tomato;
                    case DebugColor.Green:
                        return Brushes.DarkOliveGreen;
                    case DebugColor.Blue:
                        return Brushes.AliceBlue;
                    case DebugColor.Yellow:
                        return Brushes.LightGoldenrodYellow;
                    default:
                        return Brushes.Black;
                }
            }
        }
    }
}
