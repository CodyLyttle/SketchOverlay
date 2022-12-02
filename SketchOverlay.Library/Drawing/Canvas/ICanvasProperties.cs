namespace SketchOverlay.Library.Drawing.Canvas;

public interface ICanvasProperties<TColor> : IImmutableCanvasProperties<TColor>
{
    public const float MinimumStrokeSize = 1;
    public const float MaximumStrokeSize = 64;

    new TColor FillColor { get; set; }
    new TColor StrokeColor { get; set; }
    new float StrokeSize { get; set; }

    void ResetDefaults();
}