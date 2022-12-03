using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasManager : CanvasManager<DrawingStack, System.Windows.Media.Drawing, DrawingGroup, Color>
{
    public WpfCanvasManager(ICanvasProperties<Color> canvasProperties, IDrawingToolRetriever<System.Windows.Media.Drawing, Color> toolRetriever) : base(canvasProperties, toolRetriever)
    {
    }
}