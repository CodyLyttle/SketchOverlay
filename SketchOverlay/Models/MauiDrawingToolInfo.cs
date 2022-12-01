using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Models;

internal class MauiDrawingToolInfo : DrawingToolInfo<IDrawable, ImageSource, Color>
{
    public MauiDrawingToolInfo(IDrawingTool<IDrawable, Color> tool, ImageSource iconSource, string name)
        : base(tool, iconSource, name)
    {
    }
}