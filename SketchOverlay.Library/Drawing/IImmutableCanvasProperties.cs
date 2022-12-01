namespace SketchOverlay.Library.Drawing;

public interface IImmutableCanvasProperties<out TColor>
{
    TColor FillColor { get; }
    TColor StrokeColor { get; }
    float StrokeSize { get; }
}