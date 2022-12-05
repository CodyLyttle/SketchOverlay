using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfCanvasManager : CanvasManager<WpfDrawing, WpfDrawingOutput, WpfBrush>
{
    public WpfCanvasManager(ICanvasProperties<WpfBrush> canvasProperties,
        IDrawingStack<WpfDrawing, WpfDrawingOutput> drawStack,
        IDrawingToolRetriever<WpfDrawing, WpfBrush> toolRetriever) 
        : base(canvasProperties, drawStack, toolRetriever)
    {
    }
}