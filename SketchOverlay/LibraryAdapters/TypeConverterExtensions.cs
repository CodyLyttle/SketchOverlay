namespace SketchOverlay.LibraryAdapters;

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
}