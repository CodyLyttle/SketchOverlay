using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Maui.Drawing;

public class MauiCanvasManager : CanvasManager<MauiDrawing, MauiDrawingOutput, MauiColor>
{
    public MauiCanvasManager(ICanvasProperties<MauiColor> canvasProps,
        IDrawingStack<MauiDrawing, MauiDrawingOutput> drawStack,
        IDrawingToolRetriever<MauiDrawing, MauiColor> toolRetriever) :
        base(canvasProps, drawStack, toolRetriever)
    {
    }
}