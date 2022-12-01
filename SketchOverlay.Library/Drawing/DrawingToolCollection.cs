using System.Collections;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public class DrawingToolCollection<TDrawing, TImageSource, TColor> : IDrawingToolCollection<TDrawing, TImageSource, TColor>,
    IDrawingToolRetriever<TDrawing, TColor>
{
    private readonly DrawingToolInfo<TDrawing, TImageSource, TColor>[] _tools;

    public DrawingToolCollection(IEnumerable<DrawingToolInfo<TDrawing, TImageSource, TColor>> tools)
    {
        _tools = tools.ToArray();
        DefaultTool = _tools[0].Tool;
        SelectedToolInfo = _tools[0];
    }

    public int Count => _tools.Length;

    public IDrawingTool<TDrawing, TColor> DefaultTool { get; }

    public IDrawingTool<TDrawing, TColor> SelectedTool => SelectedToolInfo.Tool;

    public DrawingToolInfo<TDrawing, TImageSource, TColor> SelectedToolInfo { get; set; }

    public TTool GetTool<TTool>() where TTool : IDrawingTool<TDrawing, TColor>
    {
        foreach (DrawingToolInfo<TDrawing, TImageSource, TColor> toolInfo in this)
        {
            if (toolInfo.Tool is TTool tool)
                return tool;
        }

        throw new ArgumentOutOfRangeException(nameof(TTool), 
            $"{typeof(TTool).Name} doesn't exist");
    }

    public IEnumerator<DrawingToolInfo<TDrawing, TImageSource, TColor>> GetEnumerator()
    {
        return ((IEnumerable<DrawingToolInfo<TDrawing, TImageSource, TColor>>)_tools)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}