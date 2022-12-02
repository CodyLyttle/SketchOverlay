using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Maui.Drawing;

public class MauiCanvasManager : CanvasManager<DrawableStack, IDrawable, IDrawable, Color>
{
    public MauiCanvasManager(ICanvasProperties<Color> canvasProps, 
        IDrawingToolRetriever<IDrawable, Color> toolRetriever) : base(canvasProps, toolRetriever)
    {
    }
}