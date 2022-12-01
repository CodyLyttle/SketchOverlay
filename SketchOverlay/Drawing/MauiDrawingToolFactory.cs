using SketchOverlay.Drawing.Tools;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Drawing;

internal class MauiDrawingToolFactory : DrawingToolFactory<IDrawable, ImageSource, Color>
{
    protected override ImageSource CreateImageSource(string fileName)
    {
        return ImageSource.FromFile(fileName);
    }

    protected override IPaintBrushTool<IDrawable, Color> CreatePaintBrushTool()
    {
        return new MauiPaintBrushTool();
    }

    protected override ILineTool<IDrawable, Color> CreateLineTool()
    {
        return new MauiLineTool();
    }

    protected override IRectangleTool<IDrawable, Color>? CreateRectangleTool()
    {
        return new MauiRectangleTool();
    }
}