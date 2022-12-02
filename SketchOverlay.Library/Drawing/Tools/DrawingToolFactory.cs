using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing.Tools;

public abstract class DrawingToolFactory<TDrawing, TImageSource, TColor>
{
    // We return the concrete collection because it contains multiple interfaces, which simplifies dependency injection.
    public DrawingToolCollection<TDrawing, TImageSource, TColor> CreateDrawingToolCollection()
    {
        List<DrawingToolInfo<TDrawing, TImageSource, TColor>> tools = new();
        AddTool(tools, CreatePaintBrushTool(), "placeholder_paintbrush.png", "Paintbrush");
        AddTool(tools, CreateLineTool(), "placeholder_line.png", "Line");
        AddTool(tools, CreateRectangleTool(), "placeholder_rectangle.png", "Rectangle");

        return new DrawingToolCollection<TDrawing, TImageSource, TColor>(tools);
    }

    private void AddTool(ICollection<DrawingToolInfo<TDrawing, TImageSource, TColor>> toolList,
        IDrawingTool<TDrawing, TColor>? tool, string iconPath, string name, string description = "")
    {
        if (tool is null)
            return;

        TImageSource icon = CreateImageSource(iconPath);
        toolList.Add(new DrawingToolInfo<TDrawing, TImageSource, TColor>(tool, icon, name)
        {
            Description = description
        });
    }

    protected virtual IPaintBrushTool<TDrawing, TColor>? CreatePaintBrushTool()
    {
        return null;
    }

    protected virtual ILineTool<TDrawing, TColor>? CreateLineTool()
    {
        return null;
    }

    protected virtual IRectangleTool<TDrawing, TColor>? CreateRectangleTool()
    {
        return null;
    }

    protected abstract TImageSource CreateImageSource(string fileName);
}