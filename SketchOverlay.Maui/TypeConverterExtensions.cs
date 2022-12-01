namespace SketchOverlay.Maui;

internal static class LibraryAdapterExtensions
{
    public static PointF ToMauiPointF(this System.Drawing.PointF drawingPoint)
    {
        return new PointF(drawingPoint.X, drawingPoint.Y);
    }

    public static System.Drawing.PointF ToDrawingPointF(this PointF mauiPoint)
    {
        return new System.Drawing.PointF(mauiPoint.X, mauiPoint.Y);
    }

    public static Color ToMauiColor(this System.Drawing.Color drawingColor)
    {
        return Color.FromInt(drawingColor.ToArgb());
    }

    public static System.Drawing.Color ToDrawingColor(this Color mauiColor)
    {
        return System.Drawing.Color.FromArgb(mauiColor.ToInt());
    }

    public static Color? ToMauiColor(this System.Drawing.Color? drawingColor)
    {
        if (drawingColor == null) return null;
        return Color.FromInt(drawingColor.Value.ToArgb());
    }

    public static System.Drawing.Color? ToNullableDrawingColor(this Color? mauiColor)
    {
        if (mauiColor is null) return null;
        return System.Drawing.Color.FromArgb(mauiColor.ToInt());
    }
}