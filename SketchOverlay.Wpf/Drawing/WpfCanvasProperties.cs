using System.Drawing;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasProperties : CanvasProperties<Color>
{
    public WpfCanvasProperties(IColorPalette<Color> drawingColors) : base(drawingColors)
    {
    }
}