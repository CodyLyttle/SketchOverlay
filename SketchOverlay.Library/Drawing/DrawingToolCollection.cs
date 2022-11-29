using System.Collections;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public class DrawingToolCollection<TDrawing, TImageSource> : IDrawingToolCollection<TDrawing, TImageSource>,
    IDrawingToolRetriever<TDrawing>
{
    private readonly DrawingToolInfo<TDrawing, TImageSource>[] _tools;

    public DrawingToolCollection(IEnumerable<DrawingToolInfo<TDrawing, TImageSource>> tools)
    {
        _tools = tools.ToArray();
        DefaultTool = _tools[0].Tool;
        SelectedToolInfo = _tools[0];
    }

    public int Count => _tools.Length;

    public IDrawingTool<TDrawing> DefaultTool { get; }

    public IDrawingTool<TDrawing> SelectedTool => SelectedToolInfo.Tool;

    public DrawingToolInfo<TDrawing, TImageSource> SelectedToolInfo { get; set; }

    public IEnumerator<DrawingToolInfo<TDrawing, TImageSource>> GetEnumerator()
    {
        return ((IEnumerable<DrawingToolInfo<TDrawing, TImageSource>>)_tools)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}