using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Library.Models;

public class DrawingToolInfo<TDrawing, TImageSource>
{
    public DrawingToolInfo(IDrawingTool<TDrawing> tool, TImageSource iconSource, string name)
    {
        Tool = tool;
        IconSource = iconSource;
        Name = name;
    }

    public IDrawingTool<TDrawing> Tool { get; set; }
    public TImageSource IconSource { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
}