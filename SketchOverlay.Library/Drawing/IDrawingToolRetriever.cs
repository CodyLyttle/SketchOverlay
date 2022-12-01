using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingToolRetriever <out TDrawing, TColor>
{
    public IDrawingTool<TDrawing, TColor> DefaultTool { get; }
    public IDrawingTool<TDrawing, TColor> SelectedTool { get; }
}