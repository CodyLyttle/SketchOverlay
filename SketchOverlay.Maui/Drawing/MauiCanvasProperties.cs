using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Maui.Drawing;

public class MauiCanvasProperties : CanvasProperties<MauiColor>
{
    public MauiCanvasProperties(IColorPalette<MauiColor> drawingColors) : base(drawingColors)
    {
    }
}