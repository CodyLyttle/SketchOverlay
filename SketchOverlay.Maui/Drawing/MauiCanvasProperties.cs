using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Maui.Drawing;

public class MauiCanvasProperties : CanvasProperties<Color>
{
    public MauiCanvasProperties(IColorPalette<Color> drawingColors) : base(drawingColors)
    {
    }
}