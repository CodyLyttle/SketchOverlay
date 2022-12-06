namespace SketchOverlay.Library.Drawing.Canvas;

public interface ICanvasProperties<TColor> : IImmutableCanvasProperties<TColor>
{
    new TColor FillColor { get; set; }
    new TColor StrokeColor { get; set; }
    new float StrokeSize { get; set; }
}