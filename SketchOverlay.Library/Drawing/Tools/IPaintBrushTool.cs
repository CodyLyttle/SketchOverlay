using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public interface IPaintBrushTool<out TDrawing> : IDrawingTool<TDrawing>
{
     Color StrokeColor { get; set; }
     float StrokeSize { get; set; }
}