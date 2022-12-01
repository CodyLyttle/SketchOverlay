using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Maui.Drawing;

public class MauiCanvasProperties : CanvasProperties<Color>
{
    public MauiCanvasProperties(IColorPalette<Color> drawingColors) : base(drawingColors)
    {
    }
}