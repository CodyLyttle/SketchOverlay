using System;
using System.Windows.Media;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Wpf;

internal static class TypeConverterExtensions
{
    public static System.Windows.Point ToWpfPoint(this System.Drawing.PointF drawingPoint)
    {
        return new System.Windows.Point(drawingPoint.X, drawingPoint.Y);
    }

    public static System.Drawing.PointF ToDrawingPointF(this System.Windows.Point wpfPoint)
    {
        return new System.Drawing.PointF((float)wpfPoint.X, (float)wpfPoint.Y);
    }

    public static Color ToWindowsColor(this System.Drawing.Color drawingColor)
    {
        return Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
    }

    public static System.Drawing.Color ToDrawingColor(this Color windowsColor)
    {
        return System.Drawing.Color.FromArgb(windowsColor.A, windowsColor.R, windowsColor.G, windowsColor.B);
    }

    public static SolidColorBrush ToSolidColorBrush(this Color windowsColor, bool freeze = true)
    {
        SolidColorBrush brush = new(windowsColor);
        if(freeze) brush.Freeze();
        return brush;
    }

    public static Pen ToPen(this Brush brush, float size, bool freeze = true)
    {
        Pen pen = new(brush, size);
        if(freeze) pen.Freeze();
        return pen;
    }

    public static MouseButton ToLibraryMouseButton(this System.Windows.Input.MouseButton button)
    {
        return button switch
        {
            System.Windows.Input.MouseButton.Left => MouseButton.Left,
            System.Windows.Input.MouseButton.Right => MouseButton.Right,
            System.Windows.Input.MouseButton.Middle => MouseButton.Middle,
            System.Windows.Input.MouseButton.XButton1 => MouseButton.XButton1,
            System.Windows.Input.MouseButton.XButton2 => MouseButton.XButton2,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }
}