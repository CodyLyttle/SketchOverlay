using SketchOverlay.Drawing.Tools;

namespace SketchOverlay.Models;

public class DrawingToolInfo
{
    public DrawingToolInfo(IDrawingTool tool, ImageSource icon, string name)
    {
        Tool = tool;
        Icon = icon;
        Name = name;
    }

    public IDrawingTool Tool { get; set; }
    public ImageSource Icon { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
}