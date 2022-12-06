namespace SketchOverlay.Library.Drawing.Canvas;

public interface IImmutableCanvasProperties<out TColor>
{
    TColor DefaultFillColor { get; }
    TColor DefaultStrokeColor { get; }
    TColor FillColor { get; }
    TColor StrokeColor { get; }

    float MinimumStrokeSize { get; }
    float MaximumStrokeSize { get; }
    float DefaultStrokeSize { get; }
    float StrokeSize { get; }
}