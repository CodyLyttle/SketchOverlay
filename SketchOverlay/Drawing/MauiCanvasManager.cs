using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Drawing;

public class MauiCanvasManager : CanvasManager<DrawableStack, IDrawable, IDrawable>
{
    public MauiCanvasManager(IDrawingToolRetriever<IDrawable> toolRetriever) : base(toolRetriever)
    {
    }
}