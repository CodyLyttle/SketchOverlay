using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public interface IRectangleTool<out TDrawing, TColor> : IDrawingTool<TDrawing>
{
    public TColor StrokeColor { get; set; }
    public TColor FillColor { get; set; }
    public float StrokeSize { get; set; }
}