using SketchOverlay.Drawing.Tools;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Drawing;

internal class MauiDrawingToolFactory : DrawingToolFactory<IDrawable, ImageSource>
{
    protected override ImageSource CreateImageSource(string fileName)
    {
        return ImageSource.FromFile(fileName);
    }

    protected override IPaintBrushTool<IDrawable> CreatePaintBrushTool()
    {
        return new MauiPaintBrushTool(System.Drawing.Color.Aqua, 16);
    }

    protected override ILineTool<IDrawable> CreateLineTool()
    {
        return new MauiLineTool(System.Drawing.Color.BlanchedAlmond, 6);
    }

    protected override IRectangleTool<IDrawable>? CreateRectangleTool()
    {
        return new MauiRectangleTool(System.Drawing.Color.Navy, System.Drawing.Color.Green, 6);
    }
}