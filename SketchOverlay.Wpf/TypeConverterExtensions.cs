using System.Windows.Media;

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
}