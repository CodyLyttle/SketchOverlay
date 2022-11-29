using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public interface ILineTool<out TDrawing> : IDrawingTool<TDrawing>
{
    public Color StrokeColor { get; set; }
    public float StrokeSize { get; set; }
}