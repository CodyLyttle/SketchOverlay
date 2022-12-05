using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Tools;

namespace SketchOverlay.Maui.Drawing;

internal class MauiDrawingToolFactory : DrawingToolFactory<MauiDrawing, MauiImageSource, MauiColor>
{
    protected override MauiImageSource CreateImageSource(string fileName)
    {
        return ImageSource.FromFile(fileName);
    }

    protected override IEllipseTool<IDrawable, Color>? CreateEllipseTool()
    {
        return new MauiEllipseTool();
    }

    protected override ILineTool<MauiDrawing, MauiColor> CreateLineTool()
    {
        return new MauiLineTool();
    }

    protected override IPaintBrushTool<MauiDrawing, MauiColor> CreatePaintBrushTool()
    {
        return new MauiPaintBrushTool();
    }

    protected override IRectangleTool<MauiDrawing, MauiColor>? CreateRectangleTool()
    {
        return new MauiRectangleTool();
    }
}