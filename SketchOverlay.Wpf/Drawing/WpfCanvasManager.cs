using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasManager : CanvasManager<DrawingStack, WpfDrawing, WpfDrawingOutput, WpfBrush>
{
    public WpfCanvasManager(ICanvasProperties<WpfBrush> canvasProperties,
        IDrawingToolRetriever<WpfDrawing, WpfBrush> toolRetriever) 
        : base(canvasProperties, toolRetriever)
    {
    }
}