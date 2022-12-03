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

    public static MauiColor ToMauiColor(this System.Drawing.Color drawingColor)
    {
        return MauiColor.FromInt(drawingColor.ToArgb());
    }

    public static System.Drawing.Color ToDrawingColor(this MauiColor mauiColor)
    {
        return System.Drawing.Color.FromArgb(mauiColor.ToInt());
    }

    public static MauiColor? ToMauiColor(this System.Drawing.Color? drawingColor)
    {
        return drawingColor is null 
            ? null 
            : MauiColor.FromInt(drawingColor.Value.ToArgb());
    }

    public static System.Drawing.Color? ToNullableDrawingColor(this MauiColor? mauiColor)
    {
        if (mauiColor is null) return null;
        return System.Drawing.Color.FromArgb(mauiColor.ToInt());
    }
}