using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Drawing;

public class MauiCanvasProperties : CanvasProperties<Color>
{
    public MauiCanvasProperties(IColorPalette<Color> drawingColors) : base(drawingColors)
    {
    }
}