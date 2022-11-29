using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public abstract class DrawingToolFactory<TDrawing, TImageSource>
{
    // We return the concrete collection because it contains multiple interfaces, which simplifies dependency injection.
    public DrawingToolCollection<TDrawing, TImageSource> CreateDrawingToolCollection()
    {
        List<DrawingToolInfo<TDrawing, TImageSource>> tools = new();
        AddTool(tools, CreatePaintBrushTool(), "placeholder_paintbrush.png", "Paintbrush");
        AddTool(tools, CreateLineTool(), "placeholder_line.png", "Line");
        AddTool(tools, CreateRectangleTool(), "placeholder_rectangle.png", "Rectangle");

        return new DrawingToolCollection<TDrawing, TImageSource>(tools);
    }

    private void AddTool(ICollection<DrawingToolInfo<TDrawing, TImageSource>> toolList,
        IDrawingTool<TDrawing>? tool, string iconPath, string name, string description = "")
    {
        if (tool is null)
            return;

        TImageSource icon = CreateImageSource(iconPath);
        toolList.Add(new DrawingToolInfo<TDrawing, TImageSource>(tool, icon, name)
        {
            Description = description
        });
    }

    protected virtual IPaintBrushTool<TDrawing>? CreatePaintBrushTool()
    {
        return null;
    }

    protected virtual ILineTool<TDrawing>? CreateLineTool()
    {
        return null;
    }

    protected virtual IRectangleTool<TDrawing>? CreateRectangleTool()
    {
        return null;
    }

    protected abstract TImageSource CreateImageSource(string fileName);
}