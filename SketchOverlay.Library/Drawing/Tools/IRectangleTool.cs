using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public interface IRectangleTool<out TDrawing> : IDrawingTool<TDrawing>
    where TDrawing : class, new()
{
    public Color StrokeColor { get; set; }
    public Color? FillColor { get; set; }
    public float StrokeSize { get; set; }
}