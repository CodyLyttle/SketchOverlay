using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Maui.Models;

internal class MauiDrawingToolInfo : DrawingToolInfo<MauiDrawing, MauiImageSource, MauiColor>
{
    public MauiDrawingToolInfo(IDrawingTool<MauiDrawing, MauiColor> tool, MauiImageSource iconSource, string name)
        : base(tool, iconSource, name)
    {
    }
}