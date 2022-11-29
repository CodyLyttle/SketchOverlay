using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingToolRetriever <out TDrawing>
{
    public IDrawingTool<TDrawing> DefaultTool { get; }
    public IDrawingTool<TDrawing> SelectedTool { get; }
}