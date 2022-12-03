using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasManager : CanvasManager<DrawingStack, WpfDrawing, WpfDrawingOutput, WpfColor>
{
    public WpfCanvasManager(ICanvasProperties<WpfColor> canvasProperties,
        IDrawingToolRetriever<WpfDrawing, WpfColor> toolRetriever) 
        : base(canvasProperties, toolRetriever)
    {
    }
}