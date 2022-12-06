namespace SketchOverlay.Library.Drawing.Canvas;

public class CanvasProperties<TColor> : ICanvasProperties<TColor>
{
    public CanvasProperties(IColorPalette<TColor> drawingColors)
    {
        DefaultFillColor = drawingColors.DefaultSecondaryColor;
        DefaultStrokeColor = drawingColors.DefaultPrimaryColor;
        FillColor = drawingColors.SecondaryColor;
        StrokeColor = drawingColors.PrimaryColor;
        StrokeSize = DefaultStrokeSize;
    }
    
    public TColor DefaultFillColor { get; }
    public TColor DefaultStrokeColor { get; }
    public TColor FillColor { get; set; }
    public TColor StrokeColor { get; set; }


    public float MinimumStrokeSize => 1;
    public float MaximumStrokeSize => 64;
    public float DefaultStrokeSize => 4;
    public float StrokeSize { get; set; }

}