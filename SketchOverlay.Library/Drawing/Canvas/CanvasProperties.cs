namespace SketchOverlay.Library.Drawing.Canvas;

public class CanvasProperties<TColor> : ICanvasProperties<TColor>
{
    private readonly IColorPalette<TColor> _drawingColors;
    private const float DefaultStrokeSize = 4;

    public CanvasProperties(IColorPalette<TColor> drawingColors)
    {
        _drawingColors = drawingColors;
        ResetDefaults();
    }

    // Suppress nullable warnings. Properties are set on instantiation via ResetDefaults()
    public TColor FillColor { get; set; } = default!;
    public TColor StrokeColor { get; set; } = default!;
    public float StrokeSize { get; set; }

    public void ResetDefaults()
    {
        FillColor = _drawingColors.DefaultSecondaryColor;
        StrokeColor = _drawingColors.DefaultPrimaryColor;
        StrokeSize = DefaultStrokeSize;
    }
}