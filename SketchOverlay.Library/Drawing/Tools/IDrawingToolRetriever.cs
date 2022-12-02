namespace SketchOverlay.Library.Drawing.Tools;

public interface IDrawingToolRetriever<out TDrawing, TColor>
{
    public IDrawingTool<TDrawing, TColor> DefaultTool { get; }
    public IDrawingTool<TDrawing, TColor> SelectedTool { get; }
}