using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasProperties : CanvasProperties<WpfColor>
{
    public WpfCanvasProperties(IColorPalette<WpfColor> drawingColors) : base(drawingColors)
    {
    }
}