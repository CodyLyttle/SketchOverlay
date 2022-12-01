using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Drawing;

public class MauiCanvasManager : CanvasManager<DrawableStack, IDrawable, IDrawable, Color>
{
    public MauiCanvasManager(ICanvasProperties<Color> canvasProps, 
        IDrawingToolRetriever<IDrawable, Color> toolRetriever) : base(canvasProps, toolRetriever)
    {
    }
}