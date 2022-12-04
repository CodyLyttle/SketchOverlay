namespace SketchOverlay.Library.Drawing.Tools;

public interface IEllipseTool<out TDrawing, TColor> : IDrawingTool<TDrawing, TColor>
{
    // Keep empty tool interfaces until we're sure that tools won't require their own properties & logic.
    // Tearing them down would require refactoring DrawingToolFactory.
}