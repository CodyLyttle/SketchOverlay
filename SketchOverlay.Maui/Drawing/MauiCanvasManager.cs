using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Maui.Drawing;

public class MauiCanvasManager : CanvasManager<DrawableStack, MauiDrawing, MauiDrawingOutput, MauiColor>
{
    public MauiCanvasManager(ICanvasProperties<MauiColor> canvasProps, 
        IDrawingToolRetriever<MauiDrawing, MauiColor> toolRetriever) : base(canvasProps, toolRetriever)
    {
    }
}