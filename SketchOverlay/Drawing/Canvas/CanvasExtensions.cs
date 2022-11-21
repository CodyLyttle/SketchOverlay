namespace SketchOverlay.Drawing.Canvas;

internal static class CanvasExtensions
{
    public static void SetProperties(this ICanvas canvas, CanvasProperties properties)
    {
        canvas.FillColor = properties.FillColor;
        canvas.StrokeColor = properties.StrokeColor;
        canvas.StrokeSize = properties.StrokeSize;
    }
}