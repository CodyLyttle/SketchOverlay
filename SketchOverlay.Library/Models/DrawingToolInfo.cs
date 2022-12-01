using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Models;

public class DrawingToolInfo<TDrawing, TImageSource, TColor>
{
    public DrawingToolInfo(IDrawingTool<TDrawing, TColor> tool, TImageSource iconSource, string name)
    {
        Tool = tool;
        IconSource = iconSource;
        Name = name;
    }

    public IDrawingTool<TDrawing, TColor> Tool { get; set; }
    public TImageSource IconSource { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
}