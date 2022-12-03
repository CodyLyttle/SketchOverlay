using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasProperties : CanvasProperties<WpfBrush>
{
    public WpfCanvasProperties(IColorPalette<WpfBrush> drawingColors) : base(drawingColors)
    {
    }
}