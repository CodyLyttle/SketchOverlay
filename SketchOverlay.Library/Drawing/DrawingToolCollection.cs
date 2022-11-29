using System.Collections;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Drawing;

public class DrawingToolCollection<TDrawing, TImageSource> : IDrawingToolCollection<TDrawing, TImageSource>,
    IDrawingToolRetriever<TDrawing>
{
    private readonly DrawingToolInfo<TDrawing, TImageSource>[] _items;

    public DrawingToolCollection(params DrawingToolInfo<TDrawing, TImageSource>[] items)
    {
        _items = items;
        DefaultTool = _items[0].Tool;
        SelectedToolInfo = _items[0];
    }

    public int Count => _items.Length;

    public IDrawingTool<TDrawing> DefaultTool { get; }

    public IDrawingTool<TDrawing> SelectedTool => SelectedToolInfo.Tool;

    public DrawingToolInfo<TDrawing, TImageSource> SelectedToolInfo { get; set; }

    public IEnumerator<DrawingToolInfo<TDrawing, TImageSource>> GetEnumerator()
    {
        return ((IEnumerable<DrawingToolInfo<TDrawing, TImageSource>>)_items)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}