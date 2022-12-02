namespace SketchOverlay.Library.Drawing.Canvas;

public interface IImmutableCanvasProperties<out TColor>
{
    TColor FillColor { get; }
    TColor StrokeColor { get; }
    float StrokeSize { get; }
}