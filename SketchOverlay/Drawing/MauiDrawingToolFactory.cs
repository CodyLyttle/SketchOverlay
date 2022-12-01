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
        return new MauiPaintBrushTool(Colors.Aqua, 16);
    }

    protected override ILineTool<IDrawable, Color> CreateLineTool()
    {
        return new MauiLineTool(Colors.Red, 6);
    }

    protected override IRectangleTool<IDrawable, Color>? CreateRectangleTool()
    {
        return new MauiRectangleTool(Colors.Navy, Colors.Green, 6);
    }
}