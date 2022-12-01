using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public interface ILineTool<out TDrawing, TColor> : IDrawingTool<TDrawing>
{
    public TColor StrokeColor { get; set; }
    public float StrokeSize { get; set; }
}